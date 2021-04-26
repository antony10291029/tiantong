<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="is-flex">
      <SearchField @search="handleSearch" />

      <div class="is-flex-auto"></div>

      <router-link
        class="button is-info"
        :to="{ name: 'NameiWcsRcsAgcTasksCreate' }"
      >
        创建任务
      </router-link>
    </div>

    <table class="table is-fullwidth is-bordered is-centered is-nowrap">
      <thead>
        <th>编号</th>
        <th>状态</th>
        <th>订单类型</th>
        <th>订单号</th>
        <th>任务编号</th>
        <th>起点</th>
        <th>终点</th>
        <th>托盘码</th>
        <th>AGC编号</th>
        <th>日期</th>
        <th></th>
      </thead>
      <tbody>
        <DataMapIterator
          :dataMap="data"
          v-slot="{ entity }"
          tag="tr"
        >
          <td>{{entity.id}}</td>
          <TheStatus :value="entity.status" />
          <td>{{entity.orderType}}</td>
          <td>{{entity.orderId}}</td>
          <td>{{entity.taskCode}}</td>
          <td>{{entity.position}}</td>
          <td>{{entity.destination}}</td>
          <td>{{entity.podCode}}</td>
          <td>{{entity.agcCode}}</td>
          <td v-text="entity.createdAt" tag="td" />
          <td>
            <a @click="handleClose(entity.id)">
              关闭
            </a>
          </td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.25rem"></div>

    <Pagination
      v-bind="data"
      @change="changePage"
    />

    <router-view @refresh="getTasks" />
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
  name: "PickTicketTasks",

  components: {
    TheStatus,
    SearchField
  },

  setup() {
    const http = useWcsHttp();
    const param = useQuery(10);
    const data = usePagination();
    const confirm = useConfirm();

    async function getTasks() {
      data.value = await http.paginate("/rcs/agc-tasks/search", param);
    }

    function changePage(page: number) {
      param.page = page;

      return getTasks();
    }

    function handleSearch(query: string) {
      param.query = query;

      return getTasks();
    }

    function handleClose(id: number) {
      confirm.open({
        title: "关闭任务",
        content: "确认后任务将被关闭",
        handler: async () => {
          await http.post("/rcs/agc-tasks/close", { id });
          await getTasks();
        }
      });
    }

    return {
      data,
      getTasks,
      changePage,
      handleSearch,
      handleClose,
    };
  }
});
</script>
