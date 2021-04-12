<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <SearchField @search="handleSearch" />

    <table class="table is-fullwidth is-bordered is-centered is-nowrap">
      <thead>
        <th>编号</th>
        <th>日期</th>
        <th>捡货单</th>
        <th>货码</th>
        <th>托盘码</th>
        <th>货名</th>
        <th>数量</th>
        <th>库位号</th>
        <th>仓库</th>
      </thead>
      <tbody>
        <DataMapIterator
          :dataMap="data"
          v-slot="{ entity }"
          tag="tr"
        >
          <td>{{entity.id}}</td>
          <TimeWrapper :value="entity.createdAt" tag="td" />
          <td>{{entity.orderNumber}}</td>
          <td>{{entity.itemCode}}</td>
          <td>{{entity.barcode}}</td>
          <td>{{entity.itemName}}</td>
          <td>{{entity.pickedQuantity}}</td>
          <td>{{entity.locationCode}}</td>
          <td>{{entity.fromName}}</td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.25rem"></div>

    <Pagination
      v-bind="data"
      @change="changePage"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { usePagination } from "../../hooks/use-pagination";
import { useQuery } from "../../hooks/use-query";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import SearchField from "../../components/SearchField.vue";

export default defineComponent({
  name: "PickTicketTasks",

  components: {
    SearchField
  },

  setup() {
    const api = useRcsExtHttp();
    const param = useQuery(15);
    const data = usePagination();

    async function getTasks() {
      data.value = await api.paginate("/wms/pick-ticket-tasks/search", param);
    }

    function changePage(page: number) {
      param.page = page;

      return getTasks();
    }

    function handleSearch(query: string) {
      param.query = query;

      return getTasks();
    }

    return {
      data,
      getTasks,
      changePage,
      handleSearch
    };
  }
});
</script>
