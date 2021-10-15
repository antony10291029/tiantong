<template>
  <Layout icon="plc">
    <template #title>
      {{plcConfig?.name}}
    </template>

    <template #menu="{ toggleMenu }">
      <SidebarMenu
        :route="{ name: 'PlcSettings', params: { plcId } }"
        icon="settings"
        text="配置管理"
        @click="toggleMenu(false)"
      />

      <SidebarMenu
        :route="{ name: 'PlcDebug', params: { plcId } }"
        icon="debug"
        text="数据调试"
        @click="toggleMenu(false)"
      />

      <SidebarMenu
        route="/"
        text="返回工作台"
        icon="back"
        @click="toggleMenu(false)"
      />
    </template>

    <template #body>
      <router-view
        v-if="plcConfig"
        :plcId="plcId"
        :plcConfig="plcConfig"
        @updated="getPlc"
      />
    </template>
  </Layout>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { PlcConfig, PlcConfigContext } from "../../domain";
import Layout from "../../components/Layout/index.vue";
import SidebarMenu from "../../components/Layout/SidebarMenu.vue";

export default defineComponent({
  components: {
    Layout,
    SidebarMenu
  },

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const plcConfig = ref<PlcConfig>();

    const getPlc = async () => {
      const response = await PlcConfigContext.getById(props.plcId);

      plcConfig.value = response.data;
    };

    onMounted(getPlc);

    return {
      plcConfig,
      getPlc
    };
  }
});
</script>
