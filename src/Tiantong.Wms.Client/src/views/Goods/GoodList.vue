<template>
  <AsyncLoader :handler="getEntities">
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />
      <div class="is-flex-auto"></div>
      <router-link
        class="button is-info"
        :to="baseURL + 'create'"
      >
        添加
      </router-link>
    </div>

    <Table v-show="!isPending">
      <thead>
        <th style="width: 1px">#</th>
        <th>货码</th>
        <th>货名</th>
        <th>备注</th>
        <th style="width: 1px">规格码</th>
        <th style="width: 1px">规格</th>
        <th style="width: 1px">单位</th>
        <th style="width: 1px">库存</th>
        <th style="width: 1px">启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <template v-for="(good, goodKey) in entityList">
          <template v-for="(item, itemKey) in good.items">
            <tr :key="good.id + '' + item.id">
              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                {{goodKey + 1}}
              </td>
              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                {{good.number}}
              </td>
              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                {{good.name}}
              </td>
              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                {{good.comment}}
              </td>
              <td>
                {{item.number}}
              </td>

              <td>
                {{item.name}}
              </td>
              <td>
                {{item.unit}}
              </td>
              <td>
                {{item.stocks.reduce((sum, stock) => sum + stock.quantity, 0)}}
              </td>
              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                <YesOrNoCell :value="good.is_enabled"></YesOrNoCell>
              </td>

              <td
                v-if="itemKey === 0"
                :rowspan="good.items.length"
              >
                <router-link :to="baseURL + `${good.id}/manage`">
                  <span class="icon">
                    <i class="iconfont icon-edit"></i>
                  </span>
                  <span>管理</span>
                </router-link>
              </td>
            </tr>
          </template>
        </template>
      </tbody>
    </Table>
    <div style="height: 1rem"></div>
    <Pagination v-show="!isPending" v-bind="entities.meta" @change="handlePageChange"></Pagination>

    <router-view
      :baseURL="baseURL"
      :warehouseId="warehouseId"
      @refresh="handleRefresh"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DataSet from '@/share/DataSet'
import Table from '@/components/Table.vue'
import SearchField from '@/components/SearchField.vue'
import YesOrNoCell from '@/components/YesOrNoCell.vue'
import Pagination from '@/components/Pagination.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DateWrapper from '@/components/wrappers/DateWrapper.vue'
import groupBy from 'lodash/groupBy'

@Component({
  name: 'GoodStocks',
  components: {
    Table,
    Pagination,
    SearchField,
    YesOrNoCell,
    AsyncLoader,
    DateWrapper
  }
})
export default class extends DataSet {
  api = '/goods/search'

  @Prop({ required: true })
  warehouseId!: number

  get baseURL () {
    return `/warehouses/${this.warehouseId}/goods/`
  }

  get params () {
    return {
      warehouse_id: this.warehouseId
    }
  }

}
</script>
