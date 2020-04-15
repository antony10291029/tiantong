import { cloneDeep, isEqual } from 'lodash'
import axios from '@/providers/axios'
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

export class OrderEntity {
  applicant: User

  supplier: Supplier

  order: Order

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
    this.order.payments.forEach((payment, index) => payment.index = index)
    this.order.items.forEach((item, index) => item.index = index)
    this.order.items.forEach(item => item.projects.forEach((project, index) => project.index = index))
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

  async find(warehouseId: number, orderId: number) {
    let response = await axios.post('/purchase-orders/find', {
      order_id: orderId,
      warehouse_id: warehouseId
    })

    this.setFrom(response.data)
  }

  async create () {
    this.setIndex()
    return await axios.post('/purchase-orders/create', this.order)
  }

  async delete () {
    if (this.order.id === 0) return

    await axios.post('/purchase-orders/delete', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

  async update () {
    this.setIndex()
    await axios.post('/purchase-orders/update', this.order)
  }

  async finish (locationId: number) {
    await axios.post('/purchase-orders/finish', {
      order_id: this.order.id,
      location_id: locationId,
      warehouse_id: this.order.warehouse_id
    })
  }

  async file () {
    await axios.post('/purchase-orders/file', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

  async restore () {
    await axios.post('/purchase-orders/restore', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

}
