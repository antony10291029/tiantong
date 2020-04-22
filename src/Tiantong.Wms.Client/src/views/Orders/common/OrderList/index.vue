<template>
  <AsyncLoader :handler="getEntities">
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      >
        <p class="control">
          <span class="select">
            <select v-model="status">
              <option :value="null">默认</option>
              <option value="created">未完成</option>
              <option value="finished">已入库</option>
              <option value="filed">已归档</option>
            </select>
          </span>
        </p>
      </SearchField>
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
          <th colspan="7">录料单信息</th>
          <th colspan="3">工程信息</th>
          <th v-if="isAdmin"></th>
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
          <th v-if="isAdmin">操作</th>
        </tr>
      </thead>
      <OrderRow
        :orders="entityList"
        :relationships="relationships"
        v-slot="{
          order, orderItem, itemProject, invoice,
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
            <OrderStatusTag
              :value="order.status"
              :createdStatus="createdStatus"
              :finishedStatus="finishedStatus"
            />
            <div style="height: 0.5rem"></div>
            <DateWrapper :value="order.created_at"/>
          </td>
          <td :rowspan="orderRowspan">
            <template v-if="applicant">
              <div v-if="applicant">{{applicant.name}}</div>
              <div style="height: 0.5rem"></div>
            </template>
            <template v-if="department">
              <div v-if="department">{{department.name}}</div>
              <div style="height: 0.5rem"></div>
            </template>
            <template v-if="supplier">
              <div>{{supplier.name}}</div>
            </template>
          </td>
        </template>
        <template v-if="isItemShow">
          <td
            v-if="good"
            :rowspan="itemRowspan"
          >{{good.name}}</td>
          <td
            v-else
            class="has-text-grey is-italic"
          >无</td>
          <td
            v-if="item"
            :rowspan="itemRowspan"
          >{{item.name}}</td>
          <td
            v-else
            class="has-text-grey is-italic"
          >无</td>
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

        <template v-if="isOrderShow && isAdmin">
          <td :rowspan="orderRowspan">
            <router-link :to="`${baseURL}/${order.id}/detail`">
              <span class="icon">
                <i class="iconfont icon-detail"></i>
              </span>
              <span>详情</span>
            </router-link>
          </td>
        </template>
      </OrderRow>
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
import OrderRow from './OrderRow.vue'
import OrderStatusTag from './OrderStatusTag.vue'
import { Department } from '../../../../Entities'

@Component({
  name: 'PurchaseOrderList',
  components: {
    Table,
    Pagination,
    AsyncLoader,
    SearchField,
    DateWrapper,
    OrderStatusTag,
    OrderRow
  }
})
export default class extends DataSet {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  department!: Department

  @Prop({ required: true })
  path!: string

  @Prop({ default: () => createdStatus })
  createdStatus!: object

  @Prop({ default: () => finishedStatus })
  finishedStatus!: object

  pageSize = 10

  api = this.path

  status: string | null = null

  get params () {
    return {
      warehouse_id: this.warehouseId,
      status: this.status
    }
  }

  get isAdmin () {
    return Department.isAdmin(this.department)
  }

}

const createdStatus = {
  text: '未完成',
  color: 'warning'
}

const finishedStatus = {
  text: '已完成',
  color: 'success'
}
</script>
