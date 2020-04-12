import { Vue, Component } from 'vue-property-decorator'
import Axios from 'axios'

interface IString {
  toString() : string
}

@Component
export default class DataSet<TKey extends IString = number> extends Vue {
  //
  api!: string

  page: number = 1

  pageSize: number = 20

  search: string | null = null

  get params (): object {
    return {}
  }

  //

  isPending: boolean = false

  entities: Entities<TKey> = new Entities<TKey>()

  // computed

  get relationships () {
    return this.entities.relationships
  }

  get entityList () {
    const { keys, data } = this.entities

    return keys.map(key => data[key.toString()])
  }

  // methods

  getEntity (key: string | number)  {
    return this.entities.data[key]
  }

  async getEntities () {
    if (this.isPending) return

    const params = Object.assign({
      page: this.page,
      page_size: this.pageSize,
      search: this.search,
    }, this.params)

    this.isPending = true

    try {
      const response = await Axios.post(this.api, params)
      this.entities = response.data
    } finally {
      this.isPending = false
    }
  }

  async handleSearch (value: string) {
    this.search = value !== '' ? value : null
    await this.getEntities()
  }

  async handlePageChange (page: number) {
    this.page = page
    await this.getEntities()
  }

  async handlePageSizeChange (pageSize: number) {
    this.page = 1
    this.pageSize = pageSize
    await this.getEntities()
  }

  async handleRefresh () {
    await this.handlePageChange(1)
  }
}

class Entities<TKey extends IString> {
  meta = {
    page: 1,
    pageSize: 1,
    total: 1,
  }

  keys: TKey[] = new Array<TKey>()

  data: { [key: string]: object } = {}

  relationships: any = {}
}
