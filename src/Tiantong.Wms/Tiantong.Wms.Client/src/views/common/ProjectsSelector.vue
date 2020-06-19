<template>
  <td
    :class="isShow || 'is-clickable'"
    @click="!readonly && (isShow = true)"
  >
    <slot></slot>
    <template v-if="isShow && !readonly">
      <AsyncLoader
        :handler="getProjects"
        class="modal is-active"
        style="position: fixed"
      >
        <div
          @click.stop="handleClose"
          class="modal-background"
        ></div>
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">
              选择工程
            </p>
          </header>

          <section class="modal-card-body">
            <table class="table is-nowrap is-fullwidth is-hoverable is-bordered is-centered">
              <thead>
                <th style="width: 1px"></th>
                <th>工程编号</th>
                <th>工程名</th>
                <th>数量</th>
              </thead>
              <tbody>
                <tr v-for="(project) in projects" :key="project.id">
                  <td
                    style="width: 1px"
                    @click="project.$isSelected = !project.$isSelected"
                  >
                    <Checkbox :value="project.$isSelected"></Checkbox>
                  </td>
                  <td>
                    {{project.number}}
                  </td>
                  <td>
                    {{project.name}}
                  </td>
                  <EditableCell
                    type="number"
                    v-model="project.$quantity"
                  ></EditableCell>
                </tr>
              </tbody>
            </table>
          </section>
          <footer class="modal-card-foot">
            <a
              class="button is-success"
              @click.stop="handleSubmit"
            >选择</a>
          </footer>
        </div>
      </AsyncLoader>
    </template>
  </td>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import YesOrNoCell from '@/components/YesOrNoCell.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import EditableCell from '@/components/EditableCell.vue'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'
import { Project as BaseProject, OrderItem, OrderItemProject } from '@/Entities'

class Project extends BaseProject {
  $isSelected = false
  $quantity = 0
}

@Component({
  name: 'OrderItemProjectsSelector',
  components: {
    AsyncLoader,
    YesOrNoCell,
    EditableCell
  }
})
export default class extends Vue {
  @Prop({ required: true })
  orderItem!: OrderItem

  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  itemProjects!: Array<OrderItemProject>

  @Prop({ default: false })
  readonly!: boolean

  isShow: boolean = false

  entities: {
    keys: Array<number>
    data: { [ key: string ]: Project }
  } = { keys: [], data: {} }

  isPending: boolean = false

  get projects () {
    return this.entities.keys.map(key => this.entities.data[key])
  }

  async getProjects () {
    let response = await axios.post('/projects/all', {
      warehouse_id: this.warehouseId
    })
    let entities = response.data

    entities.keys.forEach((key: number) => {
      entities.data[key].$isSelected = false
      entities.data[key].$quantity = 0
    })

    this.itemProjects.forEach(itemProject => {
      entities.data[itemProject.project_id].$isSelected = true
      entities.data[itemProject.project_id].$quantity = itemProject.quantity
    })

    this.entities = entities
  }

  handleProjectSelect (project: Project, index: number) {
    project.$isSelected = true
  }

  handleClose () {
    this.isShow = false
  }

  handleSubmit () {
    let itemProjects = this.projects
      .filter(project => project.$isSelected)
      .map((project, index) => {
        let itemProject = new OrderItemProject()

        itemProject.index = index
        itemProject.project_id = project.id
        itemProject.quantity = project.$quantity
        itemProject.order_item_id = this.orderItem.id

        return itemProject
      })

    let projects = itemProjects.map(itemProject => this.entities.data[itemProject.project_id])

    this.$emit('change', { itemProjects, projects })
    this.handleClose()
  }
}
</script>
