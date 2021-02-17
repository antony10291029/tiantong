<template>
  <div
    class="is-flex-auto is-flex is-flex-column"
  >
    <div class="tabs" style="margin-bottom: 0; flex-shrink: 0">
      <ul>
        <router-link
          v-for="(tab, key) in tabs" :key="key"
          :to="{ name: tab.route, params: { plcId } }"
          v-slot="{ isActive, navigate }"
          custom
        >
          <li
            v-class:is-active="isActive"
            @click="navigate"
          >
            <a>
              <span class="icon">
                <i :class="`iconfont icon-${tab.icon}`"></i>
              </span>
              <span>{{tab.text}}</span>
            </a>
          </li>
        </router-link>
      </ul>
    </div>

    <router-view
      :key="plcId"
      :plcId="plcId"
      class="is-flex-auto"
      style="overflow: auto"
      v-bind="$attrs"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Plc",

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup() {
    return {
      tabs: [
        { text: "控制台", route: "IotPlcsPlcDashboard", icon: "dashboard" },
        { text: "调试", route: "IotPlcsPlcDebug", icon: "debug" },
        { text: "配置", route: "IotPlcsPlcConfig", icon: "settings" },
        { text: "数据点", route: "IotPlcsPlcStatesIndex", icon: "table" },
        { text: "日志", route: "IotPlcsPlcLogs", icon: "logs" },
      ]
    };
  }
});
</script>
