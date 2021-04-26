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
        位置类型
      </label>

      <Input
        v-model:value="params.nextPositionCode.type"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        位置编号
      </label>

      <Input
        v-model:value="params.nextPositionCode.positionCode"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        货架编号
      </label>

      <Input
        v-model:value="params.podCode"
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
  name: "Rcs.Tasks.Continue",

  setup(_, { emit }) {
    const http = useWcsHttp();
    const params = ref({
      reqCode: "",
      agvCode: "",
      podCode: "",
      taskCode: "",
      nextPositionCode: {
        positionCode: "",
        type: "00"
      },
    });

    async function handleSubmit() {
      const result = await http.post("/rcs/tasks/continue", params.value);

      emit("log", `[${DateTime.now}]: ${JSON.stringify(result, null, 4)}`);
    }

    return {
      params,
      handleSubmit
    };
  }
});
</script>
