<template>
  <tr @click="isShow = true">
    <teleport to="body">
      <div
        v-if="isShow"
        class="modal is-active"
      >
        <div class="modal-background"/>

        <div class="modal-card" style="overflow: auto">
          <div class="modal-card-head">
            <p class="modal-card-title">
              详情
            </p>
          </div>

          <div class="modal-card-body">
            <div class="field">
              <p class="label">
                请求
              </p>
              <div class="control">
                <pre>{{parseJsonData(httpLog.requestBody)}}</pre>
              </div>
            </div>

            <div class="field">
              <p class="label">
                响应
              </p>
              <div class="control">
                <pre>{{parseJsonData(httpLog.responseBody)}}</pre>
              </div>
            </div>

            <div class="field">
              <p class="label">
                请求头
              </p>
              <div class="control">
                <pre>{{parseJsonData(httpLog.requestHeaders)}}</pre>
              </div>
            </div>

            <div class="field">
              <p class="label">
                响应头
              </p>
              <div class="control">
                <pre>{{parseJsonData(httpLog.responseHeaders)}}</pre>
              </div>
            </div>

            <div class="field">
              <p class="label">
                异常
              </p>
              <div class="control">
                <pre>{{parseJsonData(httpLog.exception)}}</pre>
              </div>
            </div>
          </div>

          <div class="modal-card-foot">
            <a
              @click="isShow = false"
              class="button"
            >取消</a>
          </div>
        </div>
      </div>
    </teleport>
    <slot />
  </tr>
</template>

<script lang="ts">
import { defineComponent, ref, PropType } from "vue";
import { HttpLog } from "../../domain";

export default defineComponent({
  props: {
    httpLog: {
      type: Object as PropType<HttpLog>,
      required: true
    }
  },

  setup() {
    const isShow = ref(false);

    function parseJsonData(text: any) {
      try {
        if ((typeof text) === "string") {
          text = JSON.parse(text);
        }

        return JSON.stringify(text, null, 2);
      } catch {
        return text;
      }
    }

    return {
      isShow,
      parseJsonData
    };
  }
});
</script>
