import PurchaseOrderItemProject from './PurchaseOrderItemProject'
import PurchaseOrderItemFinance from './PurchaseOrderItemFinance'

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

  arrived_at: string = ''

  finance: PurchaseOrderItemFinance

  projects: Array<PurchaseOrderItemProject>

  constructor (projects: Array<PurchaseOrderItemProject> = []) {
    this.projects = projects
    this.finance = new PurchaseOrderItemFinance
  }
}
