<template>
  <OrderTemplate
    v-bind="$attrs"
    itemsTableWidth="1000px"
    :hasSupplier="false"
    :hasPayments="false"
    :hasInvoice="false"
    :hasPurchase="false"
  >
    <template #header>
      <slot name="header" />
    </template>
    <template #footer>
      <slot name="footer" />
    </template>
  </OrderTemplate>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import OrderTemplate from '../common/OrderTemplate/index.vue'

@Component({
  name: 'RequisitionOrderTemplate',
  components: {
    OrderTemplate
  }
})
export default class extends Vue {

}
</script>
