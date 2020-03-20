<template>
  <AsyncLoader
    class="is-flex-auto"
    style="height: 100%; overflow: auto; padding: 0.75rem"
    :handler="getDataSet"
  >
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="search"
      />
      <div class="is-flex-auto"></div>
      <a
        class="button is-info"
        @click="$router.push(`/warehouses/${warehouseId}/items/create`)"
      >
        添加
      </a>
    </div>

    <table
      v-show="!isPending"
      class="table is-bordered is-fullwidth is-vcentered is-centered is-nowrap is-hoverable"
    >
      <thead>
        <th>货码</th>
        <th>货名</th>
        <th style="width: 1px">数量</th>
        <th style="width: 1px">规格</th>
        <th>备注</th>
        <th>启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <tr v-for="item in dataSet" :key="item.id">
          <td>{{item.number}}</td>
          <td>{{item.name}}</td>
          <td>{{item.stocks.reduce((s, stock) => s += stock.quantity, 0)}}</td>
          <td>{{item.specification}}</td>
          <td>{{item.comment}}</td>
          <td>
            <YesOrNoCell :value="item.is_enabled"></YesOrNoCell>
          </td>
          <td>
            <router-link :to="`/warehouses/${warehouseId}/items/${item.id}/update`">
              <span class="icon is-info">
                <i class="iconfont icon-edit"></i>
              </span>
              <span>编辑</span>
            </router-link>
          </td>
        </tr>
      </tbody>
    </table>
    <div style="height: 1rem"></div>
    <Pagination v-show="!isPending" v-bind="meta" @change="changePage"></Pagination>

    <router-view
      :warehouseId="warehouseId"
      @refresh="refresh"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DataSet from '@/mixins/data-set'
import SearchField from '@/components/SearchField.vue'
import YesOrNoCell from '@/components/YesOrNoCell.vue'
import Pagination from '@/components/Pagination.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DateWrapper from '@/components/wrappers/DateWrapper.vue'

@Component({
  name: 'Stocks',
  mixins: [
    DataSet({
      searchApi: '/items/search',
      searchParams: (vm: any) => ({ warehouse_id: vm.warehouseId })
    })
  ],
  components: {
    Pagination,
    SearchField,
    YesOrNoCell,
    AsyncLoader,
    DateWrapper
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number
}
</script>
