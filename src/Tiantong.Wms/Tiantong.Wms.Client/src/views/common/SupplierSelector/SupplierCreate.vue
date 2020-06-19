<template>
  <div>
    <a
      class="button is-info"
      @click="isShow = true"
    >添加供应商</a>
    <div v-if="isShow" class="modal is-active">
      <div
        @click="handleClose"
        class="modal-background"
      ></div>
      <div class="modal-card" style="width: 440px">
        <header class="modal-card-head">
          <p class="modal-card-title">
            添加供应商
          </p>
        </header>
        <section class="modal-card-body">
          <div class="field">
            <label class="label">供应商名称</label>
            <div class="control">
              <input
                v-model.lazy="supplier.name"
                type="text" class="input"
              >
            </div>
          </div>
          <div class="field">
            <label class="label">备注</label>
            <div class="control">
              <Textarea
                v-model="supplier.comment"
                type="text" class="input"
              ></Textarea>
            </div>
          </div>
        </section>
        <footer class="modal-card-foot">
          <AsyncButton
            class="button is-success"
            :handler="handleSubmit"
          >添加</AsyncButton>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'
import { Supplier } from '@/Entities'
import Textarea from '@/components/Textarea.vue'

@Component({
  name: 'SupplierCreate',
  components: {
    AsyncButton,
    Textarea
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  isShow: boolean = false

  supplier: Supplier = new Supplier()

  handleClose () {
    this.isShow = false
  }

  async handleSubmit () {
    this.supplier.warehouse_id = this.warehouseId
    await axios.post('/suppliers/create', this.supplier)
    this.handleClose()
    this.$emit('refresh')
    this.supplier = new Supplier()
  }
}
</script>
