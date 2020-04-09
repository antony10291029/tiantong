<template>
  <PurchaseOrderTemplate
    :entity="orderEntity"
    :loader="getOrderEntity"
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
            <a>订单详情</a>
          </li>
        </ul>
      </nav>

      <hr>
    </template>
    <template #footer>
      <AsyncButton
        class="button is-info"
        :handler="handleSubmit"
      >
        保存
      </AsyncButton>
    </template>
  </PurchaseOrderTemplate>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '../../providers/axios'
import { IPurchaseOrderEntity } from './PurchaseOrderTemplate/IPurchaseOrderEntity'
import PurchaseOrderTemplate from './PurchaseOrderTemplate/index.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import cloneDeep from 'lodash/cloneDeep'
import {
  User, Supplier, Project, Good, Item,
  Department, PurchaseOrder, PurchaseOrderItem,
} from '../../Entities'

@Component({
  name: 'PurchaseOrderUpdate',
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
  
  @Prop({ required: true })
  orderId!: number

  orderEntity: IPurchaseOrderEntity = {
    order: new PurchaseOrder,
    supplier: new Supplier,
    department: new Department,
    applicant: new User,
    goods: { 0: new Good },
    items: { 0: new Item },
    projects: { 0: new Project },
  }

  sourceData: PurchaseOrder = new PurchaseOrder

  async getOrderEntity () {
    var { data: entity } = await axios.post('purchase-orders/find', {
      id: this.orderId,
      warehouse_id: this.warehouseId,
    })

    this.orderEntity.order = entity.order
    this.orderEntity.supplier = entity.supplier
    this.orderEntity.applicant = entity.applicant
    this.orderEntity.department = entity.department
    let attrs = ['goods', 'items', 'projects']
    attrs.forEach(attr => {
      for (let id in entity[attr]) {
        this.$set((this.orderEntity as any)[attr], id, entity[attr][id])
      }
    })
    this.sourceData = cloneDeep(this.orderEntity.order)
  }

  async handleSubmit () {
    await axios.post('/purchase-orders/update', this.orderEntity.order)
  }
}
</script>
