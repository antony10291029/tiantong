<template>
  <AsyncLoader
    :handler="getDepartments"
    #default="{ isPending }"
  >
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />
      <div class="is-flex-auto"></div>
      <a
        class="button is-info"
        @click="$router.push(`/warehouses/${warehouseId}/departments/create`)"
      >
        添加
      </a>
    </div>

    <Table>
      <thead>
        <th style="width: 1px">#</th>
        <th>部门</th>
        <th>备注</th>
        <th style="width: 1px">操作</th>
      </thead>
      <tbody>
        <tr v-for="(department, index) in departments" :key="department.id">
          <td>{{index + 1}}</td>
          <td>{{department.name}}</td>
          <td>{{department.comment}}</td>
          <td>
            <router-link :to="baseURL + `/departments/${department.id}/update`">
              <span class="icon">
                <i class="iconfont icon-edit"></i>
              </span>
              <span>编辑</span>
            </router-link>
          </td>
        </tr>
      </tbody>
    </Table>
    <router-view
      :warehouseId="warehouseId"
      @created="getDepartments"
      @updated="getDepartments"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import SearchField from '@/components/SearchField.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import Table from '@/components/Table.vue'
import { Department } from '@/Entities'
import axios from '@/providers/axios'

@Component({
  name: 'DepartmentList',
  components: {
    Table,
    AsyncLoader,
    SearchField,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  baseURL!: string

  search: string = ''

  dataSource: {
    keys: Array<number>
    data: { [key: string]: Department }
  } = { keys: [], data: {} }

  get departments () {
    return this.dataSource.keys.map(key => this.dataSource.data[key])
  }

  handleSearch (value: string) {
    this.search = value
    return this.getDepartments()
  }

  async getDepartments () {
    let response = await axios.post('/departments/all', {
      warehouse_id: this.warehouseId,
      search: this.search
    })

    this.dataSource = response.data
  }
}
</script>
