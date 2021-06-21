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

    <router-view
      class="is-flex-auto"
      style="overflow-y: auto; overflow-x: auto"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",

  setup() {
    return {
      menus: [
        { text: "设备监控", route: "NameiWcsDevicesDashboard", icon: "monitor" },
        { text: "提升机", route: "NameiWcsLifters", icon: "lifter" },
        { text: "自动门", route: "NameiWcsDoors", icon: "door" },
        { text: "AGC 任务", route: "NameiWcsRcsAgcTasks", icon: "door" },
        { text: "发料任务", route: "NameiWcsWmsPickTicketTasks", icon: "deliver-items" },
        { text: "箱码绑定", route: "NameiWmsMoveDocBoxes", icon: "deliver-items" },
        { text: "地图管理", route: "NameiWcsRcs", icon: "map-unbind" },
        { text: "系统日志", route: "NameiWcsLogs", icon: "log" },
        { text: "系统功能", route: "NameiWcsSystem", icon: "settings" },
      ]
    };
  }
});
</script>
