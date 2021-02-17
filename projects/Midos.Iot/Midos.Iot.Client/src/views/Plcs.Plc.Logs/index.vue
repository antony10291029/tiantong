<template>
  <div
    class="box"
    style="margin: 1.25rem; padding-top: 1.25rem"
  >
    <div
      class="tabs is-toggle is-small"
      style="margin-bottom: 0.75rem"
    >
      <ul>
        <router-link
          custom
          v-for="(tab, key) in tabs" :key="key"
          :to="{ name: tab.route, params: { plcId } }"
          v-slot="{ navigate, isActive }"
        >
          <li
            v-class:is-active="isActive"
            @click="navigate"
          >
            <a>{{tab.text}}</a>
          </li>
        </router-link>
      </ul>
    </div>

    <router-view :plcId="plcId"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "PlcLogs",

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup() {
    return {
      tabs: [
        { text: "数据读写", route: "IotPlcsPlcLogsStateLogs" },
        { text: "读写异常", route: "IotPlcsPlcLogsStateErrors" },
        { text: "HTTP推送", route: "IotPlcsPlcLogsHttpPusher" },
        { text: "推送异常", route: "IotPlcPlcsPlcLogsHttpPusherErrors" },
      ]
    };
  }
});
</script>
