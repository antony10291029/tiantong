<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="is-flex" style="margin-bottom: 0.75rem">
      <SearchField
        @search="handleSearch"
        style="margin-bottom: 0"
      >
        <div class="control">
          <DatePicker
            v-model:value="param.date"
            initial="now"
            style="width: 120px"
          />
        </div>
      </SearchField>

      <AsyncButton
        class="button is-info"
        style="margin-left: 0.75rem; margin-bottom: 0"
        v-if="selectedStatus !== false"
        :handler="handleTasksCreate"
      >
        批量下发
      </AsyncButton>
    </div>

    <table
      v-show="!isPending"
      class="table is-fullwidth is-bordered is-centered is-nowrap is-clickable is-hoverable"
    >
      <thead>
        <th @click="selectAll">
          <Checkbox :value="selectedStatus"/>
        </th>
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
          <td @click="selectTask(value.id)">
            <Checkbox :value="selectedTasks.indexOf(value.id) !== -1" />
          </td>
          <td>{{index + 1}}</td>
          <td>{{value.id}}</td>
          <TheStatus :value="value.status" />
          <TimeWrapper :value="value.createdAt" tag="td" />
          <td>{{value.orderNumber}}</td>
          <td>{{value.palletCode}}</td>
          <td>{{value.locationCode}}</td>
          <td>{{value.fromName}}</td>
          <td>{{value.pickedQuantity}}</td>
          <TheRestQuantityCell
            :value="getRestQuantity(value)"
          />
          <td>{{value.itemName}}</td>
          <td>{{value.itemCode}}</td>
          <td>
            <TheOperation
              v-if="isRestQuantityLoaded"
              :entity="value"
              :restQuantity="getRestQuantity(value)"
              @refresh="getTasks"
            />
          </td>
        </DataMap>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { DataMap, Pagination } from "@midos/seed-work";
import { useConfirm, DateTime } from "@midos/vue-ui";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import { WmsPickTicketTask } from "./entities/pick-ticket-task";
import { RestQuantity } from "./entities/rest-quantity";
import SearchField from "../../components/SearchField.vue";
import TheStatus from "./TheStatus.vue";
import TheOperation from "./TheOperation.vue";
import TheRestQuantityCell from "./TheRestQuantityCell.vue";
import DatePicker from "../../components/DatePicker.vue";

type RestQuantityMap = { [ key: string ]: RestQuantity }

export default defineComponent({
  name: "PickTicketTasks",

  components: {
    SearchField,
    TheStatus,
    TheOperation,
    TheRestQuantityCell,
    DatePicker,
  },

  setup() {
    const api = useRcsExtHttp();
    const confirm = useConfirm();
    const isPending = ref(false);
    const param = ref({ date: DateTime.now, query: "" });
    const data = ref(new DataMap<WmsPickTicketTask>());
    const restQuantities = ref<RestQuantityMap>({});
    const isRestQuantityLoaded = ref(false);
    const selectedTasks = ref<number[]>([]);
    const selectedStatus = computed(() => {
      if (selectedTasks.value.length === 0) {
        return false;
      }

      if (selectedTasks.value.length === data.value.keys.length) {
        return true;
      }

      return "minus";
    });

    async function getTasks() {
      isPending.value = true;

      try {
        data.value = await api.post<Pagination<WmsPickTicketTask>>(
          "/wms/pick-ticket-tasks/search", param.value
        );
      } finally {
        isPending.value = false;
      }

      const palletCodes = data.value.keys.map(
        key => data.value.values[key].palletCode
      );

      api.post<RestQuantityMap>(
        "/wms/inventory-rest-quantity/query",
        { codes: palletCodes }
      ).then(result => {
        restQuantities.value = result;
        isRestQuantityLoaded.value = true;
      });
    }

    function handleSearch(query: string) {
      param.value.query = query;

      return getTasks();
    }

    function selectTask(id: number) {
      const index = selectedTasks.value.indexOf(id);

      if (index === -1) {
        selectedTasks.value.push(id);
      } else {
        selectedTasks.value.splice(index, 1);
      }
    }

    function selectAll() {
      if (selectedTasks.value.length !== data.value.keys.length) {
        selectedTasks.value = [...data.value.keys];
      } else {
        selectedTasks.value = [];
      }
    }

    function getRestQuantity(task: WmsPickTicketTask) {
      return restQuantities.value[task.palletCode]?.restQuantity ?? 0;
    }

    function handleTasksCreate() {
      const params = selectedTasks.value
        .map(id => data.value.values[id])
        .filter(task => task.status === null)
        .map(task => ({
          taskId: task.id,
          position: task.locationCode,
          // eslint-disable-next-line no-template-curly-in-string
          destination: getRestQuantity(task) ? "204${04}" : "294${04}",
          palletCode: task.palletCode,
        }));

      confirm.open({
        title: "提示",
        content: `确认将执行未下发的 ${params.length} 条任务`,
        handler: async () => await api.post("/wms/pick-ticket-tasks/start-batch", params)
      });
    }

    return {
      data,
      isPending,
      isRestQuantityLoaded,
      selectedTasks,
      selectedStatus,
      param,
      getTasks,
      selectTask,
      selectAll,
      handleSearch,
      getRestQuantity,
      handleTasksCreate
    };
  }
});
</script>
