<template>
  <AsyncLoader
    :handler="loader"
    v-dragscroll:nochilddrag
  >
    <slot name="header"></slot>

    <div class="field" style="width: 320px">
      <label class="label">部门</label>
      <div class="control">
        <DepartmentSelector
          :readonly="readonly"
          :warehouseId="warehouseId"
          :department="entity.department"
          @select="handleDepartmentSelect"
        />
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">申请人</label>
      <div class="control">
        <WarehouseUserSelector
          :readonly="readonly"
          :warehouseId="warehouseId"
          :user="entity.applicant"
          @select="handleApplicantSelect"
        />
      </div>
    </div>

    <slot name="supplier">
      <div
        v-if="hasSupplier"
        class="field"
        style="width: 320px"
      >
        <label class="label">供应商</label>
        <div class="control">
          <SupplierSelector
            :readonly="readonly"
            :warehouseId="warehouseId"
            :supplier="entity.supplier"
            @select="handleSupplierSelect"
          />
        </div>
      </div>
    </slot>

    <div class="field" style="width: 320px">
      <label class="label">日期</label>
      <div class="control">
        <DatePicker
          :readonly="readonly"
          v-model="entity.order.created_at"
        />
      </div>
    </div>

    <div
      v-if="hasPayments"
      class="field" style="width: 600px"
    >
      <label class="label">付款信息</label>
      <div class="control">
        <table class="table is-bordered is-nowrap is-vcentered">
          <thead>
            <th>#</th>
            <th>金额</th>
            <th style="width: 1px">付款日期</th>
            <th style="width: 1px">截止日期</th>
            <th style="width: 240px">付款说明</th>
            <th
              v-if="!readonly"
              class="is-unselectable"
            >
              <a @click="handlePaymentsAdd">
                <span class="icon">
                  <i class="iconfont icon-plus"></i>
                </span>
              </a>
            </th>
          </thead>
          <tbody>
            <tr
              v-for="(payment, index) in entity.order.payments"
              :key="`${index}_${$uid()}`"
            >
              <th style="width: 1px">
                {{index + 1}}
              </th>
              <EditableCell
                :readonly="readonly"
                type="number"
                v-model="payment.amount"
                style="width: 80px"
              >
              </EditableCell>
              <DatePicker
                tag="td"
                :readonly="readonly"
                v-model="payment.due_time"
              />
              <DatePicker
                tag="td"
                :readonly="readonly"
                v-model="payment.paid_at"
              />
              <EditableCell
                type="textarea"
                :readonly="readonly"
                style="text-align: left"
                v-model="payment.comment"
              ></EditableCell>
              <td
                v-if="!readonly"
                style="width: 1px"
              >
                <a
                  @click="handlePaymentsRemove(index, payment)"
                  class="has-text-danger"
                >
                  <span class="icon">
                    <i class="iconfont icon-close"></i>
                  </span>
                </a>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="field">
      <label class="label">货品列表</label>
      <div class="control" v-style:width="itemsTableWidth">
        <table class="table is-fullwidth is-bordered is-vcentered is-centered is-nowrap">
          <thead>
            <tr>
              <th></th>
              <th :colspan="hasPurchase ? 8 : 3">货品信息</th>
              <th colspan="3">工程信息</th>
              <th v-if="hasInvoice" colspan="9">财务信息</th>
              <th :colspan="readonly ? 1 : 2">其他</th>
            </tr>
            <tr>
              <th style="width: 1px">#</th>
              <th>货名</th>
              <th>规格</th>
              <th>单位</th>

              <template v-if="hasPurchase">
                <th>交货期</th>
                <th>到货日期</th>
                <th>数量</th>
                <th>单价</th>
                <th>总价</th>
              </template>

              <th>工程编号</th>
              <th>工程</th>
              <th>用量</th>

              <template v-if="hasInvoice">
                <th>货名</th>
                <th>规格</th>
                <th>数量</th>
                <th>单价</th>
                <th>金额</th>
                <th>税额</th>
                <th>税率</th>
                <th>类型</th>
                <th>发票号码</th>
              </template>

              <th style="min-width: 120px">备注</th>

              <th
                v-if="!readonly"
                style="width: 1px"
                class="is-unselectable"
              >
                <a @click="handleOrderItemAdd">
                  <span class="icon">
                    <i class="iconfont icon-plus"></i>
                  </span>
                </a>
              </th>
            </tr>
          </thead>
          <PurchaseOrderItemRows
            :goods="entity.goods"
            :items="entity.items"
            :projects="entity.projects"
            :orderItems="entity.order.items"
            :warehouseId="warehouseId"
            v-slot="{
              itemProject, itemProjectIndex,
              orderItem, orderItemIndex, orderItemRowspan,
              isItemGoodShow, isOrderItemShow, isItemProjectsShow,
              project, good, item, invoice,
            }"
          >
            <template v-if="isOrderItemShow">
              <td :rowspan="orderItemRowspan">
                {{ orderItemIndex + 1 }}
              </td>

              <GoodSelector
                v-if="!isItemGoodShow"
                colspan="3"
                :readonly="readonly"
                :warehouseId="warehouseId"
                :rowspan="orderItemRowspan"
                :good="entity.goods[orderItem.good_id] || null"
                :item="entity.items[orderItem.item_id] || null"
                @select="handleGoodItemSelect(orderItemIndex, $event.good, $event.item)"
              >
                <span class="has-text-grey is-italic">无</span>
              </GoodSelector>
              <template v-else>
                <GoodSelector
                  v-for="(text, index) in [good.name, item.name, item.unit]"
                  :key="`${index}_${$uid}`"
                  :readonly="readonly"
                  :warehouseId="warehouseId"
                  :rowspan="orderItemRowspan"
                  :good="entity.goods[orderItem.good_id] || null"
                  :item="entity.items[orderItem.item_id] || null"
                  @select="handleGoodItemSelect(orderItemIndex, $event.good, $event.item)"
                >{{text}}</GoodSelector>
              </template>

              <template v-if="hasPurchase">
                <EditableCell
                  :readonly="readonly"
                  v-model="orderItem.delivery_cycle"
                  :rowspan="orderItemRowspan"
                  style="width: 1px"
                />
                <DatePicker
                  tag="td"
                  initial="today"
                  :readonly="readonly"
                  :rowspan="orderItemRowspan"
                  v-model="orderItem.arrived_at"
                />
                <EditableCell
                  :readonly="readonly"
                  type="number"
                  :value="orderItem.quantity"
                  @change="handleItemQuantityChange(orderItem, $event)"
                  :rowspan="orderItemRowspan"
                  style="width: 1px"
                />
                <EditableCell
                  :readonly="readonly"
                  type="number"
                  style="width: 1px"
                  :value="orderItem.price"
                  @change="handleItemPriceChange(orderItem, $event)"
                  :rowspan="orderItemRowspan"
                />
                <th
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                >
                  {{(orderItem.price * orderItem.quantity).toFixed(2)}}
                </th>
              </template>
            </template>

            <template v-if="!isItemProjectsShow">
              <ProjectsSelector
                colspan="3"
                :readonly="readonly"
                :orderItem="orderItem"
                :warehouseId="warehouseId"
                :itemProjects="orderItem.projects"
                @change="handleProjectChange(orderItem, $event.itemProjects, $event.projects)"
              >
                <span class="has-text-grey is-italic">无</span>
              </ProjectsSelector>
            </template>
            <template v-else>
              <ProjectsSelector
                v-for="(text, index) in [project.number, project.name]"
                :key="index"
                :readonly="readonly"
                :warehouseId="warehouseId"
                :orderItem="orderItem"
                :itemProjects="orderItem.projects"
                @change="handleProjectChange(orderItem, $event.itemProjects, $event.projects)"
              >
                {{text}}
              </ProjectsSelector>
              <EditableCell
                :readonly="readonly"
                type="number"
                v-model="itemProject.quantity">
              </EditableCell>
            </template>

            <template v-if="isOrderItemShow">
              <template v-if="hasInvoice">
                <EditableCell
                  :readonly="readonly"
                  style="width: 1px"
                  v-model="invoice.name"
                  :rowspan="orderItemRowspan"
                />
                <EditableCell
                  :readonly="readonly"
                  style="width: 1px"
                  v-model="invoice.specification"
                  :rowspan="orderItemRowspan"
                />
                <EditableCell
                  :readonly="readonly"
                  type="number"
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                  :value="invoice.quantity"
                  @change="handleInvoiceQuantityChange(orderItem, $event)"
                />
                <th
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                >
                  {{invoice.price.toFixed(2)}}
                </th>
                <th
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                >
                  {{invoice.amount.toFixed(2)}}
                </th>
                <th
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                >
                  {{invoice.tax_amount.toFixed(2)}}
                </th>
                <EditableCell
                  :readonly="readonly"
                  type="number"
                  style="width: 1px"
                  :rowspan="orderItemRowspan"
                  :value="invoice.tax_rate"
                  @change="handleTaxRateChange(orderItem, $event)"
                />
                <InvoiceTypeSelector
                  :value="invoice.type"
                  :rowspan="orderItemRowspan"
                  @change="handleInvoiceTypeChange(orderItem, $event)"
                />
                <EditableCell
                  :readonly="readonly"
                  v-model="invoice.number"
                  :rowspan="orderItemRowspan"
                  style="width: 1px"
                />
              </template>
              <EditableCell
                type="textarea"
                style="text-align: left"
                :readonly="readonly"
                :rowspan="orderItemRowspan"
                v-model="orderItem.comment"
              />
            </template>

            <td
              v-if="isOrderItemShow && !readonly"
              :rowspan="orderItemRowspan"
            >
              <a
                class="has-text-danger"
                @click="handleOrderItemRemove(orderItemIndex, orderItem)"
              >
                <span class="icon">
                  <i class="iconfont icon-close"></i>
                </span>
              </a>
            </td>
          </PurchaseOrderItemRows>
          <thead>
            <tr>
              <th :colspan="hasPurchase ? 5 : 4">合计</th>
              <template v-if="hasPurchase">
                <th>总数</th>
                <th>{{totalQuantity}}</th>
                <th>总价</th>
                <th>{{totalAmount.toFixed(2)}}</th>
              </template>
              <th colspan="2">工程用量</th>
              <th>{{totalProjectsQuantity}}</th>

              <template v-if="hasInvoice">
                <th colspan="4">金额 / 税额 / 总额</th>
                <th>{{totalInvoiceAmount.toFixed(2)}}</th>
                <th>{{totalInvoiceTaxAmount.toFixed(2)}}</th>
                <th colspan="3">
                  {{(totalInvoiceAmount + totalInvoiceTaxAmount).toFixed(2)}}
                </th>
              </template>
              <th colspan="2"></th>
            </tr>
          </thead>
        </table>
      </div>
    </div>
    <slot name="footer"></slot>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import DatePicker from '@/components/DatePicker/index.vue'
