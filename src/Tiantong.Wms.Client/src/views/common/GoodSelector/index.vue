<template>
  <component
    :class="isShow || readonly || 'is-clickable'"
    @click.self="!readonly && (isShow = true)"
    :is="tag"
  >
    <slot></slot>
    <AsyncLoader
      v-if="isShow"
      :handler="getEntities"
      class="modal is-active"
      style="text-align: left"
    >
      <div
        class="modal-background"
        @click="handleClose"
      ></div>
      <div class="modal-card" style="height: 100%">
        <header class="modal-card-head is-flex is-vcentered">
          <p class="modal-card-title">选择物品</p>
        </header>
        <section class="modal-card-body">
          <div class="is-flex">
            <SearchField
              :isPending="isPending"
              @search="handleSearch"
            ></SearchField>
            <div class="is-flex-auto"></div>
            <Pagination
              v-bind="entities.meta"
              @change="handlePageChange"
            ></Pagination>
          </div>
          <div style="height: 0.75rem"></div>
          <Table
            v-if="!isPending"
            class="is-hoverable"
          >
            <thead>
              <th style="width: 1px">#</th>
              <th>货名</th>
              <th>备注</th>
              <th>规格名</th>
              <th>单位</th>
              <th style="width: 1px">
                <span
                  class="icon"
                  style="color: transparent"
                >
                  <i class="iconfont icon-tick"></i>
                </span>
              </th>
            </thead>
            <tbody>
              <template v-for="(good, goodIndex) in entityList">
                <template v-for="(item, itemIndex) in good.items">
                  <tr :key="`${good.id}_${item.id}`">
                    <template v-if="itemIndex == 0">
                      <td :rowspan="good.items.length">
                        {{goodIndex + 1}}
                      </td>
                      <td :rowspan="good.items.length">
                        {{good.name}}
                      </td>
                      <td :rowspan="good.items.length">
                        {{good.comment}}
                      </td>
                    </template>
                    <td @click="handleSelectItem(good, item)">
                      {{item.name}}
                    </td>
                    <td @click="handleSelectItem(good, item)">
                      {{item.unit}}
                    </td>
                    <td @click="handleSelectItem(good, item)">
                      <span
                        v-if="currentGood.id == good.id && currentItem.id == item.id"
                        class="icon has-text-success"
                      >
                        <i class="iconfont icon-tick"></i>
                      </span>
                    </td>
                  </tr>
                </template>
              </template>
            </tbody>
          </Table>
        </section>
        <footer class="modal-card-foot">
          <a
            @click.stop="handleSelect"
            class="button is-success"
          >选择</a>
          <div class="is-flex-auto"></div>
          <GoodCreate
            :warehouseId="warehouseId"
            @refresh="getEntities"
          ></GoodCreate>
        </footer>
      </div>
    </AsyncLoader>
  </component>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncLoader from '@/components/AsyncLoader.vue'
import Pagination from '@/components/Pagination.vue'
import SearchField from '@/components/SearchField.vue'
import Table from '@/components/Table.vue'
import GoodCreate from './GoodCreate.vue'
import DataSet from '@/share/DataSet'
import { Good, Item } from '../../../Entities'

@Component({
  name: 'GoodSelector',
  components: {
    Table,
    Pagination,
    AsyncLoader,
    SearchField,
    GoodCreate,
  }
})
export default class extends DataSet {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  good!: Good | null

  @Prop({ required: true })
  item!: Item | null

  @Prop({ required: false })
  readonly!: boolean

  @Prop({ default: 'td' })
  tag!: string

  api = '/goods/search'

  isShow: boolean = false

  currentGood: Good = new Good()

  currentItem: Item = new Item()

  get params () {
    return {
      warehouse_id: this.warehouseId
    }
  }

  handleClose () {
    this.isShow = false
  }

  handleSelectItem (good: Good, item: Item) {
    this.currentGood = good
    this.currentItem = item
  }

  handleSelect () {
    this.$emit('select', {
      good: this.currentGood,
      item: this.currentItem
    })
    this.handleClose()
  }

  created () {
    if (this.good !== null) {
      this.currentGood = this.good
    }
    if (this.item !== null) {
      this.currentItem = this.item
    }
  }
}
</script>
