<template>
  <AsyncLoader
    class="modal is-active"
    :handler="getDepartment"
  >
    <div class="modal-background"></div>
    <div
      class="modal-card"
      style="width: 400px"
    >
      <header class="modal-card-head">
        <p class="modal-card-title">添加部门</p>
      </header>
      <section class="modal-card-body">
        <div class="field">
          <label class="label">部门名称</label>
          <div class="control">
            <input
              v-model="currentDepartment.name"
              type="text" class="input"
            >
          </div>
        </div>
        <div class="field">
          <label class="label">备注</label>
          <div class="control">
            <input
              v-model="currentDepartment.comment"
              type="text" class="input"
            >
          </div>
        </div>
      </section>
      <footer class="modal-card-foot is-flex">
        <AsyncButton
          :handler="handleSave"
          class="button is-success"
        >
          提交
        </AsyncButton>
        <a
          class="button"
          @click="handleClose"
        >
          取消
        </a>
        <div class="is-flex-auto"></div>
        <AsyncButton
          :handler="handleDelete"
          class="button is-danger is-light"
        >
          删除
        </AsyncButton>
      </footer>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Department } from '../../Entities'
import AsyncLoader from '@/components/AsyncLoader.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'

@Component({
  name: 'DepartmentUpdate',
  components: {
    AsyncLoader,
    AsyncButton,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  departmentId!: number

  department: Department = new Department()

  currentDepartment: Department = new Department()

  handleClose () {
    this.$router.go(-1)
  }

  handleReset () {
    this.currentDepartment = cloneDeep(this.department)
  }

  async handleSave () {
    await axios.post('departments/update', this.currentDepartment)
    this.$emit('updated')
    this.handleClose()
  }

  async handleDelete () {
    this.$confirm({
      title: '提示',
      content: '删除后将无法恢复',
      handler: async () => {
        await axios.post('departments/delete', { id: this. departmentId})
        this.$emit('updated')
        this.handleClose()
      }
    })

  }

  async getDepartment() {
    let response = await axios.post('/departments/find', {
      id: this.departmentId
    })

    this.department = response.data
    this.handleReset()
  }
}
</script>
