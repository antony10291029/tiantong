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
          <DataMapIterator
            :dataMap="routes"
            tag="tr"
            v-slot="{ entity, index }"
          >
            <td>{{index + 1}}</td>
            <td>{{entity.name}}</td>
            <td>{{entity.path}}</td>
            <td>{{entity.endpoint.name}}</td>
            <td>{{entity.endpoint.url + entity.endpointPath}}</td>
            <TheUpdate
              :route="entity"
              @refresh="getRoutes"
            />
          </DataMapIterator>
        </tbody>
      </table>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DataMap } from "@midos/core";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";
import TheCreate from "./Create.vue";
import TheUpdate from "./Update.vue";

export default defineComponent({
  components: {
    TheCreate,
    TheUpdate
  },

  setup() {
    const http = UseApiGatewayHttp();
    const routes = ref(new DataMap());

    async function getRoutes() {
      routes.value = await http.paginate("/$routes/all");
    }

    return {
      routes,
      getRoutes
    };
  }
});
</script>
