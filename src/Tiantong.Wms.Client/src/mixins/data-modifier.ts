import axios from '@/providers/axios'
import { isStrictEqual } from '@/utils/common'

interface Options {
  dataApi: string
  updateApi: string
  updateParams: (vm: any) => object
  data: string
  params: string
  dataId: string
  paramsId: string
  confirmTitle: string
  confirmContent: string
  successText: string
  failureText: string
}

export default function ({
  dataApi,
  updateApi,
  updateParams,
  data = 'data',
  params = 'params',
  dataId = 'id',
  paramsId = 'id',
  confirmTitle = '提示',
  confirmContent = '信息尚未保存，是否确认离开',
  successText = '数据已更新',
  failureText = '数据更新失败',
}: {
  dataApi: string
  updateApi: string
  updateParams: any
  data?: string
  params?: string
  dataId?: string
  paramsId?: string
  confirmTitle?: string
  confirmContent?: string
  successText?: string
  failureText?: string
}) {
  const mixin = {}
  const config: Options = {
    dataApi,
    updateApi,
    updateParams,
    data,
    params,
    dataId,
    paramsId,
    confirmTitle,
    confirmContent,
    successText,
    failureText
  }

  bindData(mixin, config)
  bindHooks(mixin, config)
  bindMethods(mixin, config)
  bindComputed(mixin, config)

  return mixin
}

function bindData (mixin: any, { data, params }: Options) {
  mixin.data = function () {
    const res = {} as any

    res[data] = {}
    res[params] = {}

    return res
  }
}

function bindComputed (mixin: any, { data, params }: Options) {
  mixin.computed = {
    changedParams () {
      const result = {} as any
      Object.keys(this[params])
        .forEach(key => {
          if (!isStrictEqual(this[params][key], this[data][key])) {
            result[key] = this[params][key]
          }
        })

      return result
    },
    isChanged () {
      return !!Object.keys(this.changedParams).length
    },
    // todo remove
    isModified () {
      return !!Object.keys(this.changedParams).length
    }
  }
}

function bindMethods (mixin: any, { dataApi, updateParams, updateApi, data, params, dataId, paramsId, successText, failureText }: Options) {
  mixin.methods = {
    async getData () {
      var response = await axios.post(dataApi, updateParams(this))
      this[data] = response.data
      Object.keys(this[params]).forEach(key => {
        this[params][key] = this[data][key]
      })
    },

    async handleSave () {
      if (!this.isChanged) return

      const keys = Object.keys(this.changedParams)
      this.changedParams[paramsId] = this[data][dataId]

      this.beforeUpdate && this.beforeUpdate()
      try {
        await axios.post(updateApi, this.changedParams)
      } catch (error) {
        this.$notify.danger(failureText)

        throw error
      }

      keys.forEach(key => {
        this.$set(this[data], key, this[params][key])
      })

      var temp = this[data]
      this[data] = {}
      this[data] = temp

      this.$notify.success(successText)
      this.updated && this.updated()
    }
  }
}

function bindHooks (mixin: any, { confirmTitle, confirmContent }: Options) {
  mixin.beforeRouteLeave = mixin.beforeRouteUpdate = function (to: any, from: any, next: any) {
    if (!this.isChanged) {
      return next()
    }
    this.$confirm({
      title: confirmTitle,
      content: confirmContent,
      handler: () => next()
    })
  }
}
