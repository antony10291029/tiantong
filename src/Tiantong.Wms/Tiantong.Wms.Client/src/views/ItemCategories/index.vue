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
        @click="$router.push(`/warehouses/${warehouseId}/item-categories/create`)"
      >
        添加
      </a>
    </div>

    <table
      v-show="!isPending"
      class="table is-fullwidth is-vcentered is-centered is-nowrap is-hoverable"
    >
      <thead>
        <th>货类名称</th>
        <th>货类编码</th>
        <th>备注</th>
        <th>启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <tr v-for="category in dataSet" :key="category.id">
          <td>{{category.name}}</td>
          <td>{{category.number}}</td>
          <td>{{category.comment}}</td>
          <td>
            <YesOrNoCell :value="category.is_enabled"></YesOrNoCell>
          </td>
          <td>
            <router-link :to="`/warehouses/${warehouseId}/item-categories/${category.id}/update`">
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

@Component({
  name: 'ItemCategories',
  mixins: [
    DataSet({
      searchApi: '/item-categories/search',
      searchParams: (vm: any) => ({ warehouse_id: vm.warehouseId })
    })
  ],
  components: {
    Pagination,
    AsyncLoader,
    SearchField,
    YesOrNoCell,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  value: string = 'false'
}
</script>
