<template>
  <AsyncLoader
    :handler="getIsAutorun"
    style="padding: 1.25rem"
  >
    <div class="field">
      <p class="title is-5">
        自动运行
      </p>

      <p class="content">
        开启自动运行后，能够在系统启动时运行所有设备。
      </p>

      <div class="field">
        <div class="control">
          <AsyncButton
            v-if="!isAutorun"
            :handler="() => setAutorun(true)"
            class="button is-info is-light is-small"
          >
            开启
          </AsyncButton>
          <AsyncButton
            v-else
            :handler="() => setAutorun(false)"
            class="button is-info is-light is-small"
          >
            关闭
          </AsyncButton>
        </div>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useIotHttp } from "@/services/iot-http-client";

export default defineComponent({
  name: "Autorun",

  setup() {
    const isAutorun = ref(false);
    const http = useIotHttp();

    async function getIsAutorun () {
      const response = await http.post("/autorun/get");
      isAutorun.value = response.data;
    }

    async function setAutorun (value: boolean) {
      await http.post("/autorun/set", { value });
      await getIsAutorun();
    }

    return {
      isAutorun: false,
      setAutorun,
      getIsAutorun,
    };
  },
});
</script>
