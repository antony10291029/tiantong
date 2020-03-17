<template>
  <AsyncLoader class="modal is-active" :handler="getData">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">供应商编辑</p>
      </header>

      <section class="modal-card-body">
        {{params}}
        <div class="field">
          <div class="label">
            <label>供应商名称</label>
          </div>
          <div class="control">
            <input v-model="params.name" type="text" class="input">
          </div>
        </div>
        <div class="field">
          <div class="label">
            <label>供应商名称</label>
          </div>
          <div class="control">
            <Textarea v-model="params.comment" />
          </div>
        </div>
        <div class="field">
          <div class="control">
            <Checkbox v-model="params.is_enabled" class="label">是否启用</Checkbox>
          </div>
        </div>
      </section>

      <footer class="modal-card-foot">
        <AsyncButton
          :disabled="!isChanged"
          :handler="handleSubmit"
          class="button is-info"
        >
          确认
        </AsyncButton>
        <button
          class="button"
          @click="$router.go(-1)"
        >取消</button>
      </footer>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DataModifier from '@/mixins/data-modifier'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'

@Component({
  name: 'SupplierUpdate',
  mixins: [
    DataModifier({
      dataApi: '/suppliers/find',
      updateApi: '/suppliers/update',
      dataParams: (vm: any) => ({
        warehouse_id: vm.warehouseId,
        supplier_id: vm.supplierId,
      })
    })
  ],
  components: {
    Textarea,
    AsyncButton,
    AsyncLoader
  }
})
export default class extends Vue {
  @Prop({ required: true })
  supplierId!: number

  @Prop({ required: true })
  warehouseId!: number

  params = {
    name: '',
    comment: '',
    is_enabled: false
  }

  async handleSubmit () {
    await (this as any).handleSave()
    this.$router.go(-1)
    this.$emit('refresh')
  }
}
</script>
