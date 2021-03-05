<template>
  <div class="is-flex is-flex-column">
    <div
      class="tabs"
      style="margin-bottom: 0; font-size: 1.125rem; flex-shrink: 0"
    >
      <ul>
        <router-link
          custom
          v-for="(tab, key) in tabs" :key="key"
          :to="{ name: tab.route }"
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

    <router-view />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Devices",

  setup() {
    return {
      tabs: [
        { text: "实时监控", route: "NameiWcsDevicesDashboard" },
        { text: "提升机任务", route: "NameiWcsDevicesLifterTasks" },
      ]
    };
  }
});
</script>
