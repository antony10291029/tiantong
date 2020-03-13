<template>
  <div class="is-flex">
    <aside
      class="menu has-border-right is-unselectable"
      style="width: 220px; height: 100%"
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
    <router-view :warehouseId="warehouseId"></router-view>
  </div>
</template>

<script>
import MenuItem from './MenuItem'

export default {
  name: 'Warehouse',
  components: {
    MenuItem
  },
  props: {
    warehouseId: {
      type: Number,
      required: true
    }
  },
  data: () => ({
    menus: [
      {
        icon: 'inbound',
        text: '入库单',
        route: 'inbounds'
      },
      {
        icon: 'picking',
        text: '领料单',
        route: 'pickings'
      },
      {
        icon: 'return',
        text: '返料单',
        route: 'returns'
      },
      {
        icon: 'inventory',
        text: '盘点',
        route: 'inventory'
      },
      {
        icon: 'item',
        text: '库存',
        route: 'stocks'
      },
      {
        icon: 'project',
        text: '工程',
        route: 'projects'
      },
      {
        icon: 'category',
        text: '货类',
        route: 'item-categories'
      },
      {
        icon: 'area',
        text: '区域',
        route: 'areas'
      },
      {
        icon: 'location',
        text: '位置',
        route: 'locations'
      },
      {
        icon: 'supplier',
        text: '供应商',
        route: 'suppliers'
      },
      {
        icon: 'settings',
        text: '设置',
        route: 'settings'
      },
    ]
  })
}
</script>
