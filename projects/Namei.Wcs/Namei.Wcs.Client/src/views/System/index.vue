<template>
  <AsyncLoader
    :handler="getSystemParams"
    style="padding: 1.25rem"
  >
    <div class="box">
      <p class="title is-5">
        系统配置
      </p>

      <hr>

      <ClearLogs />

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行货梯指令
        </p>

        <Switcher
          v-model="settings.enableLifterCommands"
          @input="setSystemParams"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行提升机指令
        </p>

        <Switcher
          @input="setSystemParams"
          v-model="settings.enableHoisterCommands"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行自动门指令
        </p>

        <Switcher
          @input="setSystemParams"
          v-model="settings.enableDoorsCommands"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行 WMS 指令
        </p>

        <Switcher
          v-model="settings.enableWmsCommands"
          @input="setSystemParams"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行 RCS 指令
        </p>

        <Switcher
          v-model="settings.enableRcsCommands"
          @input="setSystemParams"
        />
      </div>
      <hr>

    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";
import ClearLogs from "./ClearLogs.vue";

export default defineComponent({
  name: "System",

  components: {
    ClearLogs
  },

  setup() {
    const http = useWcsHttp();

    const settings = ref({
      enableDoorsCommands: false,
      enableLifterCommands: false,
      enableHoisterCommands: false,
      enableWmsCommands: false,
      enableRcsCommands: false,
    });

    async function getSystemParams() {
      const result = await http.post("/test/system-settings");

      settings.value = result;
    }

    async function setSystemParams() {
      await http.post("/test/system-settings/set", settings.value);
      await getSystemParams();
    }

    return {
      settings,
      getSystemParams,
      setSystemParams
    };
  },

});
</script>
