<template>
  <OrderItem
    :entity="entity"
    :warehouseId="warehouseId"
  >
    <template #header>
      <nav class="breadcrumb is-medium">
        <ul>
          <li>
            <router-link :to="baseURL">
              订单
            </router-link>
          </li>
          <li class="is-active">
            <a>添加订单</a>
          </li>
        </ul>
      </nav>

      <hr>
    </template>

    <template #footer>
      <div class="field" style="width: 120px">
        <div class="control">
          <AsyncButton
            class="button is-info"
            :handler="handleSubmit"
          >提交</AsyncButton>
        </div>
      </div>
    </template>
  </OrderItem>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { OrderEntity } from './OrderItem/OrderEntity'
import OrderItem from './OrderItem/index.vue'
import { dragscroll } from 'vue-dragscroll'
import axios from '@/providers/axios'
import AsyncButton from '@/components/AsyncButton.vue'
import {
  User,
  Good,
  Item,
  Project,
  Supplier,
  Department,
  PurchaseOrder,
  PurchaseOrderPayment,
  PurchaseOrderItem,
  PurchaseOrderItemProject,
} from '@/Entities'

@Component({
  name: 'PurchaseOrderCreate',
  directives: {
    dragscroll
  },
  components: {
    AsyncButton,
    OrderItem
  }
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  warehouseId!: number

  entity: OrderEntity = new OrderEntity()

  handleClose () {
    this.$router.push(this.baseURL)
  }

  async handleSubmit () {
    let { data: { id } } = await this.entity.create()

    this.$router.push(this.baseURL + `/${id}/detail`)
  }

  created () {
    this.entity.order.warehouse_id = this.warehouseId
  }
}
</script>
