<template>
  <td
    v-if="tag === 'input'"
    class="input"
    v-text="valueDate"
    @click="handleClick"
    style="cursor: pointer"
  />
  <td
    v-else
    v-text="valueDate"
    class="is-clickable"
    @click="handleClick"
  />
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import Flatpickr from 'flatpickr'
import zh from 'flatpickr/dist/l10n/zh.js'
import { DateTime } from '@/utils/common'

@Component({
  name: 'DatePicker',
  model: {
    prop: 'value',
    event: 'input'
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  value!: string

  // 默认值为 DateTime.minValue
  // 将空字符串 '' DateTime.minValue
  @Prop({ default: true })
  clearable!: boolean

  @Prop({ default: false })
  readonly!: Boolean

  @Prop({ default: false })
  nullable!: Boolean

  @Prop({ default: 'input' })
  tag!: String

  @Prop({ default: 'zh' })
  locale!: string

  @Prop({ default: 'min' })
  initial!: string

  datepicker: any = null

  get valueDate () {
    let val = this.value.split('T')[0]

    if (val === '0001-01-01') {
      return ''
    } else {
      return val
    }
  }

  dateUpdated (selectedDates: any, value: any) {
    if (this.clearable && this.valueDate === value) {
      this.handleInput(DateTime.minValue)
    } else {
      this.handleInput(value + 'T00:00:00')
    }
  }

  handleClick () {
    if (!this.datepicker) {
      let config = {} as any
      config.onValueUpdate = this.dateUpdated
      config.clickOpens = !this.readonly
      if (this.locale === 'zh') {
        config.locale = zh.zh
      }
      this.datepicker = Flatpickr(this.$el, config)
      this.handleValueChange(this.value)
      !this.readonly && this.datepicker.open()
    }
  }

  handleDestroy () {
    if (this.datepicker) {
      this.datepicker.destroy()
      this.datepicker = null
    }
  }

  handleValueChange (value: string) {
    if (this.datepicker) {
      this.datepicker.setDate(this.value === DateTime.minValue ? '' : value)
    }
  }

  private handleInput (value: string) {
    this.$emit('input', value)
  }

  mounted () {
    if (this.value === '') {
      if (this.initial === 'min') {
        this.handleInput(DateTime.minValue)
      } else {
        this.handleInput(DateTime.now)
      }
    }
  }

  beforeDestroy () {
    this.handleDestroy()
  }
}
</script>

<style lang="sass">
@import '~flatpickr/dist/flatpickr.min.css'
@import '~flatpickr/dist/themes/material_blue.css'

.flatpickr-calendar
  margin-top: 0.375rem

.dayContainer
  padding: 0.5rem 0.25rem 0.25rem 0.25rem

span.flatpickr-day
  margin-bottom: 0.125rem
</style>
