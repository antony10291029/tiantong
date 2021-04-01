<template>
  <div>
    <a
      class="button is-info"
      @click="isShow = true"
    >
      创建任务
    </a>
    <div
      class="modal"
      v-class:is-active="isShow"
    >
      <div
        class="modal-background"
        @click="isShow = false"
      ></div>
      <div class="modal-card" style="max-width: 480px">
        <div class="modal-card-head">
          <p class="modal-card-title">
            创建任务
          </p>
        </div>
        <div class="modal-card-body">
          <div class="field">
            <label class="label">任务名</label>
            <div class="control">
              <Input :value="taskType.key" readonly />
            </div>
          </div>

          <div class="field">
            <label class="label">数据</label>
            <div class="control">
              <TaskTypeDataEditor
                type="order"
                v-model:value="params.data"
              />
            </div>
          </div>
        </div>

        <div class="modal-card-foot">
          <AsyncButton
            class="button is-info is-small"
            :handler="handleSubmit"
          >
            提交
          </AsyncButton>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { cloneDeep } from "lodash";
import { useMidosCenterHttp } from "../../services/midos-center-http";
import TaskTypeDataEditor from "../Tas.Types.Type/TaskTypeDataEditor.vue";

export default defineComponent({
  name: "Create",

  components: {
    TaskTypeDataEditor
  },

  props: {
    taskType: {
      type: Object,
      required: true
    }
  },

  setup(props, { emit }) {
    const isShow = ref(false);
    const http = useMidosCenterHttp();
    const params = ref({
      key: props.taskType.key,
      data: cloneDeep(props.taskType.data)
    });

    async function handleSubmit() {
      await http.post("/midos/tasks/create", params.value);
      emit("created");
      isShow.value = false;
    }

    return {
      isShow,
      params,
      handleSubmit
    };
  }
});
</script>
