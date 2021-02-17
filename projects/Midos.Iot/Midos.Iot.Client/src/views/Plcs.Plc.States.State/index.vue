<template>
  <div class="box" style="padding: 0">
    <div class="tabs">
      <ul>
        <router-link
          custom
          v-for="(tab, key) in tabs" :key="key"
          :to="{ name: tab.route, params: { plcId, stateId } }"
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

    <div style="padding: 1.25rem">
      <router-view
        :stateId="stateId"
        v-bind="$attrs"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "PlcState",

  props: {
    plcId: {
      type: Number,
      required: true,
    },

    stateId: {
      type: Number,
      required: true
    }
  },

  setup() {
    return {
      tabs: [
        { text: "基本配置", route: "IotPlcsPlcStatesStateDetail" },
        { text: "HTTP 推送", route: "IotPlcsPlcStatesStateHttpPushers" }
      ]
    };
  }
});
</script>
