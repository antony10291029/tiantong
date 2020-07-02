<template>
  <div style="padding: 1.25rem; width: 100%">
    <div
      class="box"
      style="width: 100%;"
    >
      <p class="label is-size-5">
        创建项目
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 80px">类型</label>

        <div class="is-flex-auto">
          <label class="label" style="margin-left: -0.25rem">
            <Radio value="true"></Radio>
            <span>垂直输送机</span>
          </label>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 80px">项目名</label>

        <div class="is-flex-auto">
          <input
            v-model="project.name"
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
        <label class="label" style="width: 80px">编号</label>

        <div class="is-flex-auto">
          <input
            v-model="project.number"
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
        <label class="label" style="width: 80px">备注</label>

        <div class="is-flex-auto">
          <div style="max-width: 640px">
            <Textarea v-model="project.comment"></Textarea>
          </div>
        </div>
      </div>

      <hr>

      <div class="is-flex">
        <div style="width: 80px"></div>
        <AsyncButton
          :handler="handleSubmit"
          class="button is-info is-small"
        >
          添加
        </AsyncButton>
      </div>

    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'CreateProject'
})
export default class extends Vue {
  project = {
    type: 'lifters',
    name: '',
    number: '',
    comment: '',    
  }

  async handleSubmit () {
    const response = await domain.post('/projects/create', this.project)
    this.$router.push(`/projects/${response.data.id}`)
    this.$emit('refresh')
  }
}
</script>
