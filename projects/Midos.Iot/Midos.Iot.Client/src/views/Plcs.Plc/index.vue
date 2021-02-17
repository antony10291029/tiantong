<template>
  <div
    class="is-flex-auto is-flex is-flex-column"
  >
    <div class="tabs" style="margin-bottom: 0; flex-shrink: 0">
      <ul>
        <router-link
          v-for="(tab, key) in tabs" :key="key"
          :to="{ route: tab.route, params: { plcId } }"
          v-slot="{ isActive }"
          custom
        >
          <li v-class:is-active="isActive">
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
      :key="$route.fullPath"
      :baseURL="`${baseURL}/${plcId}`"
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
    baseURL: {
      type: String,
      required: true
    },
    plcId: {
      type: Number,
      required: true
    }
  },

  setup() {
    return {
      tabs: [
        { text: "控制台", route: "PlcsPlcDashboard", icon: "dashboard" },
        { text: "调试", route: "debug", icon: "debug" },
        { text: "数据点", route: "states", icon: "table" },
        { text: "日志", route: "logs", icon: "logs" },
      ]
    };
  }
});
</script>
