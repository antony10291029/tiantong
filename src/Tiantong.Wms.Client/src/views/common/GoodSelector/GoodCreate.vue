<template>
  <div>
    <a
      class="button is-info"
      @click.prevent="handleOpen"
    >
      添加规格
    </a>
    <div
      v-show="isShow"
      class="modal is-active"
    >
      <div class="modal-background"></div>
      <div
        class="modal-card"
        style="width: 400px"
      >
        <header class="modal-card-head">
          <p class="modal-card-title">
            添加货品规格
          </p>
        </header>
        <section class="modal-card-body">
          <div class="field">
            <label class="label">货名</label>
            <div class="control">
              <input
                v-model="good_name"
                type="text" class="input"
              >
            </div>
          </div>

          <div class="field">
            <label class="label">规格</label>
            <div class="control"> 
              <input
                v-model="item_name"
                type="text" class="input"
              >
            </div>
          </div>

          <div class="field">
            <label class="label">单位</label>
            <div class="control"> 
              <input
                v-model="item_unit"
                type="text" class="input"
              >
            </div>
          </div>
        </section>
        <footer class="modal-card-foot">
          <AsyncButton
            :handler="handleSubmit"
            class="button is-success"
          >
            添加
          </AsyncButton>
          <a
            class="button"
            @click="handleClose"
          >
            取消
          </a>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Good, Item } from '@/Entities'
import axios from '../../../providers/axios'
import AsyncButton from '@/components/AsyncButton.vue'

@Component({
  name: 'GoodSelectorCreate',
  components: {
    AsyncButton
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  isShow: boolean = false

  good_name: string = ''

  item_name: string = ''

  item_unit: string = '个'

  async handleSubmit () {
    await axios.post('/goods/items/create', {
      warehouse_id: this.warehouseId,
      good_name: this.good_name,
      item_name: this.item_name,
      item_unit: this.item_unit
    })
    this.handleClose()
    this.$emit('refresh')
  }

  handleOpen () {
    this.isShow = true
    this.good_name = ''
    this.item_name = ''
    this.item_unit = '个'
  }

  handleClose () {
    this.isShow = false
  }
}
</script>
