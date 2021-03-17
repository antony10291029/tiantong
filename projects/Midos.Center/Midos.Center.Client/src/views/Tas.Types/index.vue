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
            <span class="is-flex-auto"></span>
            <router-link
              :to="{ name: 'MidosCenterTasTypesCreate' }"
            >
              添加
            </router-link>
          </p>

          <router-link
            class="panel-block"
            v-for="id in types.result" :key="id"
            style="padding: 0.25rem 0.75rem"
            :to="{
              name: 'MidosCenterTasTypesType',
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
        :typeId="typeId"
        :taskType="taskType"
        :taskTypes="types"
        @refresh="refresh"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useMidosCenterHttp } from "../../services/midos-center-http";

export default defineComponent({
  name: "Tas.Types",

  setup() {
    const route = useRoute();
    const router = useRouter();
    const http = useMidosCenterHttp();
    const types = ref<any>({
      result: [] as string[],
      entities: {} as { [ key: string ]: any }
    });
    const typeId = computed(() => +route.params.typeId);
    const taskType = computed(() => types.value.entities[typeId.value]);

    async function getTypes() {
      const result = await http.post("/midos/tas/types/search");

      types.value = result;
    }

    async function refresh(id: number) {
      await getTypes();

      if (id === 0) {
        router.push({
          name: "MidosCenterTasTypes"
        });
      } else {
        router.push({
          name: "MidosCenterTasTypesType",
          params: { typeId: id }
        });
      }
    }

    return {
      types,
      typeId,
      taskType,
      getTypes,
      refresh
    };
  }
});
</script>
