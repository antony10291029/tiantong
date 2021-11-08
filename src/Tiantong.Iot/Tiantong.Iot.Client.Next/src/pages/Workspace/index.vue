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
      </SidebarMenu>
      <SidebarMenu
        route="/workspace/settings"
        icon="settings"
        text="设置"
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
