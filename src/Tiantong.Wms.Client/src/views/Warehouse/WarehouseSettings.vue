<template>
  <AsyncLoader
    :handler="getWarehouse"
    class="is-flex-auto" style="padding: 1rem"
  >
    <h1 class="title is-size-4">仓库设置</h1>

    <div style="max-width: 600px;">
      <div class="field">
        <label class="label">名称</label>
        <div class="control">
          <input class="input" type="text" v-model="warehouse.name">
        </div>
      </div>

      <div class="field">
        <label class="label">编号</label>
        <div class="control">
          <Input
            :default="null"
            class="input" type="text"
            v-model="warehouse.number"
          />
        </div>
      </div>

      <div class="field">
        <label class="label">地址</label>
        <div class="control">
          <Textarea v-model="warehouse.address"></Textarea>
        </div>
      </div>

      <div class="field">
        <label class="label">备注</label>
        <div class="control">
          <Textarea v-model="warehouse.comment"></Textarea>
        </div>
      </div>

      <div class="field is-grouped">
        <div class="control">
          <AsyncButton
            :disabled="!isChanged"
            :handler="handleSave"
            class="button is-info"
          >
            提交
          </AsyncButton>
        </div>
      </div>
    </div>
  </asyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import cloneDeep from 'lodash/cloneDeep'
import { Warehouse, getDiffs, isModified } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DataModifier from '@/mixins/data-modifier'
import Textarea from '@/components/Textarea.vue'
import axios from '@/providers/axios'
import isEqual from 'lodash/isEqual'

@Component({
  name: 'WarehouseSettings',
  components: {
    AsyncButton,
    AsyncLoader,
    Textarea
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  warehouse: Warehouse = new Warehouse

  sourceData: Warehouse = new Warehouse

  get isChanged () {
    return !isEqual(this.warehouse, this.sourceData)
  }

  handleReset () {
    this.warehouse = cloneDeep(this.sourceData)
  }

  async handleSave () {
    await axios.post('/warehouses/update', this.warehouse)
    await this.getWarehouse()
  }

  async getWarehouse () {
    let response = await axios.post('warehouses/find', {
      id: this.warehouseId
    })

    this.sourceData = response.data
    this.handleReset()
  }
}
</script>
