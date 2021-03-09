<template>
  <div
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />

      <slot></slot>

      <table class="table is-centered is-bordered is-nowrap">
        <thead>
          <th class="is-narrow"></th>
          <th>时间</th>
          <th>类别</th>
          <th>操作</th>
          <th>对象</th>
          <th>消息</th>
          <th>数据</th>
        </thead>
        <tbody>
          <tr v-for="log in logs.data" :key="log.id">
            <td class="is-centered">
              <span :class="`icon has-text-${log.level}`">
                <i :class="`icon-midos icon-midos-${log.level}`"
                style="font-size: 1.25rem"></i>
              </span>
            </td>
            <td>{{log.createdAt.split('T').join(' ')}}</td>
            <td>{{log.class}}</td>
            <td>{{log.operation}}</td>
            <td>{{log.index}}</td>
            <td class="has-text-left">{{log.message}}</td>
            <td>{{log.data}}</td>
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
      data: [] as any[],
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
