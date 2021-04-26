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

          <DataMapIterator
            :dataMap="types"
            v-slot="{ entity: taskType }"
          >
            <router-link
              class="panel-block"
              style="padding: 0.25rem 0.75rem"
              :to="{
                name: 'MidosCenterTasTypesType',
                params: { typeId: taskType.id }
              }"
            >
              <div
                class="is-flex is-flex-column"
                style="padding: 0.25rem"
              >
                <span>{{taskType.name}}</span>
              </div>
            </router-link>
          </DataMapIterator>
        </nav>
      </div>

      <router-view
        class="column"
        :key="typeId"
        :typeId="typeId"
        :taskType="taskType"
        :taskTypes="types"
        @refresh="refresh"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { DataMap } from "@midos/core";
import { defineComponent, ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useMidosCenterHttp } from "../../services/midos-center-http";

export default defineComponent({
  name: "Tas.Types",

  setup() {
    const route = useRoute();
    const router = useRouter();
    const http = useMidosCenterHttp();
    const types = ref(new DataMap());
    const typeId = computed(() => +(route.params.typeId ?? 0));
    const taskType = computed(() => types.value.entities[typeId.value]);

    async function getTypes() {
      types.value = await http.post("/midos/tas/types/search");
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
