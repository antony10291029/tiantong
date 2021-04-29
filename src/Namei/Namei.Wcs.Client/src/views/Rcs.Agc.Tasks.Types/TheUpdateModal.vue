<template>
  <div>
    <a @click="isShow = true">
      编辑
    </a>
    <a
      @click="handleDelete"
      style="margin-left: 0.25rem"
    >
      删除
    </a>
    <div
      v-if="isShow"
      class="modal is-active"
      style="text-align: left"
    >
      <div
        @click="isShow = false"
        class="modal-background"
      />
      <div class="modal-card" style="width: 560px">
        <header class="modal-card-head">
          <p class="modal-card-title">
            任务类型
          </p>
        </header>

        <section class="modal-card-body">
          <TheForm :params="data" />
        </section>

        <footer class="modal-card-foot">
          <AsyncButton
            :handler="handleSubmit"
            class="button is-success"
            :disabled="!isChanged"
          >
            提交
          </AsyncButton>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, toRefs } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";
import { useCopy } from "../../hooks/use-copy";
import TheForm from "./TheForm.vue";

interface Params {
  id: number;
  key: string;
  name: string;
  method: string;
  webhook: string;
}

export default defineComponent({
  components: {
    TheForm
  },

  props: {
    params: {
      type: Object as PropType<Params>,
      required: true
    }
  },

  setup(props, { emit }) {
    const confirm = useConfirm();
    const http = useWcsHttp();
    const isShow = ref(false);
    const { data, isChanged } = useCopy<any>(toRefs(props).params as any);

    function handleDelete() {
      confirm.open({
        title: "提示",
        content: "确认后将删除该任务",
        handler: async () => {
          await http.post("/agc-task-types/delete", { id: props.params.id });
          emit("refresh");
        }
      });
    }

    async function handleSubmit() {
      await http.post("/agc-task-types/update", data.value);
      isShow.value = false;
      emit("refresh");
    }

    return {
      isShow,
      data,
      isChanged,
      handleDelete,
      handleSubmit,
    };
  }
});
</script>
