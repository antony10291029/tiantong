<template>
  <p class="title is-5">
    路由表同步
  </p>

  <p class="content">
    当系统路由表发生变更时，须手动执行同步操作。
  </p>

  <template v-if="proxyTable && isChanged">
    <p class="has-text-danger">
      检测到路由表变更，请执行更新
    </p>

    <div style="height: 1.5rem"></div>
  </template>

  <AsyncButton
    :handler="handleClick"
    class="button is-info is-small is-light"
  >
    更新
  </AsyncButton>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from "vue";
import isEqual from "lodash/isEqual";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";

export default defineComponent({
  name: "ClearLogs",

  setup() {
    const http = UseApiGatewayHttp();
    const proxyTable = ref<any>(null);
    const currentProxyTable = ref<any>(null);
    const isChanged = computed(() => !isEqual(
      proxyTable.value,
      currentProxyTable.value
    ));

    async function getProxyTable() {
      currentProxyTable.value = await http.post("/$proxy-table/get");
      proxyTable.value = await http.post("/$proxy-table/fetch");
    }

    async function handleClick () {
      await http.post("/$proxy-table/sync");
      await getProxyTable();
    }

    onMounted(getProxyTable);

    return {
      isChanged,
      proxyTable,
      currentProxyTable,
      handleClick,
    };
  }
});
</script>
