<template>
  <component :is="wrapperComponent" />
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import Td from './td.vue'
import Input from './input.vue'
import Flatpickr from 'flatpickr'
import zh from 'flatpickr/dist/l10n/zh.js'
import { DateTime } from '@/utils/common'

@Component({
  name: 'DatePicker'
})
export default class extends Vue {
  @Prop({ required: true })
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
  wrapper!: String

  @Prop({ default: 'zh' })
  locale!: string

  config: any = {}

  datepicker: any

  selectedDates: any

  get wrapperComponent () {
    switch (this.wrapper) {
      case 'input': return Input
      case 'td': return Td
    }
  }

  get valueDate () {
    return DateTime.getDate(this.value)
  }

  redraw (newConfig: any) {
    this.datepicker.config = Object.assign(this.datepicker.config, newConfig)
    this.datepicker.redraw()
    this.datepicker.jumpToDate()
  }

  dateUpdated (selectedDates: any, value: any) {
    if (this.clearable && this.valueDate === value) {
      this.handleInput(DateTime.minValue)
    } else {
      this.handleInput(value + 'T00:00:00')
    }
  }

  @Watch('value')
  private handleValueChange (value: string) {
    if (this.clearable && this.value === DateTime.minValue) {
      this.datepicker.setDate('')
    } else {
      this.datepicker.setDate(value)
    }
  }

  private handleInput (value: string) {
    this.$emit('input', value)
  }

  mounted () {
    if (!this.datepicker) {
      this.config.onValueUpdate = this.dateUpdated
      if (this.locale === 'zh') {
        this.config.locale = zh.zh
      }
      this.config.clickOpens = !this.readonly
      this.datepicker = Flatpickr(this.$el, this.config)

      if (this.value === '') {
        this.handleInput(DateTime.minValue)
      } else {
        this.handleValueChange(this.value)
      }
    }

    this.$watch('config', this.redraw)
  }

  beforeDestroy () {
    if (this.datepicker) {
      this.datepicker.destroy()
      this.datepicker = null
    }
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
