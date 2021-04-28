<template>
  <AsyncLoader
    :handler="getTypes"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="is-flex">
      <SearchField @search="handleSearch" />
      <div class="is-flex-auto" />
      <TheCreateModal @refresh="getTypes" />
    </div>

    <table class="table is-fullwidth is-bordered is-centered">
      <thead>
        <th>#</th>
        <th>Key</th>
        <th>任务名</th>
        <th>方法</th>
        <th>Hook</th>
        <th></th>
      </thead>

      <tbody>
        <DataMapIterator
          :dataMap="types"
          v-slot="{ entity }"
          tag="tr"
        >
          <td>{{entity.id}}</td>
          <td>{{entity.key}}</td>
          <td>{{entity.name}}</td>
          <td>{{entity.method}}</td>
          <td>{{entity.webhook}}</td>
          <td>
            <TheUpdateModal
              :params="entity"
              @refresh="getTypes"
            />
          </td>
        </DataMapIterator>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { UseDataMap } from "../../hooks/use-data-map";
import { useWcsHttp } from "../../services/wcs-http";
import SearchField from "../../components/SearchField.vue";
import TheCreateModal from "./TheCreateModal.vue";
import TheUpdateModal from "./TheUpdateModal.vue";

export default defineComponent({
  components: {
    SearchField,
    TheCreateModal,
    TheUpdateModal
  },

  setup() {
    const http = useWcsHttp();
    const types = UseDataMap<any>();
    const params = ref({
      query: ""
    });

    async function getTypes() {
      types.value = await http.post("/rcs-agc-task-type/all", params.value);
    }

    function handleSearch(text: string) {
      params.value.query = text;

      return getTypes();
    }

    return {
      types,
      getTypes,
      handleSearch
    };
  }
});
</script>
