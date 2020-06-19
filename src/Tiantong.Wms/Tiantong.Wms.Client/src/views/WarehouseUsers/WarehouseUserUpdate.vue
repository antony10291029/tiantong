<template>
  <AsyncLoader
    :handler="handleFind"
    class="modal is-active"
  >
    <div class="modal-background"></div>
    <div class="modal-card" style="width: 400px">
      <header class="modal-card-head">
        <p class="modal-card-title">
          编辑用户
        </p>
      </header>
      <section class="modal-card-body">
        <UserEmailField
          v-model="currentWarehouseUser.user.email"
        ></UserEmailField>

        <div class="field">
          <div class="label">
            <label class="label">姓名</label>
          </div>
          <div class="control">
            <input
              v-model="currentWarehouseUser.user.name"
              type="text" class="input"
            >
          </div>
        </div>

        <div class="field">
          <label class="label">部门</label>
          <div class="control">
            <DepartmentSelector
              :type="null"
              :warehouseId="warehouseId"
              :department="currentWarehouseUser.department"
              @select="handleDepartmentSelect"
            />
          </div>
        </div>
      </section>
      <footer class="modal-card-foot is-flex">
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
        <div class="is-flex-auto"></div>
        <AsyncButton
          :handler="HandleDelete"
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
import UserEmailField from './UserEmailField.vue'
import axios from '../../providers/axios'
import { WarehouseUser, Department } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import cloneDeep from 'lodash/cloneDeep'
import DepartmentSelector from '@/views/common/DepartmentSelector.vue'

@Component({
  name: 'WarehouseUserUpdate',
  components: {
    AsyncButton,
    AsyncLoader,
    UserEmailField,
    DepartmentSelector,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  warehouseUserId!: number

  warehouseUser: WarehouseUser = new WarehouseUser()

  currentWarehouseUser: WarehouseUser = new WarehouseUser()

  handleClose () {
    this.$router.go(-1)
  }

  handleReset () {
    this.currentWarehouseUser = cloneDeep(this.warehouseUser)
  }

  handleDepartmentSelect (department: Department) {
    this.currentWarehouseUser.department = department
    this.currentWarehouseUser.department_id = department.id
  }

  async handleFind () {
    var response = await axios.post('/warehouses/users/find', {
      id: this.warehouseUserId
    })

    this.warehouseUser = response.data
    this.handleReset()
  }

  async handleSubmit () {
    const param = cloneDeep(this.currentWarehouseUser)
    param.department = null
    await axios.post('/warehouses/users/update', this.currentWarehouseUser)
    this.$emit('updated')
    this.handleClose()
  }

  async HandleDelete () {
    this.$confirm({
      title: '提示',
      content: '删除后将无法恢复',
      handler: async () => {
        await axios.post('/warehouses/users/delete', {
          id: this.warehouseUserId
        })
        this.$emit('updated')
        this.handleClose()
      }
    })
  }

  created () {
    this.warehouseUser.warehouse_id = this.warehouseId
  }
}
</script>
