let id = 0

export default {
  install (Vue: any) {
    Vue.prototype.$uid = function () {
      return id++
    }
  }
}