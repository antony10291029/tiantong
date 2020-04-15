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
import Order from './Order'
import OrderPayment from './OrderPayment'
import OrderItem from './OrderItem'
import OrderItemProject from './OrderItemProject'

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
  Order,
  OrderPayment,
  OrderItem,
  OrderItemProject
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
