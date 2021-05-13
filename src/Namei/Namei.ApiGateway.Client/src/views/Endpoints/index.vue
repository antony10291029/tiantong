<template>
  <AsyncLoader
    :handler="getEndpoints"
    style="padding: 1.25rem;"
  >
    <div class="columns">
      <div class="column is-narrow">
        <nav class="panel" style="width: 320px">
          <p class="panel-heading is-flex">
            <span>服务列表</span>
            <span class="is-flex-auto"></span>
            <router-link :to="{ name: 'ApiGatewayEndpointsCreate' }">
              添加
            </router-link>
          </p>

          <DataMapIterator
            :dataMap="endpoints"
            v-slot="{ entity }"
          >
            <router-link
              class="panel-block"
              :to="{
                name: 'ApiGatewayEndpointsEndpoint',
                params: { endpointId: entity.id }
              }"
            >
              <div class="is-flex is-flex-column">
                <span>{{entity.name}}</span>
                <span class="has-text-grey">
                  {{entity.url}}
                </span>
              </div>
            </router-link>
          </DataMapIterator>
        </nav>
      </div>
      <router-view
        :key="$route.path"
        :endpoints="endpoints.entities"
        @refresh="getEndpoints"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DataMap } from "@midos/core";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";

export default defineComponent({
  setup() {
    const http = UseApiGatewayHttp();
    const endpoints = ref(new DataMap());
    const params = ref({
      page: 1,
      pageSize: 15,
      query: ""
    });
    const endpointId = ref(0);

    async function getEndpoints() {
      endpoints.value = await http.getDataMap<any>("$endpoints/search", params.value);

      if (endpointId.value === 0 && endpoints.value.keys.length > 0) {
        [endpointId.value] = endpoints.value.keys;
      }
    }

    return {
      endpoints,
      getEndpoints
    };
  }
});
</script>
