export default class DataSet<TEntity> {
  result: number[]
  entities: { [Key: number]: TEntity }

  constructor () {
    this.result = []
    this.entities = {}
  }

  clear () {
    this.result = []
    this.entities = {}
  }

  add (key: number, entity: TEntity) {
    this.result.push(key)
    this.entities[key] = entity
  }
}
