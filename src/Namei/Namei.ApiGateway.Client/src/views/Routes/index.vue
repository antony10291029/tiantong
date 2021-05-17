<template>
  <AsyncLoader
    :handler="getRoutes"
    style="padding: 1.25rem"
  >
    <div class="box">
      <table class="table is-centered is-hoverable is-bordered is-nowrap is-fullwidth">
        <thead>
          <th style="width: 1px">#</th>
          <th>名称</th>
          <th>路径</th>
          <th>所属系统</th>
          <th>反向代理地址</th>
          <th style="width: 1px">
            <TheCreate @refresh="getRoutes" />
          </th>
        </thead>
        <tbody>
          <DataMap
            :dataMap="routes"
            tag="tr"
            v-slot="{ value, index }"
          >
            <td>{{index + 1}}</td>
            <td>{{value.name}}</td>
            <td>{{value.path}}</td>
            <td>{{value.endpoint.name}}</td>
            <td>{{value.endpoint.url + value.endpointPath}}</td>
            <TheUpdate
              :route="value"
              @refresh="getRoutes"
            />
          </DataMap>
        </tbody>
      </table>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { QueryParams, DataMap } from "@midos/seed-work";
import { useService } from "@midos/vue-ui";
import { RouteRepository } from "../../domain/repositories/route-repository";
import { Route } from "../../domain/entities";
import TheCreate from "./Create.vue";
import TheUpdate from "./Update.vue";

export default defineComponent({
  components: {
    TheCreate,
    TheUpdate
  },

  setup() {
    const repository = useService(RouteRepository);
    const routes = ref(new DataMap<Route>());
    const params = ref(new QueryParams());

    async function getRoutes() {
      routes.value = await repository.query(params.value);
    }

    return {
      routes,
      getRoutes
    };
  }
});
</script>
