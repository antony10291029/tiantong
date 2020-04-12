<template>
  <AsyncLoader :handler="handler">
    <slot name="header"></slot>
<!-- 
    <div
      class="field"
      style="width: 320px"
    >
      <div class="label">
        <label>货码</label>
      </div>
      <div class="control">
        <Input v-model="good.number" type="text" class="input" :default="null" />
      </div>
    </div> -->

    <div
      class="field"
      style="width: 320px"
    >
      <div class="label">
        <label>货名</label>
      </div>
      <div class="control">
        <Input v-model="good.name" type="text" class="input" />
      </div>
    </div>

    <div
      class="field"
      style="width: 520px"
    >
      <div class="label">
        <label>备注</label>
      </div>
      <div class="control">
        <Textarea v-model="good.comment"></Textarea>
      </div>
    </div>

    <div
      class="field"
      style="width: 520px"
    >
      <div class="label">
        <label>规格</label>
      </div>
      <div class="control">
          <table class="table is-bordered is-fullwidth">
            <thead>
              <tr>
                <th style="width: 1px">#</th>
                <!-- <th>规格码</th> -->
                <th>规格名</th>
                <th>单位</th>
                <th style="width: 1px">
                  <a
                    class="icon"
                    @click="handleItemAdd"
                  >
                    <i class="iconfont icon-plus"></i>
                  </a>
                </th>
              </tr>
            </thead>
            <tbody>
              <GoodItemsRow
                v-for="(item, index) in good.items"
                :index="index"
                :count="good.items.length"
                :key="`${$uid()}_${item.id}`"
                @delete="handleItemDelete(item, $event)"
              >
                <!-- <EditableCell v-model="item.number" :default="null" /> -->
                <EditableCell v-model="item.name" />
                <EditableCell v-model="item.unit" />
              </GoodItemsRow>
            </tbody>
          </table>
      </div>
    </div>

    <div class="field">
      <div class="label">
        <Checkbox v-model="good.is_enabled" class="label">是否启用</Checkbox>
      </div>
    </div>

    <slot name="footer"></slot>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import DataEditor from '@/share/DataEditor'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DatePicker from '@/components/DatePicker/index.vue'
import GoodItems from './GoodItems.vue'
import GoodItemsRow from './GoodItemsRow.vue'
import { Good, Item } from '@/Entities'

@Component({
  name: 'GoodUpdate',
  components: {
    Textarea,
    AsyncButton,
    AsyncLoader,
    DatePicker,
    GoodItems,
    GoodItemsRow
  }
})
export default class extends DataEditor {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  good!: Good

  @Prop({ default: () => () => {} })
  handler!: () => {}

  handleItemAdd () {
    let item = new Item()
    item.good_id = this.good.id
    item.warehouse_id = this.warehouseId
    this.good.items.push(item)
  }

  async handleItemDelete (item: Item, index: number) {
    if (item.id != 0) {
      await axios.post('/goods/items/deletable', { id: item.id })
    }
    this.good.items.splice(index, 1)
  }
}
</script>
