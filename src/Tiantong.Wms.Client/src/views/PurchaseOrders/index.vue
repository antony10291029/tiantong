<template>
  <router-view
    :warehouseId="warehouseId"
    :baseURL="baseURL + '/purchase-orders'"
  ></router-view>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'PurchaseOrders',
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  baseURL!: string
}
</script>
