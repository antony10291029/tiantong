<template>
  <AsyncLoader
    :handler="getRoutes"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
    v-slot="{ isPending }"
  >
    <div
      v-if="!isPending"
      class="columns"
    >
      <div class="column is-narrow">
        <nav class="panel is-light" style="max-width: 360px; overflow: hidden">
          <p class="panel-heading">
            接口列表
          </p>
          <div class="panel-block">
            <p class="control has-icons-left">
              <Input
                class="input is-small"
                v-model:value="search"
              />
              <span class="icon is-left">
                <i class="icon-midos icon-midos-search" aria-hidden="true"></i>
              </span>
            </p>
          </div>

          <DataMap
            :dataMap="routes"
            v-slot="{ value }"
          >
            <router-link
              v-show="matchRouteName(value.name)"
              class="panel-block"
              :to="{
                name: 'ApiGatewayDashboardHttpLogs',
                params: { routeId: value.id }
              }"
            >
              <div class="is-flex is-flex-column">
                <span>{{value.name}}</span>
                <span class="has-text-grey">
                  {{value.path}}
                </span>
              </div>
            </router-link>
          </DataMap>
        </nav>
      </div>

      <div class="column">
        <router-view
          :key="+$route.params.routeId"
          :routeId="$route.params.routeId"
          :routes="routes.values"
        />
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { QueryParams, DataMap } from "@midos/seed-work";
import { useService } from "@midos/vue-ui";
import { Route } from "../../domain/entities";
import { RouteRepository } from "../../domain/repositories/route-repository";

export default defineComponent({
  setup() {
    const repository = useService(RouteRepository);
    const routes = ref(new DataMap<Route>());
    const params = ref(new QueryParams());
    const search = ref("");

    async function getRoutes() {
      routes.value = await repository.query(params.value);
    }

    function matchRouteName(name: string) {
      if (search.value === "") {
        return true;
      }

      return name.toLowerCase().search(search.value) !== -1;
    }

    return {
      search,
      routes,
      getRoutes,
      matchRouteName
    };
  }
});
</script>
