<template>
  <div class="box">
    <Table
      colspan="2"
      class="table is-fullwidth"
    >
      <template #head>
        <thead>
          <th>日志</th>
          <th>时间</th>
        </thead>
      </template>

      <template #body>
        <tbody v-if="logs.data.length > 0">
          <tr v-for="log in logs.data" :key="log.id">
            <td>{{log.message}}</td>
            <td>{{log.created_at.split('.')[0].split('T')[1]}}</td>
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
import { defineComponent, ref } from "vue";
import { useService } from "@midos/vue-ui";
import { IotHttpClient } from "../../services/iot-http-client";

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
    const http = useService(IotHttpClient);
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

    return {
      page,
      pageSize,
      logs,
      interval,
      getLogs,
      changePage
    };
  },

  created () {
    this.getLogs();
    this.interval = setInterval(() => {
      if (this.isRunning) {
        setTimeout(this.getLogs, 1000);
      }
    }, 1000);
  },

  beforeUnmount () {
    clearInterval(this.interval);
  },

  watch: {
    isRunning() {
      this.getLogs();
    }
  }
});
</script>
