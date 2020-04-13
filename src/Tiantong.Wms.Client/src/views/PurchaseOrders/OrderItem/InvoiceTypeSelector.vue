<template>
  <td
    v-class:is-clickable="!isShow"
    @click="isShow = !isShow"
  >
    <span>{{text}}</span>
    <div
      v-if="isShow"
      class="modal is-active"
    >
      <div
        @click.stop="handleClose"
        class="modal-background"
      ></div>
      <div
        class="modal-content has-background-white"
        style="width: 320px; border-radius: 0.5rem"
      >
        <table
          class="table is-bordered is-fullwidth is-hoverable is-size-5"
          style="z-index: 99999"
        >
          <thead>
            <th>发票类型</th>
          </thead>
          <tbody>
            <tr v-for="invoice in invoiceTypes" :key="invoice.value">
              <td @click.stop="handleChange(invoice.value)">
                {{invoice.text}}
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </td>
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

  invoiceTypes = [
    { value: '增值税专用发票', text: '专用发票'},
    { value: '增值税普通发票', text: '普通发票' },
    { value: '增值税电子普通发票', text: '电子发票' },
  ]

  isShow: boolean = false

  handleClose () {
    this.isShow = false
  }

  handleChange (value: string) {
    this.$emit('change', value)
    this.handleClose()
  }
}
</script>
