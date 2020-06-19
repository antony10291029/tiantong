<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card" style="width: 400px">
      <header class="modal-card-head">
        <p class="modal-card-title">
          添加用户
        </p>
      </header>
      <section class="modal-card-body">
        <UserEmailField
          v-model="warehouseUser.user.email"
        ></UserEmailField>

        <div class="field">
          <div class="label">
            <label class="label">姓名</label>
          </div>
          <div class="control">
            <input
              v-model="warehouseUser.user.name"
              type="text" class="input"
            >
          </div>
        </div>

        <div class="field">
          <label class="label">部门</label>
          <div class="control">
            <DepartmentSelector
              :warehouseId="warehouseId"
              :department="warehouseUser.department"
              @select="handleDepartmentSelect"
            />
          </div>
        </div>

      </section>
      <footer class="modal-card-foot">
        <AsyncButton
          :handler="handleSubmit"
          class="button is-success"
        >
          提交
        </AsyncButton>
        <a
          class="button"
          @click="handleClose"
        >取消</a>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import UserEmailField from './UserEmailField.vue'
import axios from '../../providers/axios'
import { WarehouseUser, Department } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'
import DepartmentSelector from '@/views/common/DepartmentSelector.vue'
import cloneDeep from 'lodash/cloneDeep'

@Component({
  name: 'WarehouseUserCreate',
  components: {
    AsyncButton,
    UserEmailField,
    DepartmentSelector
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  warehouseUser: WarehouseUser = new WarehouseUser()

  handleClose () {
    this.$router.go(-1)
  }

  handleDepartmentSelect (department: Department) {
    this.warehouseUser.department = department
    this.warehouseUser.department_id = department.id
  }

  async handleSubmit () {
    const param = cloneDeep(this.warehouseUser)
    param.department = null
    await axios.post('/warehouses/users/create', param)
    this.$emit('updated')
    this.handleClose()
  }

  created () {
    this.warehouseUser.warehouse_id = this.warehouseId
  }
}
</script>
