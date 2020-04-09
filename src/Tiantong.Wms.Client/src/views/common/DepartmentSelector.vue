<template>
  <div>
    <input
      readonly
      type="text" class="input"
      v-style:cursor="'pointer'"
      @click="isShow = !isShow"
      :value="department === null ? '' :  department.name"
    >
    <AsyncLoader
      v-if="isShow"
      class="modal is-active"
      :handler="getDepartments"
    >
      <div
        @click="handleClose"
        class="modal-background"
      ></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title has-text-centered">
            选择供应商
          </p>
        </header>
        <div class="modal-card-body">
          <Table class="is-hoverable">
            <thead>
              <tr>
                <th style="width: 1px">#</th>
                <th>供应商名</th>
                <th>备注</th>
                <th style="width: 1px">
                  <span
                    class="icon"
                    style="color: transparent"
                  >
                    <i class="iconfont icon-success"></i>
                  </span>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(department, index) in departments" :key="department.id"
                @click="currentDepartment = department"
              >
                <td>{{index + 1}}</td>
                <td>{{department.name}}</td>
                <td>{{department.comment}}</td>
                <td>
                  <span
                    v-if="currentDepartment && currentDepartment.id === department.id"
                    class="icon has-text-success"
                  >
                    <i class="iconfont icon-tick"></i>
                  </span>
                </td>
              </tr>
            </tbody>
          </Table>
        </div>
        <footer class="modal-card-foot">
          <a
            class="button is-success"
            @click.stop="handleSelect"
          >
            选择
          </a>
          <a
            class="button"
            @click.stop="handleClose"
          >
            取消
          </a>
        </footer>
      </div>
    </AsyncLoader>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { Department } from '@/Entities'
import axios from '@/providers/axios'
import Table from '@/components/Table.vue'

@Component({
  name: 'PurchaseOrderDepartmentSelector',
  components: {
    Table,
    AsyncLoader
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  department!: Department | null

  currentDepartment: Department | null = null

  isShow: boolean = false

  entities: {
    keys: Array<number>
    data: { [ key: string ]: Department }
  } = { keys: [], data: {} }

  get departments () {
    return this.entities.keys.map(key => this.entities.data[key])
  }

  async getDepartments () {
    let response = await axios.post('/departments/all', {
      warehouse_id: this.warehouseId
    })

    this.entities = response.data
  }

  handleSelect () {
    this.$emit('select', this.currentDepartment)
    this.handleClose()
  }

  handleClose () {
    this.isShow = false
  }

  created () {
    this.currentDepartment = this.department
  }
}
</script>
