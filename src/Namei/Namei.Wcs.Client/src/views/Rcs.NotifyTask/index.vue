<template>
  <div>
    <div class="box" style="margin: 1.25rem">
      <p class="title is-4">
        任务通知
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          设备编号
        </label>

        <Input
          v-model:value="params.doorId"
          style="max-width: 400px"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          UUID
        </label>

        <Input
          v-model:value="params.uuid"
          style="max-width: 400px"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
        </label>

        <AsyncButton
          class="button is-info"
          :handler="handleSubmit"
        >
          提交
        </AsyncButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";

export default defineComponent({
  name: "Rcs.NotifyTask",

  setup() {
    const http = useWcsHttp();
    const params = ref({
      doorId: "",
      uuid: "",
    });

    async function handleSubmit() {
      await http.post("/rcs/doors/notify", params.value);
    }

    return {
      params,
      handleSubmit
    };
  }
});
</script>
