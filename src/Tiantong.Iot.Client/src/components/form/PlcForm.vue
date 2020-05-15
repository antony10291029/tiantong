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
          @click="plc.model = 'mc3e'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; cursor: pointer"
        >
          <Radio :value="plc.model === 'mc3e'">
            MC-3E
          </Radio>
        </div>
        <div
          @click="plc.model = 's7200smart'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; border-top: none; cursor: pointer"
        >
          <Radio :value="plc.model === 's7200smart'">
            S7-200Smart
          </Radio>
        </div>
        <div
          @click="plc.model = 'test'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; border-top: none; cursor: pointer"
        >
          <Radio :value="plc.model === 'test'">
            Test
          </Radio>
        </div>
      </div>
      <div class="field" style="width: 320px">
        <label class="label">地址</label>
        <div class="control">
          <input
            v-model="plc.host"
            type="text" class="input"
          >
        </div>
      </div>
      <div class="field" style="width: 320px">
        <label class="label">端口</label>
        <div class="control">
          <input
            v-model="plc.port"
            type="text" class="input"
          >
        </div>
      </div>
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
import { Plc } from '@/entities'

@Component({
  name: 'PlcForm'
})
export default class extends Vue {
  @Prop({ required: true })
  plc!: Plc

  @Prop({ default: () => () => {} })
  handler!: () => {}
}
</script>
