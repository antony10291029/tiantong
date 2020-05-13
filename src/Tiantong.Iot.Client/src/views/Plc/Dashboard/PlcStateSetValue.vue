<template>
  <td v-if="!isShow" style="width: 200px;">
    <a @click="handleOpen">
      写入数据
    </a>
  </td>
  <td v-else style="padding: 0; width: 200px">
    <div
      class="background"
      @click="isShow = false"
    ></div>
    <div
      class="field has-addons"
      style="position: absolute; z-index: 99999"
    >
      <div class="control">
        <input
          ref="input"
          :value="value"
          @blur="setValue($event.target.value.trim())"
          @keypress.enter="setValue($event.target.value.trim()) && handleSave()"
          type="text" class="input"
          style="height: 100%; border-radius: 0; width: 200px"
        >
      </div>
      <div class="control">
        <AsyncButton
          :handler="handleSave"
          class="button is-success"
        >写入</AsyncButton>
      </div>
    </div>
  </td>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcStateSetValue'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  stateId!: number

  @Prop({ required: true })
  type!: string

  isShow = false

  value: any = ''

  setValue (value: any) {
    if (this.type == 'string') {
      value = value.toString()
    } else if (this.type) {
      value = parseInt(value)
      if (Number.isNaN(value)) {
        value = 0
      }
    }

    this.value = value
  }

  handleOpen () {
    this.isShow = true
    this.$nextTick(() => {
      (this.$refs.input as any).focus()
      this.value = ''
    })
  }

  async handleSave () {
    await axios.post(`/plc-workers/states/${this.type}/set-by-id`, {
      plc_id: this.plcId,
      state_id: this.stateId,
      value: this.value
    })

    this.isShow = false
  }
}
</script>
