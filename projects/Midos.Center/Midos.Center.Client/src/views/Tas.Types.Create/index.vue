<template>
  <div>
    <div class="box">
      <p class="title is-size-4">
        创建任务类型
      </p>

      <hr>

      <TaskTypeForm
        :taskType="params"
        :taskTypes="$attrs.taskTypes"
      >
        <template #foot>
          <AsyncButton
            class="button is-info is-small"
            :handler="handleSubmit"
          >
            提交
          </AsyncButton>
        </template>
      </TaskTypeForm>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import TaskTypeForm from "../Tas.Types.Type/Form.vue";
import { useMidosCenterHttp } from "../../services/midos-center-http";

export default defineComponent({
  name: "Tas.Types.Create",

  components: {
    TaskTypeForm
  },

  setup(props, { emit }) {
    const http = useMidosCenterHttp();

    const params = ref({
      id: 0,
      key: "",
      name: "",
      data: "{}",
      comment: "",
      subtypes: []
    });

    async function handleSubmit() {
      const result = await http.post("/midos/tas/types/create", params.value);

      emit("refresh", result.id);
    }

    return {
      params,
      handleSubmit
    };
  },
});
</script>
