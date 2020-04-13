<template>
  <tbody>
    <template v-for="(orderItem, orderItemIndex) in orderItems">
      <template v-for="(itemProject, itemProjectIndex) in getItemProjects(orderItem.projects)">
        <tr :key="`${orderItemIndex} + ${itemProjectIndex} + ${$uid()}`">
          <slot v-bind="getSlotProps(orderItem, itemProject, orderItemIndex, itemProjectIndex)"></slot>
        </tr>
      </template>
    </template>
  </tbody>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Good, Item, Project, PurchaseOrderItem, PurchaseOrderItemProject } from '@/Entities'
import cloneDeep from 'lodash/cloneDeep'

@Component({
  name: 'PurchaseOrderItemRows',
  components: {

  }
})
export default class extends Vue {
  @Prop({ required: true })
  orderItems!: PurchaseOrderItem[]

  @Prop({ required: true })
  projects!: { [key: string]: Project }

  @Prop({ required: true })
  goods!: { [key: string]: Good }

  @Prop({ required: true })
  items!: { [key: string]: Item }

  isGoodHovered: boolean = false

  project: boolean = false

  getItemProjects (projects: PurchaseOrderItemProject[]) {
    return projects.length === 0 ? [ null ] : projects
  }

  getSlotProps (
    orderItem: PurchaseOrderItem,
    itemProject: PurchaseOrderItemProject,
    orderItemIndex: number,
    itemProjectIndex: number
  ) {
    return {
      orderItem: orderItem,
      orderItemIndex: orderItemIndex,
      orderItemFinance: orderItem.finance,
      itemProjectIndex: itemProjectIndex,
      itemProject: itemProject || new PurchaseOrderItemProject,
      good: this.goods[orderItem.good_id],
      item: this.items[orderItem.item_id],
      project: itemProject && this.projects[itemProject.project_id],
      isOrderItemShow: itemProjectIndex === 0,
      isItemGoodShow: orderItem.good_id !== 0 && orderItem.item_id !== 0,
      isItemProjectsShow: itemProject !== null,
      orderItemRowspan: Math.max(orderItem.projects.length, 1),
    }
  }
}
</script>
