<template>
  <div>
    <div
      @click="$emit('select', dateType.value)"
      v-for="(dateType, key) in dateTypes" :key="key"
      class="is-bordered is-flex is-vcentered"
      style="padding: 0.5rem; cursor: pointer"
      :style="key !== 0 && 'border-top: none'"
    >
      <Radio :value="type === dateType.value">
        {{dateType.text}}
      </Radio>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { PlcStateType } from '@/entities'

@Component({
  name: 'PlcStateTypes',
  model: {
    prop: 'type',
    event: 'select'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  type!: string

  @Prop({ required: true })
  length!: number

  get dateTypes () {
    return [
      {
        value: PlcStateType.uint16,
        text: '整数（16 位 - 无符号）'
      },
      {
        value: PlcStateType.int32,
        text: '整数（32 位）'
      },
      {
        value: PlcStateType.asciiString,
        text: `字符串（${this.length * 2} 位 - ASCII）`
      }
    ]
  }
}
</script>
