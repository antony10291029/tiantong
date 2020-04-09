<template>
  <AsyncLoader :handler="getUsers" #default="{ isPending }">
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />
      <div class="is-flex-auto"></div>
      <a
        class="button is-info"
        @click="$router.push(`/warehouses/${warehouseId}/users/create`)"
      >
        添加
      </a>
    </div>

    <Table>
      <thead>
        <th style="width: 1px">#</th>
        <th>邮箱</th>
        <th>姓名</th>
        <th style="width: 1px">操作</th>
      </thead>
      <tbody>
        <tr v-for="(entity, index) in warehouseUsers" :key="entity.id">
          <td>{{index + 1}}</td>
          <td>{{entity.user.email}}</td>
          <td>{{entity.user.name}}</td>
          <td>
            <router-link :to="baseURL + `/users/${entity.id}/update`">
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
      @updated="getUsers()"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import SearchField from '@/components/SearchField.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import Table from '@/components/Table.vue'
import { WarehouseUser } from '@/Entities'
import axios from '@/providers/axios'

@Component({
  name: 'WarehouseUserList',
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

  entities: {
    keys: number[],
    data: { [key: string]: WarehouseUser }
  } = { keys: [], data: {} }

  search: string = ''

  get warehouseUsers () {
    let { keys, data } = this.entities

    return keys.map(key => data[key])
  }

  handleSearch (value: string) {
    this.search = value
    return this.getUsers()
  }

  async getUsers () {
    let response = await axios.post('/warehouses/users/all', {
      warehouse_id: this.warehouseId,
      search: this.search
    })

    this.entities = response.data
  }
}
</script>
