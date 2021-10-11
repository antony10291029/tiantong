<template>
  <Layout>
    <template #title>
      <span class="text-2xl">LinkFlow</span>
    </template>

    <template #menu="{ toggleMenu }">
      <SidebarMenu
        route="/"
        text="工作台"
        @click="toggleMenu(false)"
      >
        <span
          class="
            px-2 py-0.5 ml-auto
            text-xs font-medium tracking-wide
            text-link-500 bg-primary-50 rounded-full
          "
        >
          PLC
        </span>
      </SidebarMenu>
      <SidebarMenu
        route="/workspace/settings"
        icon="settings"
        text="系统设置"
        @click="toggleMenu(false)"
      />
    </template>

    <template #body>
      <router-view />
    </template>
  </Layout>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Layout from "../../components/Layout/index.vue";
import SidebarMenu from "../../components/Layout/SidebarMenu.vue";

export default defineComponent({
  components: {
    Layout,
    SidebarMenu,
  }
});
</script>
