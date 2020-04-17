import { cloneDeep, isEqual } from 'lodash'
import {
  User,
  Good,
  Item,
  Project,
  Supplier,
  Department,
  Order,
  OrderItem,
  OrderPayment,
} from '@/Entities'

export default abstract class OrderEntity {
  order: Order

  applicant: User

  supplier: Supplier

  department: Department

  goods: { [ key: string ]: Good }

  items: { [ key: string ]: Item }

  projects: { [ key: string ]: Project }

  constructor () {
    this.applicant = new User
    this.supplier = new Supplier
    this.department = new Department
    this.goods = { 0: new Good }
    this.items = { 0: new Item }
    this.projects = { 0: new Project }
    this.order = new Order(
      [ new OrderPayment ],
      [ new OrderItem ]
    )
  }

  setFrom (entity: OrderEntity) {
    this.order = entity.order
    this.goods = entity.goods
    this.items = entity.items
    this.projects = entity.projects
    this.supplier = entity.supplier
    this.applicant = entity.applicant
    this.department = entity.department
  }

  setIndex () {
    this.order.payments?.forEach((payment, index) => payment.index = index)
    this.order.items?.forEach((item, index) => item.index = index)
    this.order.items?.forEach(item => item.projects?.forEach((project, index) => project.index = index))
  }

  copyFrom (entity: OrderEntity) {
    this.supplier = entity.supplier
    this.department = entity.department
    this.applicant = entity.applicant
    this.order = cloneDeep(entity.order)
    this.goods =  Object.assign(entity.goods, { 0: new Good })
    this.items =  Object.assign(entity.items, { 0: new Item })
    this.projects =  Object.assign(entity.projects, { 0: new Project })
  }

  isChanged (entity: OrderEntity) {
    return !isEqual(this.order, entity.order)
  }

  abstract async finish(locationId: number): Promise<any>

}
