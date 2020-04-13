<template>
  <div>
    <input
      readonly
      type="text" class="input"
      v-style:cursor="'pointer'"
      @click="isShow = !isShow"
      :value="user === null ? '' :  user.name"
    >
    <AsyncLoader
      v-if="isShow && !readonly"
      class="modal is-active"
      :handler="getWarehouseUsers"
    >
      <div
        @click="handleClose"
        class="modal-background"
      ></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">
            选择申请人
          </p>
        </header>
        <div class="modal-card-body">
          <Table class="is-hoverable">
            <thead>
              <tr>
                <th style="width: 1px">#</th>
                <th>邮箱</th>
                <th>姓名</th>
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
                v-for="({ user }, index) in warehouseUsers"
                :key="user.id"
                @click="currentUser = user"
              >
                <td>{{index + 1}}</td>
                <td>{{user.email}}</td>
                <td>{{user.name}}</td>
                <td>
                  <span
                    v-if="currentUser.id === user.id"
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
          >选择</a>
        </footer>
      </div>
    </AsyncLoader>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { Department, WarehouseUser, User } from '@/Entities'
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
  user!: User

  @Prop({ default: false })
  readonly!: boolean

  currentUser: User = this.user

  isShow: boolean = false

  entities: {
    keys: number[]
    data: { [ key: string ]: WarehouseUser }
  } = { keys: [], data: {} }

  get warehouseUsers () {
    return this.entities.keys.map(key => this.entities.data[key])
  }

  async getWarehouseUsers () {
    let response = await axios.post('/warehouses/users/all', {
      warehouse_id: this.warehouseId
    })

    this.entities = response.data
  }

  handleSelect () {
    this.$emit('select', this.currentUser)
    this.handleClose()
  }

  handleClose () {
    this.isShow = false
  }
}
</script>
