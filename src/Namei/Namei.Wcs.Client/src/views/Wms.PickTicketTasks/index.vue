<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <SearchField @search="handleSearch" />

    <table class="table is-fullwidth is-bordered is-centered is-nowrap is-clickable is-hoverable">
      <thead>
        <th>#</th>
        <th>ID</th>
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
        <DataMap
          :dataMap="data"
          v-slot="{ value, index }"
          tag="tr"
        >
          <td>{{index + 1}}</td>
          <td>{{value.id}}</td>
          <TheStatus :value="value.status" />
          <TimeWrapper :value="value.createdAt" tag="td" />
          <td>{{value.orderNumber}}</td>
          <td>{{value.palletCode}}</td>
          <td>{{value.locationCode}}</td>
          <td>{{value.fromName}}</td>
          <td>{{value.pickedQuantity}}</td>
          <td>
            {{restQuantities[value.palletCode]?.restQuantity ?? 0}}
          </td>
          <td>{{value.itemName}}</td>
          <td>{{value.itemCode}}</td>
          <td>
            <TheOperation
              v-if="isRestQuantityLoaded"
              :entity="value"
              :restQuantity="restQuantities[value.palletCode]?.restQuantity ?? 0"
              @refresh="getTasks"
            />
          </td>
        </DataMap>
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
import { defineComponent, ref } from "vue";
import { DataMap, PaginateParams, Pagination } from "@midos/seed-work";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import { WmsPickTicketTask } from "./entities/pick-ticket-task";
import { RestQuantity } from "./entities/rest-quantity";
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
    const param = ref(new PaginateParams(100));
    const data = ref(new Pagination<WmsPickTicketTask>());
    const restQuantities = ref<any>({});
    const isRestQuantityLoaded = ref(false);

    async function getTasks() {
      data.value = await api.post<Pagination<WmsPickTicketTask>>(
        "/wms/pick-ticket-tasks/search", param.value
      );

      const palletCodes = data.value.keys.map(
        key => data.value.values[key].palletCode
      );

      api.post<DataMap<RestQuantity>>(
        "/wms/inventory-rest-quantity/query",
        { codes: palletCodes }
      ).then(result => {
        restQuantities.value = result;
        isRestQuantityLoaded.value = true;
      });
    }

    function changePage(page: number) {
      param.value.page = page;

      return getTasks();
    }

    function handleSearch(query: string) {
      param.value.query = query;

      return getTasks();
    }

    return {
      data,
      restQuantities,
      isRestQuantityLoaded,
      getTasks,
      changePage,
      handleSearch
    };
  }
});
</script>
