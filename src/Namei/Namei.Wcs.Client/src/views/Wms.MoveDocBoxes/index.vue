<template>
  <AsyncLoader
    :handler="getRecords"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="is-flex">
      <SearchField @search="handleSearch" />
      <div class="is-flex-auto"></div>
      <TheCreateDialog @refresh="getRecords"/>
    </div>

    <table class="table is-fullwidth is-bordered is-centered is-nowrap is-clickable is-hoverable">
      <thead>
        <th>编号</th>
        <th>作业单号</th>
        <th>箱码</th>
        <th>绑定日期</th>
        <th>操作</th>
      </thead>
      <tbody>
        <DataMapIterator
          :dataMap="data"
          v-slot="{ entity }"
          tag="tr"
        >
          <td>{{entity.id}}</td>
          <td>{{entity.orderCode}}</td>
          <td>{{entity.itemCode}}</td>
          <TimeWrapper :value="entity.createdAt" tag="td" />
          <td>
            <a @click="handleDelete(entity.id)">
              删除
            </a>
          </td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination v-bind="data" @change="changePage"/>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import { usePagination } from "../../hooks/use-pagination";
import SearchField from "../../components/SearchField.vue";
import TheCreateDialog from "./TheCreate.vue";

export default defineComponent({
  name: "MoveDocItemReviewRecords",

  components: {
    SearchField,
    TheCreateDialog
  },

  setup() {
    const confirm = useConfirm();
    const http = useRcsExtHttp();
    const data = usePagination<any>();
    const params = {
      query: "",
      page: 1,
      pageSize: 100,
    };

    async function getRecords() {
      data.value = await http.paginate(
        "/wms/move-doc-boxes/search",
        params
      );
    }

    function handleSearch(text: string) {
      params.query = text;
      return getRecords();
    }

    function changePage(page: number) {
      params.page = page;
      return getRecords();
    }

    function handleDelete(id: number) {
      confirm.open({
        title: "提示",
        content: "是否删除该箱码绑定记录",
        handler: async () => {
          await http.post(
            "/wms/move-doc-boxes/delete",
            { id }
          );
          await getRecords();
        }
      });
    }

    return {
      data,
      getRecords,
      handleSearch,
      handleDelete,
      changePage
    };
  }
});
</script>