import Textarea from '@/components/Textarea.vue'
import PurchaseOrderItemRows from './PurchaseOrderItemRows.vue'
import GoodSelector from '@/views/common/GoodSelector/index.vue'
import SupplierSelector from '@/views/common/SupplierSelector/index.vue'
import ProjectsSelector from '@/views/common/ProjectsSelector.vue'
import DepartmentSelector from '@/views/common/DepartmentSelector.vue'
import WarehouseUserSelector from '@/views/common/WarehouseUserSelector.vue'
import InvoiceTypeSelector from './InvoiceTypeSelector.vue'
import StatusSelector from './StatusSelector.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { dragscroll } from 'vue-dragscroll'
import OrderEntity from './OrderEntity'
import {
  User,
  Good,
  Item,
  Invoice,
  Project,
  Supplier,
  Department,
  Order,
  OrderItem,
  OrderPayment,
  OrderItemProject,
  WarehouseUser,
} from '@/Entities'

@Component({
  name: 'PurchaseOrderTemplate',
  directives: {
    dragscroll
  },
  comments: true,
  components: {
    Textarea,
    DatePicker,
    AsyncLoader,
    PurchaseOrderItemRows,
    GoodSelector,
    SupplierSelector,
    ProjectsSelector,
    DepartmentSelector,
    WarehouseUserSelector,
    InvoiceTypeSelector,
    StatusSelector,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  entity!: OrderEntity

  @Prop({ default: true })
  hasSupplier!: boolean

  @Prop({ default: true })
  hasPayments!: boolean

  @Prop({ default: true })
  hasInvoice!: boolean

  @Prop({ default: true })
  hasPurchase!: boolean

  @Prop({ default: '100%' })
  itemsTableWidth!: string

  @Prop({ default: () => () => {} })
  loader!: () => {}

  get readonly () {
    return this.entity.order.status !== 'created'
  }

  get totalAmount () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.price * item.quantity
    }, 0)
  }

  get totalQuantity () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.quantity
    }, 0)
  }

  get totalProjectsQuantity () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.projects.reduce((s, project) => s + project.quantity, 0)
    }, 0)
  }

  get totalInvoiceAmount () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.invoice.amount
    }, 0)
  }

  get totalInvoiceTaxAmount () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.invoice.tax_amount
    }, 0)
  }

  handleGoodItemSelect (index: number, good: Good, item: Item) {
    let { order, goods, items } = this.entity
    let { invoice } = order.items[index]

    this.$set(goods, good.id, good)
    this.$set(items, item.id, item)
    order.items[index].good_id = good.id
    order.items[index].item_id = item.id
    invoice.name = good.name
    invoice.unit = item.unit
    invoice.specification = item.name
  }

  handleApplicantSelect (user: User) {
    let { order } = this.entity

    order.applicant_id = user.id
    this.entity.applicant = user
  }

  handleSupplierSelect (supplier: Supplier) {
    this.entity.order.supplier_id = supplier.id
    this.entity.supplier = supplier
  }

  handleDepartmentSelect (department: Department) {
    this.entity.order.department_id = department.id
    this.entity.department = department
  }

  handleProjectChange (
    item: OrderItem,
    itemProjects: OrderItemProject[],
    projects: Project[],
  ) {
    item.projects = itemProjects;

    projects.forEach(project => {
      this.$set(this.entity.projects, project.id, project)
    })
  }

  handlePaymentsAdd () {
    this.entity.order.payments.push(new OrderPayment())
  }

  handlePaymentsRemove (index: number, payment: OrderPayment) {
    if (this.entity.order.payments.length > 1) {
      this.entity.order.payments.splice(index, 1)
    }
  }

  handleOrderItemAdd () {
    this.entity.order.items.push(new OrderItem())
  }

  handleOrderItemRemove (index: number, item: OrderItem) {
    if (this.entity.order.items.length > 1) {
      this.entity.order.items.splice(index, 1)
    }
  }

  handleItemPriceChange (item: OrderItem, price: number) {
    item.price = item.invoice!.price = price
    this.calculateInvoice(item)
  }

  handleItemQuantityChange (item: OrderItem, quantity: number) {
    item.quantity = item.invoice!.quantity = quantity
    this.calculateInvoice(item)
  }

  handleInvoiceTypeChange (item: OrderItem, type: string) {
    let { invoice } = item

    if (type === '增值税专用发票') {
      if (invoice!.tax_rate === 0) {
        invoice!.tax_rate = 13
      }
    } else {
      invoice.tax_rate = 0
    }

    invoice.type = type
    this.calculateInvoice(item)
  }

  handleTaxRateChange (item: OrderItem, rate: number) {
    item.invoice.tax_rate = rate
    this.calculateInvoice(item)
  }

  handleInvoiceQuantityChange (item: OrderItem, quantity: number) {
    item.invoice.quantity = quantity
    this.calculateInvoice(item)
  }

  calculateInvoice (item: OrderItem) {
    let { invoice } = item
    let total = item.quantity * item.price

    if (invoice.type === '增值税专用发票') {
      invoice.amount = total / (1 + 0.01 * invoice.tax_rate)
      invoice.tax_amount = total - invoice.amount
    } else {
      invoice.tax_amount = 0
      invoice.price = item.price
      invoice.amount = item.price * item.quantity
    }

    if (invoice.quantity !== 0) {
      invoice.price = invoice.amount / invoice.quantity
    } else {
      invoice.price = 0
    }
  }
}
</script>
