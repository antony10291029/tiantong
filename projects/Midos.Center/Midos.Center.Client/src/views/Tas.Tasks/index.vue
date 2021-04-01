<template>
  <AsyncLoader
    :handler="getTypes"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="columns" >
      <div class="column is-narrow">
        <nav class="panel" style="min-width: 320px">
          <p class="panel-heading is-flex">
            <span>任务类型</span>
          </p>

          <router-link
            class="panel-block"
            v-for="id in types.keys" :key="id"
            style="padding: 0.25rem 0.75rem"
            :to="{
              name: 'TasOrders',
              params: { typeId: id }
            }"
          >
            <div
              class="is-flex is-flex-column"
              style="padding: 0.25rem"
            >
              <span>{{types.entities[id].name}}</span>
            </div>
          </router-link>
        </nav>
      </div>

      <router-view
        class="column"
        v-if="typeId"
        :key="typeId"
        :typeId="typeId"
        :taskType="taskType"
        :taskTypes="types"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import { useRoute } from "vue-router";
import { DataMap } from "@midos/core";
import { useMidosCenterHttp } from "../../services/midos-center-http";

export default defineComponent({
  name: "Tas.Tasks",

  setup() {
    const route = useRoute();
    const http = useMidosCenterHttp();
    const types = ref(new DataMap());
    const typeId = computed(() => +route.params.typeId);
    const taskType = computed(() => types.value.entities[typeId.value]);

    async function getTypes() {
      const result = await http.getDataMap<any>("/midos/tas/types/search");

      types.value = result;
    }

    return {
      types,
      typeId,
      taskType,
      getTypes
    };
  }
});
</script>
