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
          <div
            class="is-flex is-vcentered"
            style="padding: 1.25rem 0"
          >
            <label class="label" style="width: 100px">
              任务名
            </label>

            <p class="is-flex-auto">
              <Input :value="taskType.name" readonly/>
            </p>
          </div>

          <div
            class="is-flex is-vcentered"
            style="padding: 1.25rem 0"
            v-for="(value, key) in params.data" :key="key"
          >
            <label class="label" style="width: 100px">
              {{key}}
            </label>

            <p class="is-flex-auto">
              <Input v-model:value="params.data[key]" />
            </p>
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

export default defineComponent({
  name: "Create",

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
