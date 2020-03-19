export function isStrictEqual (a: any, b: any) {
  if (typeof a === typeof b && typeof a === 'object') {
    // 处理 null 为 object 的问题
    if (a === null && b === null) {
      return a === b
    }

    if (a.length !== b.length) {
      return false
    }

    for (let key in a) {
      if (a[key] !== b[key]) {
        return false
      }
    }

    return true
  } else {
    return a === b
  }
}

export const DateTime = {
  minValue: '0001-01-01T00:00:00',

  get now () {
    return new Date()
  },

  getDate (dateTime: string) {
    return dateTime.substring(0, 10)
  },

}
