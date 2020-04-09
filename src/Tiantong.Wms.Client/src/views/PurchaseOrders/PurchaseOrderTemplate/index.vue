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
          :warehouseId="warehouseId"
          :user="entity.applicant"
          @select="handleApplicantSelect"
        />
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">供应商</label>
      <div class="control">
        <SupplierSelector
          :warehouseId="warehouseId"
          :supplier="entity.supplier"
          @select="handleSupplierSelect"
        />
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">日期</label>
      <div class="control">
        <DatePicker v-model="entity.order.created_at"/>
      </div>
    </div>

    <PurchaseOrderStatusSelector
      v-model="entity.order.status"
    />

    <div class="field" style="width: 600px">
      <label class="label">付款信息</label>
      <div class="control">
        <Table>
          <thead>
            <th>#</th>
            <th>金额</th>
            <th style="width: 1px">付款日期</th>
            <th style="width: 1px">截止日期</th>
            <th>付款说明</th>
            <th class="is-unselectable">
              <a @click="handlePaymentsAdd">
                <span class="icon">
                  <i class="iconfont icon-plus"></i>
                </span>
              </a>
            </th>
          </thead>
          <tbody>
            <tr
              v-for="(payment, index) in entity.order.payments.filter(payment => !payment.is_deleted)"
              :key="`${index}_${$uid()}`"
            >
              <th style="width: 1px">
                {{index + 1}}
              </th>
              <EditableCell
                type="number"
                v-model="payment.amount"
                style="width: 80px"
              >
              </EditableCell>
              <DatePicker
                v-model="payment.due_time"
                wrapper="td"
              />
              <DatePicker
                v-model="payment.paid_at"
                wrapper="td"
              />
              <EditableCell
                v-model="payment.comment"
                style="text-align: left"
              ></EditableCell>
              <td style="width: 1px">
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
        </Table>
      </div>
    </div>

    <div class="field">
      <label class="label">采购列表</label>
      <div class="control">
        <Table class="is-nowrap">
          <thead>
            <tr>
              <th></th>
              <th colspan="8">货品信息</th>
              <th colspan="3">工程信息</th>
              <th colspan="10">财务信息</th>
              <th>其他</th>
              <th></th>
            </tr>
            <tr>
              <th style="width: 1px">#</th>
              <th>货名</th>
              <th>规格</th>
              <th>单位</th>
              <th>数量</th>
              <th>单价</th>
              <th>总价</th>
              <th>交货期</th>
              <th>到货日期</th>

              <th>工程编号</th>
              <th>工程</th>
              <th>用量</th>

              <th>货名</th>
              <th>规格</th>
              <th>单位</th>
              <th>数量</th>
              <th>单价</th>
              <th>金额</th>
              <th>税额</th>
              <th>税率</th>
              <th>发票类型</th>
              <th>发票号码</th>
              <th>备注</th>

              <th
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
              orderItem, orderItemIndex, orderItemRowspan,
              itemProject, itemProjectIndex, orderItemFinance,
              isItemGoodShow, isOrderItemShow, isItemProjectsShow,
              project, good, item,
            }"
          >
            <template v-if="isOrderItemShow">
              <td :rowspan="orderItemRowspan">
                {{ orderItemIndex + 1 }}
              </td>

              <GoodSelector
                v-if="!isItemGoodShow"
                :colspan="3"
                :warehouseId="warehouseId"
                :rowspan="orderItemRowspan"
                :good="entity.goods[orderItem.good_id] || null"
                :item="entity.items[orderItem.item_id] || null"
                @select="handleGoodItemSelect(orderItemIndex, $event.good, $event.item)"
              >
              </GoodSelector>
              <template v-else>
                <GoodSelector
                  v-for="(text, index) in [good.name, item.name, item.unit]" :key="`${index}_${$uid}`"
                  style="text-align: center"
                  :warehouseId="warehouseId"
                  :rowspan="orderItemRowspan"
                  :good="entity.goods[orderItem.good_id] || null"
                  :item="entity.items[orderItem.item_id] || null"
                  @select="handleGoodItemSelect(orderItemIndex, $event.good, $event.item)"
                >{{text}}</GoodSelector>
              </template>

              <EditableCell
                type="number"
                :value="orderItem.quantity"
                @change="handleItemQuantityChange(orderItem, $event)"
                :rowspan="orderItemRowspan"
                style="width: 1px"
              ></EditableCell>
              <EditableCell
                type="number"
                style="width: 1px"
                :value="orderItem.price"
                @change="handleItemPriceChange(orderItem, $event)"
                :rowspan="orderItemRowspan"
              ></EditableCell>
              <td
                style="width: 1px"
                :rowspan="orderItemRowspan"
              >
                {{(orderItem.price * orderItem.quantity).toFixed(2)}}
              </td>

              <EditableCell
                v-model="orderItem.delivery_cycle"
                :rowspan="orderItemRowspan"
                style="width: 1px"
              ></EditableCell>
              <DatePicker
                wrapper="td"
                initial="today"
                :rowspan="orderItemRowspan"
                v-model="orderItem.arrived_at"
              />
            </template>

            <template v-if="!isItemProjectsShow">
              <ProjectsSelector
                colspan="3"
                :warehouseId="warehouseId"
                :orderItem="orderItem"
                :itemProjects="orderItem.projects"
                @change="handleProjectChange(orderItem, $event.itemProjects, $event.projects)"
              >无</ProjectsSelector>
            </template>
            <template v-else>
              <ProjectsSelector
                v-for="(text, index) in [project.number, project.name]" :key="index"
                :warehouseId="warehouseId"
                :orderItem="orderItem"
                :itemProjects="orderItem.projects"
                @change="handleProjectChange(orderItem, $event.itemProjects, $event.projects)"
              >
                {{text}}
              </ProjectsSelector>
              <EditableCell
                type="number"
                v-model="itemProject.quantity">
              </EditableCell>
            </template>

            <template v-if="isOrderItemShow">
              <EditableCell
                style="width: 1px"
                v-model="orderItemFinance.name"
                :rowspan="orderItemRowspan"
              />
              <EditableCell
                style="width: 1px"
                v-model="orderItemFinance.specification"
                :rowspan="orderItemRowspan"
              />
              <EditableCell
                style="width: 1px"
                v-model="orderItemFinance.unit"
                :rowspan="orderItemRowspan"
              />
              <EditableCell
                type="number"
                style="width: 1px"
                :rowspan="orderItemRowspan"
                :value="orderItemFinance.quantity"
                @change="handleFinanceQuantityChange(orderItem, $event)"
              />
              <td
                style="width: 1px"
                :rowspan="orderItemRowspan"
              >
                {{orderItemFinance.price.toFixed(2)}}
              </td>
              <td
                style="width: 1px"
                :rowspan="orderItemRowspan"
              >
                {{orderItemFinance.amount.toFixed(2)}}
              </td>
              <td
                style="width: 1px"
                :rowspan="orderItemRowspan"
              >
                {{orderItemFinance.tax_amount.toFixed(2)}}
              </td>
              <EditableCell
                type="number"
                style="width: 1px"
                :rowspan="orderItemRowspan"
                :value="orderItemFinance.tax_rate"
                @change="handleTaxRateChange(orderItem, $event)"
              />
              <InvoiceTypeSelector
                :value="orderItemFinance.invoice_type"
                :rowspan="orderItemRowspan"
                @change="handleInvoiceTypeChange(orderItem, $event)"
              />
              <EditableCell
                v-model="orderItemFinance.invoice_number"
                :rowspan="orderItemRowspan"
                style="width: 1px"
              />
              <EditableCell
                :rowspan="orderItemRowspan"
                v-model="orderItem.comment"
              />
            </template>

            <td
              v-if="isOrderItemShow"
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
          <tbody>
            <tr>
              <td></td>
              <td colspan="5">合计</td>
              <td>{{totalAmount.toFixed(2)}}</td>
              <td colspan="5"></td>
              <td colspan="5">合计</td>
              <td>{{totalFinanceAmount.toFixed(2)}}</td>
              <td>{{totalFinanceTaxAmount.toFixed(2)}}</td>
              <td colspan="2">
                {{(totalFinanceAmount + totalFinanceTaxAmount).toFixed(2)}}
              </td>
              <td colspan="3"></td>
            </tr>
          </tbody>
        </Table>
      </div>
    </div>
    <slot name="footer"></slot>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DatePicker from '@/components/DatePicker/index.vue'
