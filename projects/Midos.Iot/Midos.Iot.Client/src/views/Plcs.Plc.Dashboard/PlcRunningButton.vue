<template>
  <AsyncButton
    v-if="!isRunning"
    :handler="handleRun"
    class="button is-small is-info"
  >
    <span>启动</span>
    <span class="icon is-small">
      <i class="iconfont iot-play"></i>
    </span>
  </AsyncButton>
  <AsyncButton
    v-else
    :handler="handleStop"
    class="button is-small is-info"
  >
    <span>停止</span>
    <span class="icon is-small">
      <i class="iconfont iot-stop"></i>
    </span>
  </AsyncButton>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useIotHttp } from "../../services/iot-http-client";

export default defineComponent({
  name: "PlcRunningButton",

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

  setup(props, { emit }) {
    const http = useIotHttp();

    async function handleRun () {
      await http.post("/plc-workers/run", {
        plc_id: props.plcId
      });

      emit("change", true);
    }

    async function handleStop () {
      await http.post("/plc-workers/stop", {
        plc_id: props.plcId
      });

      emit("change", false);
    }

    return {
      handleRun,
      handleStop,
    };
  },
});
</script>
