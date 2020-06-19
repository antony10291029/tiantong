<template>
  <div>
    <div
      class="container"
      style="min-width: 400px; max-width: 560px"
    >
      <div style="height: 0.75rem"></div>
      <WarehouseItem
        v-for="id in warehouses.result" :key="id"
        :warehouse="warehouses.entities[id]"
      >
      </WarehouseItem>
      <div style="height: 1rem"></div>
      <div
        class="is-flex is-centered"
        style="margin-bottom: 1.5rem"
      >
        <router-link
          @click="$router.push({name: 'connection create'})"
          class="button is-info"
          style="width: 120px"
          to="/warehouses/create"
        >
          <span class="icon">
            <i class="iconfont icon-plus"></i>
          </span>
          <span>
            添加仓库
          </span>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import axios from '@/providers/axios'
import WarehouseItem from './WarehouseItem.vue'
import { Warehouse } from '@/Entities'
import DataSet from '@/providers/data-set'

@Component({
  name: 'Warehouses',
  components: {
    WarehouseItem
  },
})
export default class extends Vue {
  warehouses: DataSet<Warehouse> = new DataSet<Warehouse>()

  async getWarehouses () {
    const response = await axios.post('/warehouses/search')
    const result = [] as number[], entities = {}
    this.warehouses.clear()
    response.data.forEach((entity: Warehouse) => this.warehouses.add(entity.id, entity))
  }

  created () {
    this.getWarehouses()
  }
}
</script>
