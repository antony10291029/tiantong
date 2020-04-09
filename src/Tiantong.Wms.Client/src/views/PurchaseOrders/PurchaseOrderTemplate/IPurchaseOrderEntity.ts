import {
  User,
  Good,
  Item,
  Project,
  Supplier,
  Department,
  PurchaseOrder,
} from '@/Entities'

export interface IPurchaseOrderEntity {
  supplier: Supplier,
  order: PurchaseOrder,
  department: Department,
  applicant: User,
  goods: { [ key: string ]: Good },
  items: { [ key: string ]: Item },
  projects: { [ key: string ]: Project },
}
