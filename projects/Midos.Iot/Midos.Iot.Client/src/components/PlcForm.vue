<template>
  <AsyncLoader
    :handler="handler"
    #default="{ isPending }"
    style="padding: 1.25rem"
  >
    <template v-if="!isPending">
      <slot name="header" />

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          名称
        </label>

        <div class="is-flex-auto">
          <Input
            v-model:value="plc.name"
            type="text"
            style="width: 320px"
          />
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          编号
        </label>

        <div class="is-flex-auto">
          <Input
            v-model:value="plc.number"
            type="text"
            style="width: 320px"
          />
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
          通信协议
        </label>
        <div class="is-flex-auto">
          <div
            v-for="(model, key) in models" :key="key"
            @click="plc.model = model.value"
            class="is-bordered is-flex is-vcentered"
            style="padding: 0.5rem; cursor: pointer; width: 320px;"
            :style="key !== 0 && 'border-top: none'"
          >
            <Radio :value="plc.model === model.value">
              {{model.text}}
            </Radio>
          </div>
        </div>
      </div>

      <hr>

      <template v-if="plc.model !== 'test'">
        <div
          class="is-flex is-vcentered"
          style="padding: 0.75rem 0"
        >
          <label class="label" style="width: 100px">
            IP 地址
          </label>

          <div class="is-flex-auto">
            <Input
              v-model:value="plc.host"
              type="text"
              style="width: 320px"
            />
          </div>
        </div>

        <hr>

        <div
          class="is-flex is-vcentered"
          style="padding: 0.75rem 0"
        >
          <label class="label" style="width: 100px">
            IP 端口
          </label>

          <div class="is-flex-auto">
            <Input
              v-model:value="plc.port"
              type="number" class="input"
              style="width: 320px"
            />
          </div>
        </div>

        <hr>
      </template>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label
          class="label"
          style="width: 100px; align-self: start"
        >
          备注
        </label>

        <div style="width: 480px;">
          <Textarea
            v-model:value="plc.comment"
          />
        </div>
      </div>
      <slot name="footer" />
    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { Plc, PlcModel } from "@/entities";

export default defineComponent({
  name: "PlcForm",

  props: {
    plc: {
      type: Object as PropType<Plc>,
    },
    handler: {
      type: Function,
      default: () => () => {}
    }
  },

  setup() {
    return {
      models: [
        {
          value: PlcModel.mc3eBinary,
          text: "三菱 MC3E - TCP（二进制）",
        },
        {
          value: PlcModel.mc1eBinary,
          text: "三菱 MC1E - TCP（二进制）",
        },
        {
          value: PlcModel.s7200Smart,
          text: "西门子 200Smart",
        },
        {
          value: PlcModel.test,
          text: "测试协议",
        }
      ]
    };
  }
});
</script>
