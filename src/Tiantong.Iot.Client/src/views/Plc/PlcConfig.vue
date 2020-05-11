<template>
  <AsyncLoader
    style="padding: 1.25rem"
    :handler="getPlc"
    #default="{ isPending }"
  >
    <template v-if="!isPending">
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
        <label class="label">设备型号</label>
        <div class="control">
          <input
            v-model="plc.model"
            type="text" class="input"
          >
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

      <div class="field" style="width: 480px">
        <div class="control is-flex">
          <AsyncButton
            :handler="handleSave"
            class="button is-info is-small"
            style="margin-right: 0.5rem"
          >
            保存
          </AsyncButton>

          <a
            class="button is-success is-small"
            style="margin-right: 0.5rem"
          >
            连接测试
          </a>

          <span class="is-flex-auto"></span>

          <a
            class="button is-danger is-light is-small"
          >
            删除
          </a>
        </div>
      </div>
    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcDashboard'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  plc: any

  async getPlc () {
    let response = await axios.post('/plcs/find', {
      plc_id: this.plcId
    })

    this.plc = response.data
  }

  async handleSave () {
    await axios.post('/plcs/update', this.plc)
  }
}
</script>
