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
        @click="$router.push(`/warehouses/${warehouseId}/projects/create`)"
      >
        添加
      </a>
    </div>

    <table
      v-show="!isPending"
      class="table is-fullwidth is-vcentered is-centered is-nowrap is-hoverable"
    >
      <thead>
        <th>工程代码</th>
        <th>工程名称</th>
        <th>工程备注</th>
        <th>开始日期</th>
        <th>完工日期</th>
        <th>截止日期</th>
        <th>启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <tr v-for="project in dataSet" :key="project.id">
          <td>{{project.number}}</td>
          <td>{{project.name}}</td>
          <td>{{project.comment}}</td>
          <td>
            <DateWrapper :value="project.started_at" />
          </td>
          <td>
            <DateWrapper :value="project.finished_at" />
          </td>
          <td>
            <DateWrapper :value="project.due_time" />
          </td>
          <td>
            <YesOrNoCell :value="project.is_enabled"></YesOrNoCell>
          </td>
          <td>
            <router-link :to="`/warehouses/${warehouseId}/projects/${project.id}/update`">
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
  name: 'Projects',
  mixins: [
    DataSet({
      searchApi: '/projects/search',
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
