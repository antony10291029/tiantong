<template>
  <OrderItem
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
      <div
        class="field is-grouped"
        style="width: 1px"
      >
        <template v-if="sourceData.order.status === 'created'">
          <div class="control">
            <AsyncButton
              class="button is-info"
              :disabled="!isChanged"
              :handler="handleUpdate"
            >保存</AsyncButton>
          </div>
          <FinishButton
            class="control"
            :orderId="orderId"
            :entity="orderEntity"
            :warehouseId="warehouseId"
            @finished="getOrderEntity"
          />
          <a
            class="button is-danger is-light"
            @click="handleDelete"
          >删除</a>
        </template>
        <template v-else>
          <div
            class="control"
          >
            <AsyncButton
              v-if="orderEntity.order.status === 'filed'"
              class="button is-info"
              :handler="handleRestore"
            >恢复</AsyncButton>
            <AsyncButton
              v-else
              class="button is-success"
              :handler="handleFile"
            >归档</AsyncButton>
          </div>
        </template>
      </div>
    </template>
  </OrderItem>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { OrderEntity } from './OrderItem/OrderEntity'
import OrderItem from './OrderItem/index.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import FinishButton from './OrderFinishButton.vue'
import {
  User, Supplier, Project, Good, Item,
  Department, PurchaseOrder, PurchaseOrderItem,
} from '../../Entities'

@Component({
  name: 'PurchaseOrderDetail',
  components: {
    AsyncButton,
    FinishButton,
    OrderItem
  }
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  orderId!: number

  orderEntity: OrderEntity = new OrderEntity

  sourceData: OrderEntity = new OrderEntity

  get isChanged () {
    return this.orderEntity.isChanged(this.sourceData)
  }

  handleClose () {
    this.$router.go(-1)
  }

  async getOrderEntity () {
    await this.sourceData.find(this.warehouseId, this.orderId)
    this.handleReset()
  }

  handleReset () {
    this.orderEntity.copyFrom(this.sourceData)
  }

  async handleUpdate () {
    await this.orderEntity.update()
  }

  handleDelete () {
    this.$confirm({
      title: '提示',
      content: '删除后将无法恢复',
      handler: async () => {
        await this.orderEntity.delete()
        this.handleClose()
      }
    })
  }

  async handleFile () {
    await this.orderEntity.file()
    await this.getOrderEntity()
  }

  async handleRestore () {
    await this.orderEntity.restore()
    await this.getOrderEntity()
  }
}
</script>
