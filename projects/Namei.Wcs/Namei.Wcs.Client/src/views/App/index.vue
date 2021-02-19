<template>
  <div class="is-flex-auto is-flex">
    <aside
      class="menu is-unselectable"
      style="min-width: 260px; max-width: 260px"
    >
      <ul class="menu-list">
        <li>
          <router-link
            v-for="(menu, key) in menus" :key="key"
            :to="{ name: menu.route }"
          >
            <span class="icon">
              <i :class="`icon-namei-wcs icon-namei-wcs-${menu.icon}`"></i>
            </span>
            <span>{{menu.text}}</span>
          </router-link>
        </li>
      </ul>
    </aside>

    <router-view class="is-flex-auto" style="overflow-y: auto"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",

  setup() {
    return {
      menus: [
        { text: "设备监控", route: "NameiWcsDevices", icon: "dashboard" },
        { text: "提升机日志", route: "NameiWcsLifters", icon: "lifter" },
        { text: "自动门日志", route: "NameiWcsDoors", icon: "lifter" },
        { text: "系统设置", route: "NameiWcsSystem", icon: "settings" },
      ]
    };
  }
});
</script>
