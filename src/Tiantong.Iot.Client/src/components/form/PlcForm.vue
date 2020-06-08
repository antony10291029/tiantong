<template>
  <AsyncLoader
    :handler="handler"
    #default="{ isPending }"
    style="padding: 1.25rem"
  >
    <template v-if="!isPending">
      <slot name="header" />

      <div class="field" style="width: 320px">
        <label class="label">设备名称</label>
        <div class="control">
          <input
            v-model="plc.name"
            type="text" class="input"
          >
        </div>
      </div>
      <div class="field" style="width: 320px">
        <label class="label">通信协议</label>
        <div
          v-for="(model, key) in models" :key="key"
          @click="plc.model = model.value"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; cursor: pointer"
          :style="key !== 0 && 'border-top: none'"
        >
          <Radio :value="plc.model === model.value">
            {{model.text}}
          </Radio>
        </div>
      </div>

      <template v-if="plc.model !== 'test'">
        <div class="field" style="width: 320px">
          <label class="label">IP 地址</label>
          <div class="control">
            <input
              v-model="plc.host"
              type="text" class="input"
            >
          </div>
        </div>
        <div class="field" style="width: 320px">
          <label class="label">IP 端口</label>
          <div class="control">
            <Input
              v-model="plc.port"
              type="number" class="input"
            />
          </div>
        </div>
      </template>
      <div class="field" style="width: 480px">
        <label class="label">备注</label>
        <div class="control">
          <Textarea v-model="plc.comment" />
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
