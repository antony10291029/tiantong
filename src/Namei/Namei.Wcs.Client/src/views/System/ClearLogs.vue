<template>
  <p class="title is-5">
    日志清理
  </p>

  <p class="content">
    日志数据量过大将影响系统性能，若有需要可根据以下条件清理进行清理。
  </p>

  <div class="is-flex">
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
  </div>

  <div style="height: 1.25rem"></div>

  <AsyncButton
    :handler="handleClick"
    class="button is-info is-small is-light"
  >
    开始清理
  </AsyncButton>
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
