<template>
  <router-view
    :warehouseId="warehouseId"
    :baseURL="baseURL + '/requisition-orders'"
  ></router-view>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'PurchaseRequisition',
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  baseURL!: string
}
</script>
