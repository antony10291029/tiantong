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

          <DataMap
            :dataMap="endpoints"
            v-slot="{ value }"
          >
            <router-link
              class="panel-block"
              :to="{
                name: 'ApiGatewayEndpointsEndpoint',
                params: { endpointId: value.id }
              }"
            >
              <div class="is-flex is-flex-column">
                <span>{{value.name}}</span>
                <span class="has-text-grey">
                  {{value.url}}
                </span>
              </div>
            </router-link>
          </DataMap>
        </nav>
      </div>
      <router-view
        :key="$route.path"
        :endpoints="endpoints.values"
        @refresh="getEndpoints"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DataMap, QueryParams } from "@midos/seed-work";
import { Endpoint } from "../../domain/entities";
import { useEndpointRepository } from "../../domain/repositories/endpoint-repository";

export default defineComponent({
  setup() {
    const repository = useEndpointRepository();
    const endpoints = ref(new DataMap<Endpoint>());
    const params = ref(new QueryParams());
    async function getEndpoints() {
      endpoints.value = await repository.query(params.value);
    }

    return {
      endpoints,
      getEndpoints
    };
  }
});
</script>
