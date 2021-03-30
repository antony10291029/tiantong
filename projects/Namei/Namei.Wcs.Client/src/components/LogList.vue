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
        <th style="text-align: left">数据</th>
      </thead>
      <tbody>
        <DataMapIterator
          tag="tr"
          :dataMap="logs"
          v-slot="{ entity }"
        >
          <td class="is-centered">
            <span :class="`icon has-text-${entity.level}`">
              <i :class="`icon-midos icon-midos-${entity.level}`"
              style="font-size: 1.25rem"></i>
            </span>
          </td>
          <td>{{entity.createdAt.split('T').join(' ')}}</td>
          <td>{{entity.class}}</td>
          <td>{{entity.operation}}</td>
          <td>{{entity.index}}</td>
          <td class="has-text-left">{{entity.message}}</td>
          <td style="text-align: left">{{entity.data}}</td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination
      v-bind="logs"
      @change="getDataSource"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from "vue";
import { Pagination } from "@midos/core";
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
      default: () => []
    }
  },

  setup(props) {
    const http = useWcsHttp();

    const query = ref("");
    const isPending = ref(false);
    const logs = ref(new Pagination<any>());

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

    useInterval(getDataSource, computed(() => logs.value.page === 1));
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
