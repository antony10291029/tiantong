<template>
  <div class="field" style="width: 320px">
    <div class="label">订单状态</div>
    <div class="control">
      <table class="table is-hoverable is-bordered is-fullwidth is-centered">
        <tbody>
          <tr
            v-for="state in states" :key="state"
            @click="$emit('change', state)"
          >
            <td style="width: 1px">
              <Radio :value="value === state"></Radio>
            </td>
            <td>{{state}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'InvoiceTypeSelector',
  model: {
    prop: 'value',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  value!: string

  get text () {
    switch (this.value) {
      case '增值税专用发票': return '专票'
      case '增值税普通发票': return '普票'
      case '增值税电子普通发票': return '电子普票'
    }
  }

  states = [
    '未完成', '已完成'
  ]

  isShow: boolean = false

  handleClose () {
    this.isShow = false
  }

  handleChange (value: string) {
    this.$emit('change', value)
    this.handleClose()
  }

  created () {
    if (this.value === '') {
      this.$emit('change', this.states[0])
    }
  }
}
</script>
