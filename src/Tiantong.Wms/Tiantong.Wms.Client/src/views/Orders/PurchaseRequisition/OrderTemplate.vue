<template>
  <OrderTemplate
    v-bind="$attrs"
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
  name: 'PurchaseRequestOrderTemplate',
  components: {
    OrderTemplate
  }
})
export default class extends Vue {

}
</script>
