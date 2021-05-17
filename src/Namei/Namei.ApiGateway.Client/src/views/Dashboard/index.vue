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
        <th>耗时</th>
        <th>方法</th>
        <th>路由</th>
        <th>重定向</th>
        <th>请求数据</th>
        <th>响应数据</th>
      </thead>

      <tbody>
        <DataMap
          :dataMap="logs"
          v-slot="{ value, index }"
          tag="tr"
        >
          <td>{{index + 1}}</td>
          <td>
            <TimeWrapper :value="value.requestedAt" />
          </td>
          <td>
            <span class="tag is-success is-light">
              <span>
                {{getTimeConsuming(value)}}
              </span>
              <span>ms</span>
            </span>
          </td>
          <td>{{value.requestMethod}}</td>
          <td>{{value.sourcePath}}</td>
          <td>{{value.requestUri}}</td>
          <td>{{value.requestBody}}</td>
          <td>{{value.responseBody}}</td>
        </DataMap>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination v-bind="logs" />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { PaginateParams, Pagination } from "@midos/seed-work";
import { HttpLog } from "../../domain/entities/http-log";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";

export default defineComponent({
  setup() {
    const http = UseApiGatewayHttp();
    const params = ref(new PaginateParams());
    const logs = ref(new Pagination<HttpLog>());

    async function getLogs() {
      const result = await http.post<any>(
        "/$http-logs/search",
        params.value
      );

      logs.value = result;
    }

    function getTimeConsuming(entity: any) {
      const requestedAt = Date.parse(entity.requestedAt);
      const responsedAt = Date.parse(entity.responsedAt);

      return responsedAt - requestedAt;
    }

    return {
      logs,
      getLogs,
      getTimeConsuming
    };
  }
});
</script>
