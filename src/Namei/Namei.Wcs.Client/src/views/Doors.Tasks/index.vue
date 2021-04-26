<template>
  <AsyncLoader :handler="getTasks">
    <div class="box" style="margin: 1.25rem">
      <SearchField @search="handleSearch" />

      <table class="table is-fullwidth is-centered">
        <thead>
          <th>封锁点</th>
          <th>请求编号</th>
          <th>状态</th>
          <th>失败重试</th>
          <th>请求开门</th>
          <th>开门完成</th>
          <th>请求关门</th>
        </thead>
        <tbody>
          <DataMapIterator
            :dataMap="tasks"
            v-slot="{ entity }"
            tag="tr"
          >
            <td>{{`${entity.doorId}`}}</td>
            <td>{{entity.id}}</td>
            <TheStatus :value="entity.status" />
            <td>{{`${entity.retryCount} 次`}}</td>
            <td>{{entity.requestedAt.split('T').join(' ')}}</td>
            <td>{{entity.enteredAt.split('T').join(' ')}}</td>
            <td>{{entity.leftAt.split('T').join(' ')}}</td>
          </DataMapIterator>
        </tbody>
      </table>

      <div style="height: 1.25rem"></div>

      <Pagination v-bind="tasks" @change="changePage" />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { usePagination } from "../../hooks/use-pagination";
import { useQuery } from "../../hooks/use-query";
import { useWcsHttp } from "../../services/wcs-http";
import SearchField from "../../components/SearchField.vue";
import TheStatus from "./TheStatus.vue";

export default defineComponent({
  name: "DoorTasks",

  components: {
    TheStatus,
    SearchField
  },

  setup() {
    const http = useWcsHttp();
    const tasks = usePagination();
    const params = useQuery();

    async function getTasks() {
      tasks.value = await http.paginate("/door-tasks/search", params);
    }

    function changePage(page: number) {
      params.page = page;

      return getTasks();
    }

    function handleSearch(query: string) {
      params.page = 1;
      params.query = query;
      console.log(1000);

      return getTasks();
    }

    return {
      tasks,
      getTasks,
      changePage,
      handleSearch
    };
  }
});
</script>
