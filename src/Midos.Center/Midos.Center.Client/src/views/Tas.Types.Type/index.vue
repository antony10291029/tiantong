<template>
  <div>
    <div class="box">
      <p class="title is-5">
        详细信息
      </p>

      <hr>

      <TaskTypeForm
        :taskType="data"
        :taskTypes="$attrs.taskTypes"
      >
        <template #foot>
          <AsyncButton
            :disabled="!isChanged"
            class="button is-info is-small"
            :handler="handleSubmit"
          >
            更新
          </AsyncButton>

          <div class="is-flex-auto"></div>

          <AsyncButton
            :handler="handleDelete"
            class="button is-danger is-small is-light"
          >
            删除
          </AsyncButton>
        </template>
      </TaskTypeForm>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, Ref, toRefs } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useCopy } from "../../hooks/use-copy";
import { useMidosCenterHttp } from "../../services/midos-center-http";
import TaskTypeForm from "./Form.vue";

export default defineComponent({
  name: "Tas.Types.Type",

  components: {
    TaskTypeForm
  },

  props: {
    taskType: {
      type: Object as PropType<any>,
      required: true
    }
  },

  setup(props, { emit }) {
    const confirm = useConfirm();
    const http = useMidosCenterHttp();
    const { data, isChanged } = useCopy(toRefs(props).taskType as Ref<any>);

    async function handleSubmit() {
      await http.post("/midos/tas/types/update", data.value);

      emit("refresh", data.value.id);
    }

    function handleDelete() {
      confirm.open({
        title: "删除",
        content: "确认删除后，作业单类型将无法恢复",
        handler: async () => {
          await http.post("/midos/tas/types/delete", {
            id: data.value.id
          });

          emit("refresh", 0);
        }
      });
    }

    return {
      data,
      isChanged,
      handleSubmit,
      handleDelete,
    };
  }
});
</script>
