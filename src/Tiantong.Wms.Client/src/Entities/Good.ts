import Item from './Item'

export default class Good {
  warehouse_id: number = 0

  number: string | null = null

  name: string = ''

  comment: string = ''

  is_enabled: boolean = true

  items: Array<Item> = []

}
