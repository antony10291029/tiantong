export default class Department {
  id: number = 0

  warehouse_id: number = 0

  name: string = ''

  type: string = 'user'

  static isAdmin (department: Department) {
    return department.type === 'owner' || department.type == 'admin'
  }
}
