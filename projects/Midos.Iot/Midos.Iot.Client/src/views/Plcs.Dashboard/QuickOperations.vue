<template>
  <div style="padding: 1.25rem">
    <p class="title is-5">
      设备控制
    </p>

    <p class="content">
      该操作将一键运行或停止当前系统中的所有设备
    </p>

    <AsyncButton
      :handler="run"
      style="margin-right: 0.5rem"
      class="button is-info is-light is-small"
    >
      运行
    </AsyncButton>

    <AsyncButton
      :handler="stop"
      class="button is-info is-light is-small"
    >
      停止
    </AsyncButton>
  </div>
</template>

<script lang="ts">
import { useIotHttp } from "@/services/iot-http-client";
import { defineComponent } from "vue";

export default defineComponent({
  name: "StopDevices",

  setup() {
    const http = useIotHttp();

    async function run() {
      await http.post("/plc-workers/start-all");
    }

    async function stop () {
      await http.post("/plc-workers/stop-all");
    }

    return {
      run,
      stop,
    };
  }

});
</script>
