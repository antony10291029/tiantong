<template>
  <AsyncLoader :handler="getData">
    <nav class="breadcrumb is-medium">
      <ul>
        <li>
          <router-link :to="`/warehouses/${warehouseId}/goods`">
            库存
          </router-link>
        </li>
        <li class="is-active">
          <a>货品信息</a>
        </li>
      </ul>
    </nav>

    <hr>

    <div
      class="field"
      style="width: 320px"
    >
      <div class="label">
        <label>货码</label>
      </div>
      <div class="control">
        <input v-model.lazy="currentData.number" type="text" class="input">
      </div>
    </div>

    <div
      class="field"
      style="width: 320px"
    >
      <div class="label">
        <label>货名</label>
      </div>
      <div class="control">
        <input v-model.lazy="currentData.name" type="text" class="input">
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
        <GoodItems @add="handleItemAdd">
          <GoodItemsRow
            v-for="(item, index) in currentData.items"
            v-show="!item.is_deleted"
            :index="index"
            :count="currentData.items.filter(item => !item.is_deleted).length"
            :key="`${$uid()}_${item.id}`"
            @delete="handleItemDelete"
          >
            <EditableCell v-model="item.number"></EditableCell>
            <EditableCell v-model="item.name"></EditableCell>
            <EditableCell v-model="item.unit"></EditableCell>
          </GoodItemsRow>
        </GoodItems>
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
        <Textarea v-model="currentData.comment"></Textarea>
      </div>
    </div>

    <div class="field">
      <div class="label">
        <Checkbox v-model="currentData.is_enabled" class="label">是否启用</Checkbox>
      </div>
    </div>

    <div
      class="field is-grouped"
      style="width: 520px"
    >
      <div class="control">
        <AsyncButton
          class="button is-info"
          :disabled="!isChanged"
          :handler="handleSubmit"
        >
          保存
        </AsyncButton>
      </div>
      <div class="control">
        <a class="button is-link is-light" @click="resetData">
          重置
        </a>
      </div>
      <div class="control is-expanded"></div>
      <div class="control">
        <a
          @click="handleDelete"
          class="button is-danger is-light"
        >
          删除
        </a>
      </div>
    </div>
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
  goodId!: number

  findApi = '/goods/find'

  updateApi = '/goods/update'

  get findParams () {
    return { id: this.goodId }
  }

  sourceData = new Good()

  currentData = new Good()

  handleClose () {
    this.$router.go(-1)
  }

  getShowItems (items: Array<Item>) {
    return items.filter(item => !item.is_deleted)
  }

  async handleSubmit () {
    await this.handleSave()
  }

  async handleDelete () {
    this.$confirm({
      title: '提示',
      content: '是否确定删除该货品',
      handler: async () => {
        await axios.post('/goods/delete', { id: this.goodId })
        this.handleClose()
      }
    })
  }

  handleItemAdd () {
    this.currentData.items.push(new Item)
  }

  handleItemDelete (index: number) {
    let item = this.currentData.items[index]

    if (item.id === 0) {
      this.currentData.items.splice(index, 1)
    } else {
      item.is_deleted = true
    }
  }

}

class Good {
  name: string = ''
  number: string | null = null
  comment: string = ''
  is_enabled: boolean = true
  items: Array<Item> = []
}

class Item {
  id: number = 0
  number: string | null = null
  name: string = ''
  unit: string = '个'
  good_id: number = 0
  is_deleted: boolean = false
}
</script>
