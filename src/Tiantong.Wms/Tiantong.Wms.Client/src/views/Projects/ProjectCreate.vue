<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">添加工程</p>
      </header>
      <section class="modal-card-body">
        <div class="field">
          <div class="label">
            <label>工程代码</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.number" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>工程名称</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.name" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>截止日期</label>
          </div>
          <div class="control">
            <DatePicker v-model="params.due_time"></DatePicker>
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>开始日期</label>
          </div>
          <div class="control">
            <DatePicker v-model="params.started_at"></DatePicker>
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>完工日期</label>
          </div>
          <div class="control">
            <DatePicker v-model="params.finished_at" :default="false"></DatePicker>
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
          class="button is-success"
        >
          提交
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
import DatePicker from '@/components/DatePicker/index.vue'

@Component({
  name: 'ProjectCreate',
  components: {
    Textarea,
    AsyncButton,
    DatePicker
  }
})
export default class extends Vue {
  @Prop({ required: true})
  warehouseId!: number

  params = {
    name: '',
    number: '',
    comment: '',
    due_time: '',
    started_at: '',
    finished_at: '',
    warehouse_id: this.warehouseId
  }

  isPending: boolean = false

  get isChanged () {
    return this.params.number !== ''
  }

  handleCancel () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    try {
      await axios.post('/projects/create', this.params)
      this.handleCancel()
      this.$emit('refresh')
    } finally {}
  }
}
</script>
