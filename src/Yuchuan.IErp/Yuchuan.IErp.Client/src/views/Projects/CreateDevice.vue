<template>
  <div class="box">
    <p class="title is-size-5">
      添加设备
    </p>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <div class="label mb-0" style="width: 80px">
        设备名
      </div>

      <div class="is-flex-auto">
        <input
          v-model="params.name"
          type="text" class="input"
          style="width: 320px"
        >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <div class="label mb-0" style="width: 80px">
        编号
      </div>

      <div class="is-flex-auto">
        <input
          v-model="params.number"
          type="text" class="input"
          style="width: 320px"
        >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <div class="label mb-0" style="width: 80px">
        备注
      </div>

      <div class="is-flex-auto">
        <div style="width: 640px">
          <Textarea v-model="params.comment"></Textarea>
        </div>
      </div>
    </div>

    <hr>

    <div class="is-flex">
      <div style="width: 80px"></div>

      <div class="is-flex-auto">
        <AsyncButton
          class="button is-info is-small"
          :handler="handleSubmit"
        >
          提交
        </AsyncButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'CreateDevice'  
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  projectId!: number

  params = {
    name: '',
    number: '',
    comment: '',
  }

  async handleSubmit () {
    const response = await domain.post('/devices/create', {
      project_id: this.projectId,
      device: this.params
    })

    this.$emit('refresh')
    this.$router.push(`${this.baseURL}/${response.data.id}`)
  }
}
</script>
