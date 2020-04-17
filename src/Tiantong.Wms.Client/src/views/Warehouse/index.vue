<template>
  <AsyncLoader
    class="is-flex"
    :handler="getWarehouse"
    v-slot="{ isLoading }"
  >
    <aside
      class="menu has-border-right is-unselectable"
      style="min-width: 220px; max-width: 220px; height: 100%"
    >
      <ul class="menu-list">
        <menu-item
          v-for="(menu, key) in menus" :key="key"
          :text="menu.text"
          :icon="menu.icon"
          :route="menu.route"
          :warehouseId="warehouseId"
        />
      </ul>
    </aside>
    <router-view
      class="is-flex-auto"
      style="overflow: auto; padding: 1rem;"
      v-if="!isLoading"
      :warehouseId="warehouseId"
      :warehouse="warehouse"
      :baseURL="baseURL"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import MenuItem from './MenuItem.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { Warehouse } from '@/Entities'

@Component({
  name: 'Warehouse',
  components: {
    MenuItem,
    AsyncLoader
  },
})
export default class extends Vue {
  @Prop({ required: true })
  public warehouseId!: number

  public menus: Menu[] = menus

  public warehouse?: Warehouse

  get baseURL () {
    return `/warehouses/${this.warehouseId}`
  }

  async getWarehouse () {
    var response = await axios.post('/warehouses/find', {
      warehouse_id: this.warehouseId
    })

    this.warehouse = response.data
  }
}

interface Menu {
  icon: string
  text: string
  route: string
}

const menus: Menu[] = [
  { icon: 'inbound', text: '录料单', route: 'purchase-requisition-orders' },
  { icon: 'picking', text: '领料单', route: 'requisition-orders' },
  // { icon: 'return', text: '返料单', route: 'returns' },
  // { icon: 'inventory', text: '盘点', route: 'inventory' },
  { icon: 'item', text: '库存', route: 'goods' },
  { icon: 'project', text: '工程', route: 'projects' },
  // { icon: 'category', text: '货类', route: 'item-categories' },
  // { icon: 'area', text: '区域', route: 'areas' },
  // { icon: 'location', text: '位置', route: 'locations' },
  { icon: 'department', text: '部门', route: 'departments' },
  { icon: 'users', text: '人员', route: 'users' },
  { icon: 'supplier', text: '供应商', route: 'suppliers' },
  { icon: 'settings', text: '设置', route: 'settings' },
]
</script>
