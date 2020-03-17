<template>
  <AsyncLoader :handler="getData" class="is-flex-auto" style="max-width: 800px; padding: 1rem">
    <h1 class="title is-size-4">仓库设置</h1>

    <div class="field">
      <label class="label">名称</label>
      <div class="control">
        <input class="input" type="text" v-model="params.name">
      </div>
    </div>

    <div class="field">
      <label class="label">编号</label>
      <div class="control">
        <input class="input" type="text" v-model="params.number">
      </div>
    </div>

    <div class="field">
      <label class="label">地址</label>
      <div class="control">
        <Textarea v-model="params.address"></Textarea>
      </div>
    </div>

    <div class="field">
      <label class="label">备注</label>
      <div class="control">
        <Textarea v-model="params.comment"></Textarea>
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
  </asyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'
import { Warehouse, getDiffs, isModified } from '@/Entities'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DataModifier from '@/mixins/data-modifier'
import Textarea from '@/components/Textarea.vue'

@Component({
  name: 'WarehouseSettings',
  components: {
    AsyncButton,
    AsyncLoader,
    Textarea
  },
  mixins: [
    DataModifier({
      dataApi: 'warehouses/find',
      updateApi: 'warehouses/update',
      updateParams: (vm: any) => ({ warehouse_id: vm.warehouseId }),
      data: 'warehouse'
    })
  ]
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number
  params = {
    name: '',
    number: '',
    address: '',
    comment: ''
  }
}
</script>
