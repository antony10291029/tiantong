<template>
  <div class="modal is-active">
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
              v-model="department.name"
              type="text" class="input"
            >
          </div>
        </div>
        <div class="field">
          <label class="label">备注</label>
          <div class="control">
            <input
              v-model="department.comment"
              type="text" class="input"
            >
          </div>
        </div>
      </section>
      <footer class="modal-card-foot">
        <a
          @click="handleSubmit"
          class="button is-success"
        >
          提交
        </a>
        <a
          class="button"
          @click="handleClose"
        >
          取消
        </a>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Department } from '../../Entities'
import axios from '@/providers/axios'

@Component({
  name: 'DepartmentCreate'
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  department: Department = new Department()

  async handleSubmit () {
    await axios.post('departments/create', this.department)
    this.$emit('created')
    this.handleClose()
  }

  handleClose () {
    this.$router.go(-1)
  }

  created () {
    this.department.warehouse_id = this.warehouseId
  }
}
</script>
