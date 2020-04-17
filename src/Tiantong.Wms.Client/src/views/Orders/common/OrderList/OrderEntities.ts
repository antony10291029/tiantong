import {
  User,
  Good,
  Item,
  Project,
  Supplier,
  Department
} from '@/Entities'

export interface IOrderRelationships {
  users: { [key: string]: User }
  goods: { [key: string]: Good }
  items: { [key: string]: Item }
  projects: { [key: string]: Project }
  suppliers: { [key: string]: Supplier }
  departments: { [key: string]: Department }
}

export default interface IOrderEntities {
  relationships: IOrderRelationships
}
