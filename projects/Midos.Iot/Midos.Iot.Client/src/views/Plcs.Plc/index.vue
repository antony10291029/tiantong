<template>
  <div class="is-flex-auto is-flex is-flex-column">
    <div class="tabs" style="margin-bottom: 0; flex-shrink: 0">
      <ul>
        <router-link
          v-for="(tab, key) in tabs" :key="key"
          :to="`${baseURL}/${plc.id}/${tab.route}`"
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
      class="is-flex-auto"
      style="overflow: auto"
      :baseURL="`${baseURL}/${plc.id}`"
      :plc="plc"
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
    plc: {
      type: Object,
      required: true
    }
  },

  setup() {
    return {
      tabs: [
        { text: "控制台", route: "dashboard", icon: "dashboard" },
        { text: "调试", route: "debug", icon: "debug" },
        { text: "数据点", route: "states", icon: "table" },
        { text: "日志", route: "logs", icon: "logs" },
      ]
    };
  }
});
</script>
