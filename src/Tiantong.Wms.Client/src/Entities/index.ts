import User from './User'
import Good from './Good'
import Item from './Item'
import Invoice from './Invoice'
import Project from './Project'
import Location from './Location'
import Supplier from './Supplier'
import Warehouse from './Warehouse'
import WarehouseUser from './WarehouseUser'
import Department from './Department'
import PurchaseOrder from './PurchaseOrder'
import PurchaseOrderPayment from './PurchaseOrderPayment'
import PurchaseOrderItem from './PurchaseOrderItem'
import PurchaseOrderItemProject from './PurchaseOrderItemProject'

export {
  User,
  Good,
  Item,
  Invoice,
  Project,
  Location,
  Supplier,
  Warehouse,
  Department,
  WarehouseUser,
  PurchaseOrder,
  PurchaseOrderPayment,
  PurchaseOrderItem,
  PurchaseOrderItemProject
}

export function getDiffs(origin: any, target: any, keys: string[], id: string = 'id') {
  var param: any = {}

  param[id] = origin[id]

  for (let key in keys) {
    if (origin[key] === target[key]) {
      param[key] = origin[key]
    }
  }

  return param
}

export function isModified(origin: any, target: any, key: string) {
  return origin[key] !== target[key]
}
