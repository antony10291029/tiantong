<template>
  <tbody>
    <template v-for="(order, orderIndex) in orders">
      <template v-for="(item, itemIndex) in order.items">
        <template v-for="(project, projectIndex) in (item.projects.length === 0 ? [ null ] : item.projects)">
          <tr :key="projectIndex + '_' + $uid()">
            <slot v-bind="getProps(order, item, project, orderIndex, itemIndex, projectIndex)">
            </slot>
          </tr>
        </template>
      </template>
    </template>
  </tbody>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Order, OrderItem, OrderItemProject } from '@/Entities'
import { IOrderRelationships } from './OrderEntities'

@Component({
  name: 'PurchaseOrderRow'
})
export default class extends Vue {
  @Prop({ required: true })
  orders!: Array<any>

  @Prop({ required: true })
  relationships!: IOrderRelationships

  getProps (
    order: Order,
    item: OrderItem,
    project: OrderItemProject,
    orderIndex: number,
    itemIndex: number,
    projectIndex: number
  ) {
    return {
      order,
      orderItem: item,
      itemProject: project,
      invoice: item.invoice ?? null,
      orderIndex,
      itemIndex,
      projectIndex,
      good: this.relationships.goods[item.good_id] ?? null,
      item: this.relationships.items[item.item_id] ?? null,
      project: project === null ? null : this.relationships.projects[project.project_id] ?? null,
      supplier: this.relationships.suppliers[order.supplier_id] ?? null,
      department: this.relationships.departments[order.department_id] ?? null,
      applicant: this.relationships.users[order.applicant_id],
      orderRowspan: order.items.reduce(
        (count, item) => count + Math.max(item.projects.length, 1), 0
      ),
      itemRowspan: Math.max(item.projects.length, 1),
      projectRowspan: 1,
      isOrderShow: itemIndex === 0 && projectIndex === 0,
      isItemShow: projectIndex === 0,
      isProjectShow: true,
    }
  }
}
</script>
