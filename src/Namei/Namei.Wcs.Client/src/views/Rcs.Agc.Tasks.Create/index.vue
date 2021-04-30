<template>
  <div class="modal is-active">
    <div
      class="modal-background"
      @click="handleClose"
    />
    <div class="modal-card" style="width: 520px">
      <div class="modal-card-head">
        <div class="modal-card-title">
          创建 AGC 任务
        </div>
      </div>

      <div class="modal-card-body">
        <TheTaskType v-model:value="params.type" />

        <div class="field">
          <label class="label">起点</label>
          <div class="control">
            <Input v-model:value="params.position" />
          </div>
        </div>

        <div class="field">
          <label class="label">终点</label>
          <div class="control">
            <Input v-model:value="params.destination" />
          </div>
        </div>

        <div class="field">
          <label class="label">托盘码</label>
          <div class="control">
            <Input v-model:value="params.podCode" />
          </div>
        </div>
      </div>

      <div class="modal-card-foot">
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
import { useRouter } from "vue-router";
import { useWcsHttp } from "../../services/wcs-http";
import TheTaskType from "./TheTaskType.vue";

export default defineComponent({
  name: "RcsAgcCreate",

  components: {
    TheTaskType
  },

  setup(_, { emit }) {
    const http = useWcsHttp();
    const router = useRouter();
    const params = ref({
      type: "",
      position: "",
      destination: "",
      podCode: "",
      priority: ""
    });

    function handleClose() {
      router.push({
        name: "NameiWcsRcsAgcTasks"
      });
    }

    async function handleSubmit() {
      await http.post("/rcs/agv-tasks/create", params.value);

      handleClose();
      emit("refresh");
    }

    return {
      params,
      handleSubmit,
      handleClose
    };
  }
});
</script>
