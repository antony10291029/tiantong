<template>
  <div>
    <a
      @click="isShow = true"
      class="button is-info"
    >
      添加
    </a>
    <div
      v-if="isShow"
      class="modal is-active"
    >
      <div
        @click="isShow = false"
        class="modal-background"
      />
      <div class="modal-card" style="width: 560px">
        <header class="modal-card-head">
          <p class="modal-card-title">
            创建任务类型
          </p>
        </header>

        <section class="modal-card-body">
          <TheForm :params="params" />
        </section>

        <footer class="modal-card-foot">
          <AsyncButton
            :handler="handleSubmit"
            class="button is-success"
          >
            提交
          </AsyncButton>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, useContext } from "vue";
import { useWcsHttp } from "../../services/wcs-http";
import TheForm from "./TheForm.vue";

export default defineComponent({
  components: {
    TheForm
  },

  setup() {
    const { emit } = useContext();
    const http = useWcsHttp();
    const isShow = ref(false);
    const params = ref({
      key: "",
      name: "",
      method: "",
      webhook: "",
    });

    async function handleSubmit() {
      await http.post("/agc-task-types/create", params.value);
      emit("refresh");
      isShow.value = false;
      params.value = {
        key: "",
        name: "",
        method: "",
        webhook: "",
      };
    }

    return {
      isShow,
      params,
      handleSubmit
    };
  }
});
</script>
