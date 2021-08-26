<template>
  <tr @click="isShow = true">
    <teleport to="body">
      <div
        v-if="isShow"
        class="modal is-active"
      >
        <div class="modal-background"/>

        <div class="modal-card" style="width: calc(100% - 100px)">
          <div class="modal-card-head">
            <p class="modal-card-title">
              详情
            </p>
          </div>

          <div class="modal-card-body" style="overflow: auto">
            <div class="field">
              <p class="label">
                Query
              </p>

              <div class="control">
                <pre>{{httpLog.requestQuery}}</pre>
              </div>
            </div>

            <div class="field">
              <p class="label">
                请求
              </p>
              <div class="control">
                <DataParser :value="httpLog.requestBody" />
              </div>
            </div>

            <div class="field">
              <p class="label">
                响应
              </p>
              <div class="control">
                <DataParser :value="httpLog.responseBody" />
              </div>
            </div>

            <div class="field">
              <p class="label">
                请求头
              </p>
              <div class="control">
                <DataParser :value="httpLog.requestHeaders" />
              </div>
            </div>

            <div class="field">
              <p class="label">
                响应头
              </p>
              <div class="control">
                <DataParser :value="httpLog.responseHeaders" />
              </div>
            </div>

            <div class="field">
              <p class="label">
                异常
              </p>
              <div class="control">
                <DataParser :value="httpLog.exception" />
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
import DataParser from "./DataParser.vue";

export default defineComponent({
  components: {
    DataParser
  },

  props: {
    httpLog: {
      type: Object as PropType<HttpLog>,
      required: true
    }
  },

  setup() {
    const isShow = ref(false);

    return {
      isShow
    };
  }
});
</script>
