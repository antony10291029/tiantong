<template>
  <div>
    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        任务编号
      </label>

      <Input
        v-model:value="params.taskCode"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        AGV 编号
      </label>

      <Input
        v-model:value="params.agvCode"
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
        :handler="handleSubmit"
        class="button is-info is-small"
      >
        提交
      </AsyncButton>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DateTime } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";

export default defineComponent({
  name: "CancelTask",

  setup(_, { emit }) {
    const http = useWcsHttp();
    const params = ref({
      taskCode: "",
      agvCode: "",
    });

    async function handleSubmit() {
      const result = await http.post("/rcs/tasks/cancel", params.value);

      emit("log", `[${DateTime.now}]: ${JSON.stringify(result, null, 4)}`);
    }

    return {
      params,
      handleSubmit
    };
  }
});
</script>
