import axios from '@/providers/axios'
import { isStrictEqual } from '@/utils/common'

interface Options {
  dataApi: string
  dataParams: (vm: any) => object
  updateApi: string
  updateParams: (vm: any) => object
  confirmTitle: string
  confirmContent: string
}

export default function ({
  dataApi,
  dataParams,
  updateApi,
  updateParams = (vm: any) =>  ({ id: vm.data.id }),
  confirmTitle = '提示',
  confirmContent = '信息尚未保存，是否确认离开',
}: {
  dataApi: string
  updateApi: string
  dataParams: (vm: any) => object
  updateParams?: (vm: any) => object
  data?: string
  params?: string
  confirmTitle?: string
  confirmContent?: string
  successText?: string
  failureText?: string
}) {
  const mixin = {}
  const config: Options = {
    dataApi,
    updateApi,
    dataParams,
    updateParams,
    confirmTitle,
    confirmContent,
  }

  bindData(mixin, config)
  bindHooks(mixin, config)
  bindMethods(mixin, config)
  bindComputed(mixin, config)

  return mixin
}

function bindData (mixin: any, {  }: Options) {
  mixin.data = function () {
    return {
      data: {},
      params: {},
    }
  }
}

function bindComputed (mixin: any, { }: Options) {
  mixin.computed = {
    changedParams () {
      const result = {} as any
      Object.keys(this.params)
        .forEach(key => {
          if (!isStrictEqual(this.params[key], this.data[key])) {
            result[key] = this.params[key]
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

function bindMethods (mixin: any, { dataApi, dataParams, updateApi, updateParams }: Options) {
  mixin.methods = {
    async getData () {
      var response = await axios.post(dataApi, dataParams(this))
      this.data = response.data
      Object.keys(this.params).forEach(key => {
        this.params[key] = this.data[key]
      })
    },

    async handleSave () {
      if (!this.isChanged) return

      const keys = Object.keys(this.changedParams)
      Object.assign(this.changedParams, updateParams(this))

      this.beforeUpdate && this.beforeUpdate()
      try {
        await axios.post(updateApi, this.changedParams)
      } finally {}

      keys.forEach(key => {
        this.$set(this.data, key, this.params[key])
      })

      var temp = this.data
      this.data = {}
      this.data = temp

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
