import Warehouse from './Warehouse'

export {
  Warehouse
}

export function getDiffs(origin: any, target: any, keys: string[], id: string = 'id') {
  var param: any = {}

  param[id] = origin[id]

  for (let key in keys) {
    if (origin[key] === target[key]) {
      param[key] = origin[key]
    }
  }

  return param
}

export function isModified(origin: any, target: any, key: string) {
  return origin[key] !== target[key]
}
