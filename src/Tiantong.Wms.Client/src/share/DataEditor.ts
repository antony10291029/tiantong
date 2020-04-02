import { Vue, Component } from 'vue-property-decorator'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'
import isEqual from 'lodash/isEqual'

@Component
export default class DataEditor extends Vue {
  findApi!: string

  updateApi!: string

  sourceData: any = {}

  currentData: any = {}

  data: any = {}

  params: any = {}

  confirm = {
    title: '提示',
    content: '信息尚未保存，是否确认离开'
  }

  get findParams (): object {
    return {}
  }

  get updateParams (): object {
    return {}
  }

  get isChanged (): boolean {
    return !isEqual(this.currentData, this.sourceData)
  }

  currentParams () {
    Object.keys(this.params).forEach(key => {
      this.params[key] = this.currentData[key]
    })

    return this.params
  }

  resetData () {
    this.currentData = cloneDeep(this.sourceData)
  }

  async getData () {
    var response = await axios.post(this.findApi, this.findParams)
    this.sourceData = cloneDeep(response.data)
    this.resetData()
  }

  async handleSave () {
    if (!this.isChanged) return

    await axios.post(this.updateApi, this.currentData)

    this.getData()
  }

  beforeRouteLeave (to: any, from: any, next: any) {
    if (!this.isChanged) {
      return next()
    }
    this.$confirm({
      title: this.confirm.title,
      content: this.confirm.content,
      handler: () => next()
    })
  }
}
