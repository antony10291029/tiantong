<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <SearchField @search="handleSearch" />

    <table class="table is-fullwidth is-bordered is-centered is-nowrap is-clickable is-hoverable">
      <thead>
        <th>编号</th>
        <th>状态</th>
        <th>日期</th>
        <th>捡货单</th>
        <th>托盘码</th>
        <th>库位号</th>
        <th>仓库</th>
        <th>数量</th>
        <th>剩余库存</th>
        <th>货名</th>
        <th>货码</th>
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
          <TimeWrapper :value="entity.createdAt" tag="td" />
          <td>{{entity.orderNumber}}</td>
          <td>{{entity.palletCode}}</td>
          <td>{{entity.locationCode}}</td>
          <td>{{entity.fromName}}</td>
          <td>{{entity.pickedQuantity}}</td>
          <td>{{entity.restQuantity ?? 0}}</td>
          <td>{{entity.itemName}}</td>
          <td>{{entity.itemCode}}</td>
          <td>
            <TheOperation
              :entity="entity"
              @refresh="getTasks"
            />
          </td>
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
import { UseDataMap } from "../../hooks/use-data-map";
import { useQuery } from "../../hooks/use-query";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import SearchField from "../../components/SearchField.vue";
import TheStatus from "./TheStatus.vue";
import TheOperation from "./TheOperation.vue";

export default defineComponent({
  name: "PickTicketTasks",

  components: {
    SearchField,
    TheStatus,
    TheOperation
  },

  setup() {
    const api = useRcsExtHttp();
    const param = useQuery(100);
    const data = usePagination<any>();
    const pallets = UseDataMap<any>();

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
      pallets,
      getTasks,
      changePage,
      handleSearch
    };
  }
});
</script>
