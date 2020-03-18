<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">添加供应商</p>
      </header>
      <section class="modal-card-body">
        <div class="field">
          <div class="label">
            <label>供应商名称</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.name" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>备注</label>
          </div>
          <div class="control">
            <Textarea v-model="params.comment"></Textarea>
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
          @click="handleCancel"
        >
          取消
        </button>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'

@Component({
  name: 'SupplierCreate',
  components: {
    Textarea,
    AsyncButton
  }
})
export default class extends Vue {
  @Prop({ required: true})
  warehouseId!: number

  params = {
    name: '',
    comment: '',
    warehouse_id: this.warehouseId
  }

  isPending: boolean = false

  get isChanged () {
    return this.params.name !== '' || this.params.comment !== ''
  }

  handleCancel () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    if (!this.isChanged) return

    try {
      await axios.post('/suppliers/create', this.params)
      this.handleCancel()
      this.$emit('refresh')
    } catch (Error) {
      throw Error
    }
  }
}
</script>
