<template>
  <li>
    <router-link
      :to="routePath"
      exact-active-class="is-active"
    >
      <span class="icon" style="margin-right: 0.25rem">
        <i :class="`iconfont icon-${icon}`"></i>
      </span>
      <span>{{text}}</span>
    </router-link>
  </li>
</template>

<script>
export default {
  name: 'MenuItem',
  props: {
    icon: {
      type: String,
      required: true
    },
    text: {
      type: String,
      required: true
    },
    warehouseId: {
      type: Number,
      required: true
    },
    route: {
      type: String,
      required: true
    }
  },
  computed: {
    routePath () {
      return `/warehouses/${this.warehouseId}/${this.route}`
    }
  }
}
</script>
