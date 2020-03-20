import Axios from '@/providers/axios'

interface Options {
  pageSize: number
  searchApi: string
  searchParams: object | (() => object)
}

export default function ({
  pageSize = 15,
  searchApi,
  searchParams = () => {},
}: {
  searchApi: string
  pageSize?: number
  searchParams?: object | (() => object)
}) {
  const options: Options = {
    pageSize,
    searchApi,
    searchParams
  }

  return {
    data: resolveData(options),
    methods: resolveMethods(options),
    computed: resolveComputed(options)
  }
}

const resolveData = (options: Options) => () => {
  let params: any = {}

  if (typeof options.searchParams == 'object') {
    params = options.searchParams
  }

  params.page = 1
  params.page_size = options.pageSize
  params.search = undefined

  return {
    result: [],
    entities: {},
    meta: {
      page: 0,
      pageSize: 0,
      total: 0
    },
    params,
    isPending: false,
  }
}

const resolveComputed = (config: Options): any => ({
  dataSet () {
    return this.result.map((id: any) => this.entities[id])
  },
  isSelectedAll () {
    return this.dataSet.every((entity: any) => entity.$selected) && this.dataSet.length
  },
  isSelectedNone () {
    return !this.dataSet.some((entity: any) => entity.$selected)
  },
  selectedCheckbox () {
    if (this.isSelectedAll) return true
    else if (this.isSelectedNone) return false
    else return 'minus'
  },
  selectedIds () {
    return this.dataSet.filter((entity: any) => entity.$selected)
      .map((entity: any) => entity.id)
  }
})

const resolveMethods = (options: Options): any => ({
  async getDataSet () {
    if (this.isPending) return

    const page = this.params.page
    const pageSize = this.params.page_size

    this.params = {
      ...this.params,
      ...(options.searchParams as (vm: any) => object)(this)
    }

    this.isPending = true
    try {
      const response = await Axios.post(options.searchApi, this.params)
      const result: any = [], entities: any = {}
      const { total, data } = response.data

      this.meta.page = page
      this.meta.pageSize = pageSize
      this.meta.total = total

      data.forEach((entity: any) => {
        entity.$selected = false
        result.push(entity.id)
        entities[entity.id] = entity
      })

      this.result = result
      this.entities = entities
    } finally {
      this.isPending = false
    }
  },
  search (value: string) {
    if (value !== '') {
      this.params.search = value
    } else {
      this.params.search = undefined
    }

    this.getDataSet()
  },
  changePage (page: number) {
    this.params.page = page
    this.getDataSet()
  },
  changePageSize (pageSize: number) {
    this.params.page = 1
    this.params.pageSize = pageSize
    this.getDataSet()
  },
  refresh () {
    this.changePage(1)
  },
  find (id: any) {
    return this.entities[id]
  },
  select (id: any) {
    this.entities[id].$selected = !this.entities[id].$selected
  },
  selectAll () {
    const value = !this.selectedStatus

    this.dataSet.forEach((entity: any) => entity.$selected = value)
  },
  remove (id: any) {
    this.$delete(this.entities[id])
    this.result.splice(this.result.indexOf(id), 1)
  },
})
