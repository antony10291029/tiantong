<template>
  <section style="padding: 1.25rem">
    <div class="box" style="height: 100%">
      <div class="columns" style="height: 100%;">
        <div class="column is-narrow">
          <div style="min-width: 400px">
            <p class="title is-4">
              PLC 调试
            </p>

            <hr>

            <div
              class="is-flex is-vcentered"
              style="padding: 0.75rem 0"
            >
              <label class="label" style="width: 100px">
                操作
              </label>

              <div
                class="field has-addons"
                style="width: 320px"
              >
                <div class="control">
                  <a
                    v-class:is-focused="params.operation == 'get'"
                    @click="params.operation = 'get'"
                    class="button is-small"
                  >
                    数据读取
                  </a>
                </div>
                <div class="control">
                  <a
                    v-class:is-focused="params.operation == 'set'"
                    @click="params.operation = 'set'"
                    class="button is-small"
                  >
                    数据写入
                  </a>
                </div>
              </div>
            </div>

          </div>
          <hr>

          <div
            class="is-flex is-vcentered"
            style="padding: 0.75rem 0"
          >
            <label
              class="label"
              style="width: 100px; align-self: start"
            >
              数据类型
            </label>

            <PlcStateTypes
              v-model:value="params.type"
              v-model:length="params.length"
              style="width: 320px"
            />
          </div>

          <hr>

          <template v-if="params.type === 'string'">
            <div
              class="is-flex is-vcentered"
              style="padding: 0.75rem 0"
            >
              <label class="label" style="width: 100px">
                字符串长度
              </label>

              <input
                v-model.number="params.length"
                type="text" class="input"
                style="width: 320px"
              >
            </div>

            <hr>
          </template>

          <div
            class="is-flex is-vcentered"
            style="padding: 0.75rem 0"
          >
            <label class="label" style="width: 100px">
              数据点
            </label>

            <input
              type="text" class="input"
              v-model.lazy="params.address"
              style="width: 320px"
            >
          </div>

          <hr>

          <template v-if="params.operation === 'set'">
            <div
              class="is-flex is-vcentered"
              style="padding: 0.75rem 0"
            >
              <label class="label" style="width: 100px">
                写入数据
              </label>

              <input
                type="text" class="input"
                v-model.lazy="params.value"
                style="width: 320px"
              >
            </div>

            <hr>
          </template>

          <div class="is-flex" style="padding: 0.75rem 0">
            <div style="width: 100px"></div>
            <AsyncButton
              class="button is-small is-info is-light"
              :handler="handleOperation"
            >
              执行
            </AsyncButton>
          </div>
        </div>

        <div class="column" style="height: 100%">
          <textarea
            readonly
            :value="logs.join('\n')"
            class="textarea is-family-monospace"
            style="height: 100%; min-width: 400px;"
          ></textarea>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DateTime } from "@midos/vue-ui";
import { useIotHttp } from "../../services/iot-http-client";
import PlcStateTypes from "../../components/PlcStateTypes.vue";

export default defineComponent({
  name: "PlcDebug",

  components: {
    PlcStateTypes
  },

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const operation = ref("get");
    const type = ref("uint16");
    const params = ref({
      operation: "get",
      type: "uint16",
      address: "D100",
      length: 8,
      value: "",
    });
    const logs = ref<string[]>([]);
    const types = ref([
      { value: "uint16", text: "uint16" },
      { value: "int32", text: "int32" },
      { value: "string", text: "ASCII 字符串" },
    ]);

    async function handleOperation () {
      const url = `plc-workers/debug/${params.value.type}/${params.value.operation}`;

      const result = await http.post(url, {
        plc_id: props.plcId,
        address: params.value.address,
        length: params.value.length,
        value: params.value.value
      });

      const time = DateTime.now.split("T")[1].split(".")[0];

      logs.value.unshift(`[${time}] ${result.message}`);
      if (logs.value.length > 50) {
        logs.value.splice(50, logs.value.length - 50);
      }
    }

    return {
      operation,
      type,
      params,
      logs,
      types,
      handleOperation,
    };
  },

});
</script>
