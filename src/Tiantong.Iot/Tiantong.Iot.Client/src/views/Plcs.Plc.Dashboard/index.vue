<template>
  <div style="padding: 1.25rem">
    <div class="is-flex is-vcentered">
      <PlcRunningButton
        :plcId="plcId"
        :isRunning="isRunning"
        @change="handleRunningChange"
      />

      <span style="width: 0.5rem"></span>

      <Checkbox
        v-if="isRunning"
        v-model:value="isDataWatchOpen"
      >
        实时监控
      </Checkbox>
    </div>

    <div style="height: 1.25rem"></div>

      <template v-if="currentTab == 0">
        <div class="columns">
          <div class="column">
            <PlcStates
              :plc="plc"
              :plcId="plcId"
              :isRunning="isRunning"
              :isDataWatchOpen="isDataWatchOpen"
            />
          </div>

          <div class="column">
            <PlcLogs
              :plcId="plcId"
              :isRunning="isRunning"
            />
          </div>
        </div>
      </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from "vue";
import { useIotHttp } from "../../services/iot-http-client";
import PlcRunningButton from "./PlcRunningButton.vue";
import PlcLogs from "./PlcLogs.vue";
import PlcStates from "./PlcStates.vue";

export default defineComponent({
  name: "PlcDashboard",

  components: {
    PlcLogs,
    PlcStates,
    PlcRunningButton,
  },

  props: {
    // @refact remove plc
    plc: {
      type: Object,
      required: true
    },
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const isRunning = ref(false);
    const isDataWatchOpen = ref(false);
    const currentTab = ref(0);
    const tabs = reactive(["运行状态", "随机读写"]);
    const handleRunningChange = (_isRunning: boolean) => {
      if (_isRunning === false) {
        isDataWatchOpen.value = false;
      } else {
        isDataWatchOpen.value = true;
      }

      isRunning.value = _isRunning;
    };

    const getDataSource = async () => {
      const result = await http.post("/plc-workers/is-running", {
        plc_id: props.plcId
      });

      handleRunningChange(result.is_running);
    };

    return {
      isRunning,
      isDataWatchOpen,
      currentTab,
      tabs,
      getDataSource,
      handleRunningChange
    };
  },

  created () {
    this.getDataSource();
  }
});
</script>
