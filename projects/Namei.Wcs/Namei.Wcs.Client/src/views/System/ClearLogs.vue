<template>
  <div
    class="is-flex is-vcentered"
    style="padding: 1.25rem 0"
  >
    <p class="label" style="width: 180px">
      清理日志
    </p>

    <label
      class="label"
      @click="days = 30"
    >
      <Radio :value="days === 30">
        保留 30 天
      </Radio>
    </label>

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 15"
    >
      <Radio :value="days === 15">
        保留 15 天
      </Radio>
    </label>

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 7"
    >
      <Radio :value="days === 7">
        保留 7 天
      </Radio>
    </label>

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 0"
    >
      <Radio :value="days === 0">
        全部清理
      </Radio>
    </label>

    <div style="width: 1.5rem"></div>

    <AsyncButton
      :handler="handleClick"
      class="button is-info"
    >
      开始清理
    </AsyncButton>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";

export default defineComponent({
  name: "ClearLogs",

  setup() {
    const http = useWcsHttp();
    const confirm = useConfirm();
    const days = ref(30);

    function handleClick () {
      confirm.open({
        title: "确认",
        content: "清除日志后将无法找回",
        handler: () => http.post("/logs/clear", { days: days.value })
      });
    }

    return {
      days,
      handleClick,
    };
  }
});
</script>
