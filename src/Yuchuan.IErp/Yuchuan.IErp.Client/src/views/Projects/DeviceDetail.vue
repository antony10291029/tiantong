<template>
  <AsyncLoader
    class="box"
    :handler="getDevice"
  >
    <p class="label is-size-5">
      设备信息
    </p>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <div class="label mb-0" style="width: 80px">
        ID
      </div>

      <div class="is-flex-auto">
        {{device.id}}
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <div class="label mb-0" style="width: 80px">
        设备名
      </div>

      <div class="is-flex-auto">
        <input
          v-model="device.name"
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
      <div class="label mb-0" style="width: 80px">
        编号
      </div>

      <div class="is-flex-auto">
        <input
          v-model="device.number"
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
      <div class="label mb-0" style="width: 80px">
        备注
      </div>

      <div class="is-flex-auto">
        <div style="width: 640px">
          <Textarea v-model="device.comment"></Textarea>
        </div>
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 80px">开启</label>

      <div class="is-flex-auto">
        <Switcher v-model="device.is_enabled"></Switcher>
      </div>
    </div>

    <hr>

    <div class="is-flex">
      <div style="width: 80px"></div>

      <div class="is-flex-auto is-flex">
        <AsyncButton
          class="button is-info is-small"
          :handler="handleSave"
        >
          保存
        </AsyncButton>

        <span style="width: 0.5rem"></span>

        <AsyncButton
          :handler="handleDelete"
          class="button is-danger is-small is-light"
        >
          删除
        </AsyncButton>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'DeviceDetail'
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  projectId!: number

  @Prop({ required: true })
  deviceId!: number

  device: any = {}

  async getDevice () {
    const response = await domain.post('/devices/find', {
      id: this.deviceId
    })

    this.device = response.data
  }

  async handleSave () {
    const response = await domain.post('/devices/update', this.device)
    await this.getDevice()
    this.$emit('refresh')
  }

  async handleDelete () {
    this.$confirm({
      title: '删除设备',
      content: '删除后将无法恢复',
      handler: async () => {
        const response = await domain.post('/devices/delete', {
          id: this.device.id
        })

        this.$router.push(this.baseURL)
        this.$emit('refresh')
      }
    })
  }
}
</script>
