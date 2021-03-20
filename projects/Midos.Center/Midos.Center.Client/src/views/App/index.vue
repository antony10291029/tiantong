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
              <i :class="`icon-midos-center icon-midos-center-${menu.icon}`"></i>
            </span>
            <span>{{menu.text}}</span>
          </router-link>
        </li>
      </ul>
    </aside>

    <router-view class="is-flex-auto" style="overflow: auto" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",

  setup() {
    return {
      menus: [
        { text: "任务列表", route: "MidosCenterTasTasks", icon: "tasks" },
        { text: "任务配置", route: "MidosCenterTasTypes", icon: "task-config" },
        { text: "应用管理", route: "MidosCenterApps", icon: "apps" },
        { text: "配置中心", route: "MidosCenterConfigs", icon: "configs" },
      ]
    };
  }
});
</script>
