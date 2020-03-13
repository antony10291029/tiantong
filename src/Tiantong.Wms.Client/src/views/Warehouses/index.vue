<template>
  <div>
    <div class="container" style="min-width: 400px; max-width: 560px">
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

<script>
import axios from '@/providers/axios'
import WarehouseItem from './WarehouseItem'

export default {
  name: 'Warehouses',
  components: {
    WarehouseItem
  },
  data: () => ({
    warehouses: {
      result: [],
      entities: {},
    }
  }),
  methods: {
    async getWarehouses () {
      const response = await axios.post('/warehouses/search')
      const result = [], entities = {}
      response.data.forEach(entity => {
        result.push(entity.id)
        entities[entity.id] = entity
      })

      this.warehouses = { result, entities }
    }
  },
  created () {
    this.getWarehouses()
  }
}
</script>
