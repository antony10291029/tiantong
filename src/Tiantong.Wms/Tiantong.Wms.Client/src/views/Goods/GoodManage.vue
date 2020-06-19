<template>
  <GoodTemplate
    :handler="getGood"
    :good="currentData"
    :warehouseId="warehouseId"
  >
    <template #header>
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
    </template>

    <template #footer>
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
          <a
            class="button is-link is-light"
            :disabled="!isChanged"
            @click="handleReset"
          >
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
    </template>
  </GoodTemplate>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncButton from '@/components/AsyncButton.vue'
import GoodTemplate from './GoodTemplate/index.vue'
import { Good, Item } from '@/Entities'
import axios from '@/providers/axios'
import cloneDeep from 'lodash/cloneDeep'
import isEqual from 'lodash/isEqual'

@Component({
  name: 'GoodUpdate',
  components: {
    GoodTemplate,
    AsyncButton,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  goodId!: number

  sourceData = new Good()

  currentData = new Good()

  get isChanged () {
    return !isEqual(this.sourceData, this.currentData)
  }

  handleClose () {
    this.$router.go(-1)
  }

  handleReset () {
    this.currentData = cloneDeep(this.sourceData)
  }

  async getGood () {
    let response = await axios.post('/goods/find', { id: this.goodId })

    this.sourceData = response.data
    this.handleReset()
  }

  async handleSubmit () {
    this.currentData.items.forEach((item, index) => item.index = index)
    await axios.post('/goods/update', this.currentData)
    this.handleClose()
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

}
</script>
