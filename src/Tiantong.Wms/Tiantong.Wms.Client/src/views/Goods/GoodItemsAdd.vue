<template>
  <div class="modal is-active">
    <div
      class="modal-background"
      @click="handleCancel"
    ></div>
    <div class="modal-content">
      <div class="modal-card" style="width: 480px">
        <header class="modal-card-head">
          <h1 class="modal-card-title">
            添加规格
          </h1>
        </header>
        <section class="modal-card-body">
          <div class="field">
            <label class="label">规格码</label>
            <div class="control">
              <input
                type="text" class="input"
                v-model="params.number"
              >
            </div>
          </div>
          <div class="field">
            <label class="label">规格名</label>
            <div class="control">
              <input
                type="text" class="input"
                v-model="params.name"
              >
            </div>
          </div>
          <div class="field">
            <label class="label">单位</label>
            <div class="control">
              <input
                type="text" class="input"
                v-model="params.unit"
              >
            </div>
          </div>
        </section>
        <footer class="modal-card-foot">
          <AsyncButton
            class="button is-success"
            :handler="handleSubmit"
          >提交</AsyncButton>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'

@Component({
  name: 'GoodItemsAdd',
  components: {
    AsyncButton
  }
})
export default class extends Vue {
  @Prop({ required: true })
  goodId!: number

  params: any = {
    number: null,
    name: '',
    unit: '个'
  }

  handleCancel () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    this.params.good_id = this.goodId
    var response = await axios.post('/items/create', this.params)
    this.params.id = response.data.id
    this.$emit('add-item', this.params)
    this.handleCancel()
  }
}
</script>
