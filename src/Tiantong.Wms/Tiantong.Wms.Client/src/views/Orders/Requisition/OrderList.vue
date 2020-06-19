<template>
  <OrderList
    v-bind="$attrs"
    path="/requisition-orders/search"
  />
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import OrderList from '../common/OrderList/index.vue'

@Component({
  name: 'RequisitionOrderController',
  components: {
    OrderList
  }
})
export default class extends Vue {

}
</script>
