<template>
  <AsyncLoader
    :handler="getPushers"
    #default="{ isPushersPending }"
  >
    <AsyncLoader
      v-if="!isPushersPending"
      :handler="getLogs"
      #default="{ isPending }"
    >
      <template v-if="!isPending">
        <Table
          colspan="4"
          class="table is-centered is-bordered is-fullwidth"
        >
          <template #head>
            <thead>
              <th>推送名</th>
              <th>URL</th>
              <th>详情</th>
              <th>时间</th>
            </thead>
          </template>
          <template #body>
            <tbody v-if="logs.data.length > 0">
              <tr v-for="log in logs.data" :key="log.id">
                <td>{{pushers[log.pusher_id].name}}</td>
                <td>{{pushers[log.pusher_id].url}}</td>
                <td>{{log.message}}</td>
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
import { computed, defineComponent, ref } from "vue";
import { HttpPusher } from "../../entities";
import { useIotHttp } from "../../services/iot-http-client";

export default defineComponent({
  name: "HttpPusherErrors",

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
    const pushers = ref<{ [key: string]: HttpPusher }>({});
    const logs = ref({
      meta: {
        page: 1,
        pageSize: 1,
        total: 1,
      },
      data: []
    });
    const pusherIds = computed(() => Object.values(pushers.value).map(pusher => pusher.id));

    async function getPushers() {
      const result = await http.dataArray("/plcs/http-pushers/all", {
        plc_id: props.plcId
      });

      const tmp = {} as any;
      result.forEach((pusher: HttpPusher) => {
        tmp[pusher.id] = pusher;
      });

      pushers.value = tmp;
    }

    async function getLogs() {
      const result = await http.post("/plcs/http-pusher-logs/paginate", {
        ids: pusherIds.value,
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
      pushers,
      logs,
      changePage,
      getPushers,
      getLogs,
    };
  }
});
</script>
