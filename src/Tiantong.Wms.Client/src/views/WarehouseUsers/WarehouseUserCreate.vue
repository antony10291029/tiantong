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
import { WarehouseUser } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'

@Component({
  name: 'WarehouseUserCreate',
  components: {
    AsyncButton,
    UserEmailField,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  warehouseUser: WarehouseUser = new WarehouseUser()

  handleClose () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    await axios.post('/warehouses/users/create', this.warehouseUser)
    this.$emit('updated')
    this.handleClose()
  }

  created () {
    this.warehouseUser.warehouse_id = this.warehouseId
  }
}
</script>
