<template>
  <section>
    <nav class="breadcrumb is-medium">
      <ul>
        <li>
          <router-link :to="`/warehouses/${warehouseId}/goods`">
            库存
          </router-link>
        </li>
        <li class="is-active">
          <a>货品添加</a>
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
        <input v-model.lazy="params.number" type="text" class="input">
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
        <input v-model.lazy="params.name" type="text" class="input">
      </div>
    </div>

    <div
      class="field"
      style="width: 520px"
    >
      <label class="label">规格</label>
      <div class="control">
        <GoodItems @add="handleItemsAdd">
          <GoodItemsRow
            v-for="(item, index) in params.items" :key="item.$key"
            :item="item"
            :index="index"
            :count="params.items.length"
            @delete="params.items.splice($event, 1)"
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
        <Textarea v-model="params.comment"></Textarea>
      </div>
    </div>

    <div>
      <AsyncButton
        :handler="handleSubmit"
        class="button is-info"
      >
        提交
      </AsyncButton>
    </div>
  </section>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'
import DatePicker from '@/components/DatePicker/index.vue'
import GoodItems from './GoodItems.vue'
import GoodItemsRow from './GoodItemsRow.vue'

@Component({
  name: 'GoodCreate',
  components: {
    Textarea,
    AsyncButton,
    DatePicker,
    GoodItems,
    GoodItemsRow
  }
})
export default class extends Vue {
  @Prop({ required: true})
  warehouseId!: number

  params = {
    name: '',
    number: null,
    comment: '',
    specification: '个',
    warehouse_id: this.warehouseId,
    items: [ new Item ]
  }

  isPending: boolean = false

  get isChanged () {
    return this.params.name !== ''
  }

  handleCancel () {
    this.$router.go(-1)
  }

  handleItemsAdd (id: number) {
    if (this.params.items.length < 10) {
      this.params.items.push(new Item)
    }
  }

  async handleSubmit () {
    try {
      await axios.post('/goods/create', this.params)
      this.handleCancel()
      this.$emit('refresh')
    } finally {}
  }
}

class Item {
  static key: number = 0
  $key?: number
  number: string | null = null
  name: string = ''
  unit: string = '个'
  constructor () {
    this.$key = Item.key++
  }
}
</script>
