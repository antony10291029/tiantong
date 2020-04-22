<template>
  <router-view
    :warehouseId="warehouseId"
    :department="department"
    :baseURL="baseURL + '/purchase-requisition-orders'"
  ></router-view>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Department } from '../../../Entities'

@Component({
  name: 'PurchaseRequisition',
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  department!: Department

}
</script>
