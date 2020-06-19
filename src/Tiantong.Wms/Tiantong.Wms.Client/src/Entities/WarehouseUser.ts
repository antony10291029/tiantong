import User from './User'
import Department from './Department'

export default class WarehouseUser {
  id: number = 0

  warehouse_id: number = 0

  department_id: number = 0

  user_id: number = 0

  user: User = new User

  department: Department | null = new Department
}
