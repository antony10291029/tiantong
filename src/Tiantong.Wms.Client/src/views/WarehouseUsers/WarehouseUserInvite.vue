<template>
  <div class="modal is-active">
    <div
      class="modal-background"
      @click="handleClose"
    ></div>
    <div class="modal-card" style="width: 400px">
      <header class="modal-card-head">
        <p class="modal-card-title">
          添加用户
        </p>
      </header>
      <section class="modal-card-body">
        <div class="field">
          <label class="label">邮箱</label>
          <div
            class="control"
            v-class:is-loading="isLoadingUser"
          >
            <input
              :readonly="isLoadingUser"
              class="input" type="text"
              v-model.lazy="params.email"
              @change="handleEmailChange"
            >
          </div>
        </div>

        <div class="field">
          <label class="label">部门</label>
          <div class="control">
            <DepartmentSelector
              :warehouseId="warehouseId"
              :department="department"
              @select="handleDepartmentSelect"
            />
          </div>
        </div>

        <div class="field">
          <label class="label">姓名</label>
          <div class="control">
            <input
              v-model.lazy="params.name"
              type="text" class="input"
              :readonly="user !== null"
            >
          </div>
        </div>

      </section>
      <footer class="modal-card-foot">
        <AsyncButton
          :handler="handleSubmit"
          class="button is-success"
        >确认</AsyncButton>
        <a class="button">取消</a>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import UserEmailField from './UserEmailField.vue'
import { WarehouseUser, Department, User } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'
import DepartmentSelector from '@/views/common/DepartmentSelector.vue'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'

@Component({
  name: 'WarehouseUserInvite',
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

  params = {
    email: '',
    name: '',
    department_id: 0
  }

  user: User | null = null

  department: Department | null = null

  isLoadingUser: boolean = false

  handleClose () {
    this.$router.go(-1)
  }

  handleDepartmentSelect (department: Department) {
    this.department = department
    this.params.department_id = department.id
  }

  async handleSubmit () {
    await axios.post('/warehouses/users/create', this.params)
    this.$emit('updated')
    this.handleClose()
  }

  async handleEmailChange () {
    try {
      this.isLoadingUser = true
      let response = await axios.post('/users/find/email', {
        email: this.params.email
      })

      this.user = response.data.user
      if (this.user !== null) {
        this.params.name = this.user.name
      }
    } finally {
      this.isLoadingUser = false
    }
  }

  created () {
    this.warehouseUser.warehouse_id = this.warehouseId
  }
}
</script>
