<template>
  <div class="box">
    <Table
      colspan="2"
      class="table is-fullwidth is-nowrap"
    >
      <template #head>
        <thead>
          <th>时间</th>
          <th>日志</th>
        </thead>
      </template>

      <template #body>
        <tbody v-if="logs.data.length > 0">
          <tr v-for="log in logs.data" :key="log.id">
            <td>{{getTime(log.created_at)}}</td>
            <td>{{log.message}}</td>
          </tr>
        </tbody>
      </template>
    </Table>

    <div style="height: 0.75rem"></div>

    <Pagination
      v-bind="logs.meta"
      @change="changePage"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from "vue";
import { useIotHttp } from "../../services/iot-http-client";
import { useInterval } from "../../hooks/use-interval";

export default defineComponent({
  name: "PlcLogs",

  props: {
    plcId: {
      type: Number,
      required: true
    },

    isRunning: {
      type: Boolean,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const interval = ref(null as any);
    const logs = ref({
      meta: {
        page: 0,
        pageSize: 10,
        total: 0
      },
      data: [] as any[]
    });
    const page = ref(1);
    const pageSize = ref(15);

    const getLogs = async () => {
      const data = await http.paginateArray("/plc-logs/paginate", {
        plc_id: props.plcId,
        page: page.value,
        pageSize: pageSize.value,
      });

      logs.value = data;
    };

    const changePage = async (_page: number) => {
      page.value = _page;
      await getLogs();
    };

    const getTime = (source: string) => {
      const dateTime = source.split(".")[0];
      const date = dateTime.split("T")[0].split("-").slice(1).join("-");
      const time = dateTime.split("T")[1];

      return `${date} ${time}`;
    };

    getLogs();
    useInterval(getLogs, toRefs(props).isRunning);

    return {
      page,
      pageSize,
      logs,
      interval,
      getLogs,
      getTime,
      changePage
    };
  }
});
</script>
