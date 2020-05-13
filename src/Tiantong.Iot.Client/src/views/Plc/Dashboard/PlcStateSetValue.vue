<template>
  <td v-if="!isShow" style="width: 200px;">
    <a @click="isShow = true">
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
          :value="value"
          @blur="setValue($event.target.value.trim())"
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
      this.value = value.toString()
    } else if (this.type === 'uint16') {
      this.value = parseInt(value)
      if (this.value === NaN) {
        this.value = 0
      }
    }
  }

  async handleSave () {
    await axios.post(`/plcs/workers/states/${this.type}/set`, {
      plc_id: this.plcId,
      state_id: this.stateId,
      value: this.value
    })

    this.isShow = false
  }
}
</script>
