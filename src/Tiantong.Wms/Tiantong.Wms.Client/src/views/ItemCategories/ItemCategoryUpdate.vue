<template>
  <AsyncLoader class="modal is-active" :handler="getData">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">供应商编辑</p>
      </header>

      <section class="modal-card-body">
        <div class="field">
          <div class="label">
            <label>货类名称</label>
          </div>
          <div class="control">
            <input v-model="params.name" type="text" class="input">
          </div>
        </div>
        <div class="field">
          <div class="label">
            <label>货类编码</label>
          </div>
          <div class="control">
            <input v-model="params.number" type="text" class="input">
          </div>
        </div>
        <div class="field">
          <div class="label">
            <label>备注</label>
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
          class="button is-success"
        >
          提交
        </AsyncButton>
        <button
          class="button"
          @click="handleClose"
        >
          取消
        </button>
        <div class="is-flex-auto"></div>
        <button
          @click="handleRemove"
          class="button is-danger is-float-right"
        >
          删除
        </button>
      </footer>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import DataModifier from '@/mixins/data-modifier'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'

@Component({
  name: 'ItemCategoryUpdate',
  mixins: [
    DataModifier({
      dataApi: '/item-categories/find',
      updateApi: '/item-categories/update',
      dataParams: (vm: any) => ({
        warehouse_id: vm.warehouseId,
        category_id: vm.categoryId,
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
  categoryId!: number

  @Prop({ required: true })
  warehouseId!: number

  params = {
    name: '',
    number: '',
    comment: '',
    is_enabled: false
  }

  handleClose () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    await (this as any).handleSave()
    this.handleClose()
    this.$emit('refresh')
  }

  async handleRemove () {
    this.$confirm({
      width: 400,
      title: '确认删除',
      content: '货类删除后将无法恢复，若货类已经被使用则无法删除',
      handler: async () => {
        try {
          await axios.post('/item-categories/delete', { category_id: this.categoryId })
          this.handleClose()
          this.$emit('refresh')
        } catch (error) {
          throw error
        }
      }
    })
  }
}
</script>
