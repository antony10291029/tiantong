import OrderItemProject from './OrderItemProject'
import Invoice from './Invoice'
import { DateTime } from '@/utils/common'

export default class PurchaseOrderItem {
  id: number = 0

  order_id: number = 0

  good_id: number = 0

  item_id: number = 0

  index: number = 0

  price: number = 0

  quantity: number = 0

  comment: string = ''

  delivery_cycle: string = '30天'

  arrived_at: string = DateTime.now

  invoice: Invoice

  projects: Array<OrderItemProject>

  constructor (projects: Array<OrderItemProject> = []) {
    this.projects = projects
    this.invoice = new Invoice
  }
}
