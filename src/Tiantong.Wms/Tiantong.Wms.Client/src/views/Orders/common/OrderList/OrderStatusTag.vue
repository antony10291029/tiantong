<template>
  <span :class="`tag is-${result.color}`">
    {{result.text}}
  </span>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'OrderStatusTag'
})
export default class extends Vue{
  @Prop({ required: true })
  value!: string

  @Prop({ required: true })
  createdStatus!: IStatus

  @Prop({ required: true })
  finishedStatus!: IStatus

  get result () {
    switch (this.value) {
      case 'created': return this.createdStatus
      case 'finished': return this.finishedStatus
      case 'filed': return {
        text: '已归档',
        color: 'info'
      }
      default: return {
        text: '未知',
        color: 'danger'
      }
    }
  }
}

interface IStatus {
  text: string
  color: string
}
</script>
