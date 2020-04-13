import PurchaseOrderItem from './PurchaseOrderItem'
import PurchaseOrderPayment from './PurchaseOrderPayment'
import { DateTime } from '@/utils/common'

export default class PurchaseOrder {
  id: number = 0

  warehouse_id: number = 0

  number: string | null = null

  operator_id: number = 0

  applicant_id: number = 0

  department_id: number = 0

  supplier_id: number = 0

  status: string = 'created'

  comment: string = ''

  due_time: string = DateTime.now

  created_at: string = DateTime.minValue

  finished_at: string = DateTime.now

  items: Array<PurchaseOrderItem>

  payments: Array<PurchaseOrderPayment>

  constructor (
    payments: Array<PurchaseOrderPayment> = [],
    items: Array<PurchaseOrderItem> = [],
  ) {
    this.items = items
    this.payments = payments
  }
}
