<template>
  <AsyncLoader
    :handler="getStates"
    #default="{ isStatesPending }"
  >
    <AsyncLoader
      v-if="!isStatesPending"
      :handler="getLogs"
      #default="{ isPending }"
    >
      <template v-if="!isPending">
        <Table
          colspan="6"
          class="table is-centered is-bordered is-fullwidth"
        >
          <template #head>
            <thead>
              <th>数据点</th>
              <th>地址</th>
              <th>类型</th>
              <th>操作</th>
              <th>数据</th>
              <th>时间</th>
            </thead>
          </template>
          <template v-if="logs.data.length" #body>
            <tbody>
              <tr v-for="log in logs.data" :key="log.id">
                <td>{{states[log.state_id].name}}</td>
                <td>{{states[log.state_id].address}}</td>
                <td>{{states[log.state_id].type}}</td>
                <td>{{log.operation}}</td>
                <td>{{log.value}}</td>
                <td>{{log.created_at.split('.')[0]}}</td>
              </tr>
            </tbody>
          </template>
        </Table>

        <div style="height: 0.75rem"></div>

        <Pagination
          v-bind="logs.meta"
          @change="changePage"
        />
      </template>
    </AsyncLoader>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { PlcState } from "../../entities";
import { useIotHttp } from "../../services/iot-http-client";

export default defineComponent({
  name: "StateErrors",

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const page = ref(1);
    const pageSize = ref(15);
    const states = ref<{ [key: string]: PlcState }>({});
    const logs = ref({
      meta: {
        page: 1,
        pageSize: 1,
        total: 1,
      },
      data: []
    });

    async function getStates() {
      const result = await http.dataArray("/plcs/states/all", {
        plc_id: props.plcId
      });

      const tmp = {} as any;
      result.forEach((pusher: PlcState) => {
        tmp[pusher.id] = pusher;
      });

      states.value = tmp;
    }

    async function getLogs() {
      const result = await http.post("/plcs/state-logs/paginate", {
        plc_id: props.plcId,
        page: page.value,
        page_size: pageSize.value
      });

      logs.value = result;
    }

    async function changePage(pg: number) {
      page.value = pg;
      await getLogs();
    }

    return {
      page,
      pageSize,
      states,
      logs,
      changePage,
      getStates,
      getLogs,
    };
  }
});
</script>
