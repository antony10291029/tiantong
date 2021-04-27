<template>
  <div>
    <a
      class="button is-info"
      @click="isShow = true"
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
      ></div>

      <div class="modal-card" style="width: 480px">
        <header class="modal-card-head">
          <p class="modal-card-title">
            创建绑定记录
          </p>
        </header>

        <section class="modal-card-body">
          <div class="field">
            <label class="label">
              作业单号
            </label>

            <div class="control">
              <Input v-model:value="params.orderCode" />
            </div>
          </div>

          <div class="field">
            <label class="label">
              箱码
            </label>

            <div class="control">
              <Input v-model:value="params.itemCode" />
            </div>
          </div>
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
import { useRcsExtHttp } from "../../services/rcs-ext-http";

export default defineComponent({
  setup() {
    const { emit } = useContext();
    const http = useRcsExtHttp();
    const isShow = ref(false);
    const params = ref({
      orderCode: "",
      itemCode: ""
    });

    async function handleSubmit() {
      await http.post(
        "/wms/move-doc-boxes/bind",
        params.value
      );

      params.value.orderCode = "";
      params.value.itemCode = "";
      isShow.value = false;
      emit("refresh");
    }

    return {
      isShow,
      params,
      handleSubmit
    };
  }
});
</script>
