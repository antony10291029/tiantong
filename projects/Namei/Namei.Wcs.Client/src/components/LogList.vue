<template>
  <div class="box">
    <SearchField
      :isPending="isPending"
      @search="handleSearch"
    />

    <slot></slot>

    <table class="table is-fullwidth is-bordered">
      <thead>
        <th>运行记录</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="log in logs.data" :key="log.id">
          <td>{{log.message}}</td>
          <td>{{log.createdAt.split('T').join(' ')}}</td>
        </tr>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination
      v-bind="logs.meta"
      @change="getDataSource"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from "vue";
import { useInterval } from "@midos/vue-ui";
import { useWcsHttp } from "../services/wcs-http";
import SearchField from "./SearchField.vue";

export default defineComponent({
  name: "LifterLogs",

  components: {
    SearchField
  },

  props: {
    classes: {
      type: Object as PropType<string[]>,
      required: true
    }
  },

  setup(props) {
    const http = useWcsHttp();

    const query = ref("");
    const isPending = ref(false);
    const logs = ref({
      data: [],
      meta: {
        page: 1,
        pageSize: 15,
        total: 1
      }
    });

    async function getDataSource (page = 1) {
      const result = await http.post("/logs/search", {
        page,
        query: [query.value],
        classes: props.classes,
      });

      logs.value = result;
    }

    async function handleSearch (text: string) {
      try {
        isPending.value = true;
        query.value = text;
        await getDataSource();
      } finally {
        isPending.value = false;
      }
    }

    useInterval(getDataSource, computed(() => logs.value.meta.page === 1));
    getDataSource();

    return {
      query,
      isPending,
      logs,
      getDataSource,
      handleSearch,
    };
  }
});
</script>
