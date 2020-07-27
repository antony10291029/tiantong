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
          <input
            v-model="plc.name"
            type="text" class="input"
            style="width: 320px"
          >
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
          <input
            v-model="plc.number"
            type="text" class="input"
            style="width: 320px"
          >
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
            <input
              v-model="plc.host"
              type="text" class="input"
              style="width: 320px"
            >
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
              v-model="plc.port"
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
            <Textarea v-model="plc.comment"/>
          </div>
        </div>

      <slot name="footer" />
    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Plc, PlcModel } from '@/entities'

@Component({
  name: 'PlcForm'
})
export default class extends Vue {
  @Prop({ required: true })
  plc!: Plc

  @Prop({ default: () => () => {} })
  handler!: () => {}

  models = [
    {
      value: PlcModel.mc3eBinary,
      text: '三菱 MC - TCP（二进制）',
    },
    {
      value: PlcModel.s7200Smart,
      text: '西门子 200Smart',
    },
    {
      value: PlcModel.test,
      text: '测试协议',
    }
  ]
}
</script>
