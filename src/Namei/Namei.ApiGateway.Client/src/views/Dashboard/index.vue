<template>
  <AsyncLoader
    :handler="getLogs"
    style="padding: 1.25rem; overflow: auto"
    class="has-background-white"
  >
    <table class="table is-bordered is-nowrap is-fullwidth">
      <thead>
        <th>#</th>
        <th>时间</th>
        <th>方法</th>
        <th>路由</th>
        <th>重定向</th>
        <th>请求数据</th>
        <th>响应数据</th>
      </thead>

      <tbody>
        <DataMapIterator
          :dataMap="logs"
          v-slot="{ entity, index }"
          tag="tr"
        >
          <td>{{index + 1}}</td>
          <td>
            <TimeWrapper :value="entity.requestedAt" />
          </td>
          <td>{{entity.requestMethod}}</td>
          <td>{{entity.sourcePath}}</td>
          <td>{{entity.requestUri}}</td>
          <td>{{entity.requestBody}}</td>
          <td>{{entity.responseBody}}</td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination v-bind="logs" />
  </AsyncLoader>
</template>

<script lang="ts">
import { Pagination } from "@midos/core";
import { defineComponent, ref } from "vue";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";

export default defineComponent({
  setup() {
    const http = UseApiGatewayHttp();
    const params = ref({
      page: 1,
      pageSize: 100,
      query: "",
      path: "",
    });
    const logs = ref(new Pagination<any>());

    async function getLogs() {
      const result = await http.paginate(
        "/$http-logs/search",
        params.value
      );

      logs.value = result;
    }

    return {
      logs,
      getLogs
    };
  }
});
</script>
