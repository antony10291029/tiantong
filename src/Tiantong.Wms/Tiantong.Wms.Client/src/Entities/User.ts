export default class User {
  id: number = 0

  email: string = ''

  name: string = ''

  type: string = 'keeper'

  is_enabled: boolean = false

  is_deleted: boolean = false

  created_at: string = (new Date()).toISOString()
}
