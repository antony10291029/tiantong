import Item from './Item'
import { DateTime } from '@/utils/common'

export default class Good {
  id: number = 0

  warehouse_id: number = 0

  number: string | null = null

  name: string = ''

  comment: string = ''

  is_enabled: boolean = true

  created_at: string = DateTime.now

  items: Array<Item>

  constructor(items: Item[] = []) {
    this.items = items
  }

}
