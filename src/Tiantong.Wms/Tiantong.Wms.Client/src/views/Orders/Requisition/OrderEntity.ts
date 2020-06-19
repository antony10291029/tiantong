import BaseEntity from '../common/OrderTemplate/OrderEntity'
import axios from '@/providers/axios'

export default class OrderEntity extends BaseEntity {
  async find(warehouseId: number, orderId: number) {
    let response = await axios.post('/requisition-orders/find', {
      order_id: orderId,
      warehouse_id: warehouseId
    })

    this.setFrom(response.data)
  }

  async create () {
    this.setIndex()
    return await axios.post('/requisition-orders/create', this.order)
  }

  async delete () {
    if (this.order.id === 0) return

    await axios.post('/requisition-orders/delete', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

  async update () {
    this.setIndex()
    await axios.post('/requisition-orders/update', this.order)
  }

  async finish (locationId: number) {
    await axios.post('/requisition-orders/finish', {
      order_id: this.order.id,
      location_id: locationId,
      warehouse_id: this.order.warehouse_id
    })
  }

  async file () {
    await axios.post('/requisition-orders/file', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

  async restore () {
    await axios.post('/requisition-orders/restore', {
      order_id: this.order.id,
      warehouse_id: this.order.warehouse_id
    })
  }

}
