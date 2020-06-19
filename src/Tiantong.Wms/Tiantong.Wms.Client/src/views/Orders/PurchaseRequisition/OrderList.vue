<template>
  <OrderList
    v-bind="$attrs"
    path="/purchase-requisition-orders/search"
  />
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import OrderList from '../common/OrderList/index.vue'

@Component({
  name: 'PurchaseRequisitionOrderList',
  components: {
    OrderList
  }
})
export default class extends Vue {

}
</script>
