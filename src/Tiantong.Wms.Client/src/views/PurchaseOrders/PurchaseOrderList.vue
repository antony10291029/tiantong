<template>
  <AsyncLoader :handler="getEntities">
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />

      <div style="width: 0.5rem"></div>

      <router-link
        class="button is-info"
        :to="baseURL + '/create'"
      >
        添加
      </router-link>
    </div>

    <Table class="is-hoverable" v-if="!isPending">
      <thead>
        <tr>
          <th></th>
          <th colspan="7">订单信息</th>
          <th colspan="3">工程信息</th>
          <th></th>
        </tr>
        <tr>
          <th>#</th>
          <th>状态</th>
          <th>基本信息</th>
          <th>货名</th>
          <th>规格</th>
          <th>数量</th>
          <th>交货期</th>
          <th>到货日期</th>
          <th>工程编号</th>
          <th>工程名</th>
          <th>用量</th>
          <th>操作</th>
        </tr>
      </thead>
      <PurchaseOrderRow
        :orders="entityList"
        :relationships="relationships"
        v-slot="{
          order, orderItem, itemProject, itemFinance,
          good, item, project,
          supplier, applicant, department,
          isOrderShow, orderRowspan, orderIndex,
          isItemShow, itemRowspan
        }"
      >
        <template v-if="isOrderShow">
          <td :rowspan="orderRowspan">
            {{orderIndex + 1}}
          </td>
          <td :rowspan="orderRowspan">
            <span
              class="tag"
              :class="order.status === '已完成' ? 'is-success' : 'is-warning'"
            >
              {{order.status}}
            </span>
            <div style="height: 0.5rem"></div>
            <DateWrapper :value="order.created_at"/>
          </td>
          <td :rowspan="orderRowspan">
            <div>{{applicant.name}}</div>
            <div style="height: 0.5rem"></div>
            <div>{{department.name}}</div>
            <div style="height: 0.5rem"></div>
            <div>{{supplier.name}}</div>
          </td>
        </template>
        <template v-if="isItemShow">
          <td :rowspan="itemRowspan">
            {{good.name}}
          </td>
          <td :rowspan="itemRowspan">
            {{item.name}}
          </td>
          <td :rowspan="itemRowspan">
            {{orderItem.quantity}}
          </td>
          <td
            :rowspan="itemRowspan"
            v-text="orderItem.delivery_cycle"
          />
          <DateWrapper
            tag="td"
            :rowspan="itemRowspan"
            :value="orderItem.arrived_at"
          />
        </template>

        <template v-if="itemProject !== null">
          <td>{{project.number}}</td>
          <td>{{project.name}}</td>
          <td>{{itemProject.quantity}}</td>
        </template>
        <template v-else>
          <td class="has-text-grey-light is-italic" colspan="3">
            无
          </td>
        </template>

        <template v-if="isOrderShow">
          <td :rowspan="orderRowspan">
            <router-link :to="`${baseURL}/${order.id}/update`">
              <span class="icon">
                <i class="iconfont icon-edit"></i>
              </span>
              <span>编辑</span>
            </router-link>
          </td>
        </template>
      </PurchaseOrderRow>
    </Table>

    <div style="height: 1rem"></div>

    <Pagination
      v-show="!isPending"
      v-bind="entities.meta"
      @change="handlePageChange"
    ></Pagination>
  </AsyncLoader>
</template>

<script lang="tsx">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DataSet from '@/share/DataSet'
import AsyncLoader from '@/components/AsyncLoader.vue'
import Table from '@/components/Table.vue'
import Pagination from '@/components/Pagination.vue'
import SearchField from '@/components/SearchField.vue'
import DateWrapper from '@/components/wrappers/DateWrapper.vue'
import PurchaseOrderRow from './PurchaseOrderRow.vue'

@Component({
  name: 'PurchaseOrderList',
  components: {
    Table,
    Pagination,
    AsyncLoader,
    SearchField,
    DateWrapper,
    PurchaseOrderRow
  }
})
export default class extends DataSet {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  warehouseId!: number

  pageSize = 10

  api = '/purchase-orders/search'

  get params () {
    return {
      warehouse_id: this.warehouseId
    }
  }

}
</script>
