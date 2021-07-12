<template>
  <AsyncLoader :handler="getLogs">
    <div class="box">
      <SearchField @search="handleSearch" />

      <table class="table is-clickable is-hoverable is-nowrap is-fullwidth is-centered">
      <thead>
        <th style="width: 1px">#</th>
        <th>日期</th>
        <th>时间</th>
        <th>服务</th>
        <th>方法</th>
        <th>状态</th>
        <th>耗时</th>
      </thead>

      <tbody>
        <DataMap
          :dataMap="logs"
          v-slot="{ value, index }"
        >
          <HttpLogRow
            :httpLog="value"
          >
            <td>{{index + 1}}</td>
            <td>
              {{value.requestedAt.split('T')[0]}}
            </td>
            <td>
              {{value.requestedAt.split('T')[1].split('.')[0]}}
            </td>
            <td>
              {{value.requestHeaders.Host[0]}}
            </td>
            <td>
              {{value.requestMethod}}
            </td>
            <td>
              {{value.responseStatus}}
            </td>
            <td>
              <span class="tag is-success is-light">
                <span>
                  {{getTimeConsuming(value)}}
                </span>
                <span>ms</span>
              </span>
            </td>
          </HttpLogRow>
        </DataMap>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination
      v-bind="logs"
      @change="changePage"
    />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref, PropType, computed } from "vue";
import { PaginateParams, Pagination } from "@midos/seed-work";
import { HttpLog } from "../../domain/entities/http-log";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";
import { Route } from "../../domain/entities";
import HttpLogRow from "./HttpLog.vue";
import SearchField from "../../components/SearchField.vue";

class QueryParams extends PaginateParams {
  public sourcePath = "";

  public constructor(sourcePath: string) {
    super(10);
    this.sourcePath = sourcePath;
  }
}

export default defineComponent({
  components: {
    HttpLogRow,
    SearchField
  },

  props: {
    routeId: {
      type: String,
      required: true
    },

    routes: {
      type: Object as PropType<{[ key: string ]: Route }>,
      required: true
    }
  },

  setup(props) {
    const http = UseApiGatewayHttp();
    const logs = ref(new Pagination<HttpLog>());
    const route = computed(() => props.routes[props.routeId]);
    const params = ref(new QueryParams(route.value.path));

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

    async function handleSearch(text: string) {
      if (!text.trim()) {
        return;
      }

      params.value.query = text;

      await getLogs();
    }

    function changePage(page: number) {
      params.value.page = page;

      return getLogs();
    }

    return {
      logs,
      route,
      getLogs,
      changePage,
      handleSearch,
      getTimeConsuming
    };
  }
});
</script>
