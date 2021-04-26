<template>
  <AsyncLoader :handler="getTasks">
    <div class="box" style="margin: 1.25rem">
      <SearchField @search="handleSearch" />

      <table class="table is-fullwidth is-centered">
        <thead>
          <th>提升机</th>
          <th>托盘号</th>
          <th>TaskCode</th>
          <th>楼层</th>
          <th>状态</th>
          <th>放货完成</th>
          <th>请求取货</th>
          <th>取货完成</th>
          <th></th>
        </thead>
        <tbody>
          <DataMapIterator
            :dataMap="tasks"
            v-slot="{ entity }"
            tag="tr"
          >
            <td>{{`${entity.lifterId} 号梯`}}</td>
            <td>{{entity.barcode}}</td>
            <td>{{entity.taskCode}}</td>
            <td>{{`${entity.floor}F - ${entity.destination}F`}}</td>
            <TheStatus :value="entity.status" />
            <td>{{entity.importedAt.split('T').join(' ')}}</td>
            <td>{{entity.exportedAt.split('T').join(' ')}}</td>
            <td>{{entity.takenAt.split('T').join(' ')}}</td>
            <td>
              <a @click="handleClose(entity.id)">
                关闭
              </a>
            </td>
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
import { useConfirm } from "@midos/vue-ui";
import { usePagination } from "../../hooks/use-pagination";
import { useQuery } from "../../hooks/use-query";
import { useWcsHttp } from "../../services/wcs-http";
import SearchField from "../../components/SearchField.vue";
import TheStatus from "./TheStatus.vue";

export default defineComponent({
  name: "LifterTasks",

  components: {
    TheStatus,
    SearchField
  },

  setup() {
    const confirm = useConfirm();
    const http = useWcsHttp();
    const tasks = usePagination();
    const params = useQuery();

    async function getTasks() {
      tasks.value = await http.paginate("/lifter-tasks/search", params);
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

    function handleClose(id: number) {
      confirm.open({
        title: "关闭任务",
        content: "关闭后将改变任务状态",
        handler: async () => {
          await http.post("/lifter-tasks/close", { id });
          await getTasks();
        }
      });
    }

    return {
      tasks,
      getTasks,
      changePage,
      handleSearch,
      handleClose,
    };
  }
});
</script>
