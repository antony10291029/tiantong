<template>
  <PurchaseOrderTemplate
    :entity="orderEntity"
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
  </PurchaseOrderTemplate>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { IPurchaseOrderEntity } from './PurchaseOrderTemplate/IPurchaseOrderEntity'
import PurchaseOrderTemplate from './PurchaseOrderTemplate/index.vue'
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
    PurchaseOrderTemplate
  }
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  warehouseId!: number

  orderEntity: IPurchaseOrderEntity = {
    order: new PurchaseOrder(
      [ new PurchaseOrderPayment ],
      [ new PurchaseOrderItem ],
    ),
    items: { 0: new Item },
    goods: { 0: new Good },
    projects: { 0: new Project },
    supplier: new Supplier,
    department: new Department,
    applicant: new User,
  }

  async handleSubmit () {
    await axios.post('/purchase-orders/create', this.orderEntity.order)
  }

  created () {
    this.orderEntity.order.warehouse_id = this.warehouseId
  }
}
</script>