import Table from '@/components/Table.vue'
import Textarea from '@/components/Textarea.vue'
import PurchaseOrderItemRows from './PurchaseOrderItemRows.vue'
import GoodSelector from '@/views/common/GoodSelector/index.vue'
import SupplierSelector from '@/views/common/SupplierSelector.vue'
import ProjectsSelector from '@/views/common/ProjectsSelector.vue'
import DepartmentSelector from '@/views/common/DepartmentSelector.vue'
import WarehouseUserSelector from '@/views/common/WarehouseUserSelector.vue'
import InvoiceTypeSelector from './InvoiceTypeSelector.vue'
import PurchaseOrderStatusSelector from './PurchaseOrderStatusSelector.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { IPurchaseOrderEntity } from './IPurchaseOrderEntity'
import { dragscroll } from 'vue-dragscroll'
import {
  Good,
  Item,
  Project,
  Supplier,
  Department,
  PurchaseOrder,
  PurchaseOrderPayment,
  PurchaseOrderItem,
  PurchaseOrderItemProject,
  WarehouseUser,
  User,
} from '@/Entities'
import PurchaseOrderItemFinance from '../../../Entities/PurchaseOrderItemFinance'

@Component({
  name: 'PurchaseOrderTemplate',
  directives: {
    dragscroll
  },
  components: {
    Table,
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
    PurchaseOrderStatusSelector,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  entity!: IPurchaseOrderEntity

  @Prop({ default: () => () => {} })
  loader!: () => {}

  get totalAmount () {
    let { order } = this.entity

    return order.items.reduce((sum, item) => {
      return sum + item.price * item.quantity
    }, 0)
  }

  get totalFinanceAmount () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.finance.amount
    }, 0)
  }

  get totalFinanceTaxAmount () {
    return this.entity.order.items.reduce((sum, item) => {
      return sum + item.finance.tax_amount
    }, 0)
  }

  handleGoodItemSelect (index: number, good: Good, item: Item) {
    let { order, goods, items } = this.entity
    let { finance } = order.items[index]

    this.$set(goods, good.id, good)
    this.$set(items, item.id, item)
    order.items[index].good_id = good.id
    order.items[index].item_id = item.id
    finance.name = good.name
    finance.unit = item.unit
    finance.specification = item.name
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
    item: PurchaseOrderItem,
    itemProjects: PurchaseOrderItemProject[],
    projects: Project[],
  ) {
    item.projects = itemProjects;

    projects.forEach(project => {
      this.$set(this.entity.projects, project.id, project)
    })
  }

  handlePaymentsAdd () {
    this.entity.order.payments.push(new PurchaseOrderPayment())
  }

  handlePaymentsRemove (index: number, payment: PurchaseOrderPayment) {
    if (this.entity.order.payments.length > 1) {
      this.entity.order.payments.splice(index, 1)
    }
  }

  handleOrderItemAdd () {
    this.entity.order.items.push(new PurchaseOrderItem())
  }

  handleOrderItemRemove (index: number, item: PurchaseOrderItem) {
    if (this.entity.order.items.length > 1) {
      this.entity.order.items.splice(index, 1)
    }
  }

  handleItemPriceChange (item: PurchaseOrderItem, price: number) {
    item.price = item.finance.price = price
    this.calculateFinance(item)
  }

  handleItemQuantityChange (item: PurchaseOrderItem, quantity: number) {
    item.quantity = item.finance.quantity = quantity
    this.calculateFinance(item)
  }

  handleInvoiceTypeChange (item: PurchaseOrderItem, type: string) {
    let { finance } = item

    if (type === '增值税专用发票') {
      console.log(finance.tax_rate)
      if (finance.tax_rate === 0) {
        finance.tax_rate = 13
      }
    } else {
      finance.tax_rate = 0
    }

    finance.invoice_type = type
    this.calculateFinance(item)
  }

  handleTaxRateChange (item: PurchaseOrderItem, rate: number) {
    item.finance.tax_rate = rate
    this.calculateFinance(item)
  }

  handleFinanceQuantityChange (item: PurchaseOrderItem, quantity: number) {
    item.finance.quantity = quantity
    this.calculateFinance(item)
  }

  calculateFinance (item: PurchaseOrderItem) {
    let { finance } = item
    let total = item.quantity * item.price
    if (finance.invoice_type === '增值税专用发票') {
      finance.amount = total / (1 + 0.01 * finance.tax_rate)
      finance.tax_amount = total - finance.amount
      if (finance.quantity !== 0) {
        finance.price = finance.amount / finance.quantity
      } else {
        finance.price = 0
      }
    } else {
      finance.tax_amount = 0
      finance.quantity = item.quantity
      finance.amount = item.price * item.quantity
      finance.price = item.price
    }
  }
}
</script>
