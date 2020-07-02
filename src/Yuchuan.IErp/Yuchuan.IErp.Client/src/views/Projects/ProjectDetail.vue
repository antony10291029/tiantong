<template>
  <AsyncLoader
    :handler="getProject"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="box">
      <p class="label is-size-5">
        项目信息
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 80px">ID</label>

        <div class="is-flex-auto">
          {{project.id}}
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 80px">名称</label>

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

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 80px">开启</label>

        <div class="is-flex-auto">
          <Switcher v-model="project.is_enabled"></Switcher>
        </div>
      </div>

      <hr>

      <div class="is-flex">
        <div style="width: 80px"></div>
        <AsyncButton
          :handler="handleSave"
          class="button is-info is-small"
        >
          保存
        </AsyncButton>

        <span style="width: 0.5rem"></span>

        <AsyncButton
          :handler="handleDelete"
          class="button is-danger is-small is-light"
        >
          删除
        </AsyncButton>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'ProjectDetail'
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  projectId!: number

  project: any = {}

  async getProject () {
    const response = await domain.post('/projects/find', {
      id: this.projectId
    })

    this.project = response.data
  }

  async handleSave () {
    const response = await domain.post('/projects/update', this.project)
    await this.getProject()

    this.$emit('refresh')
  }

  handleDelete () {
    this.$confirm({
      title: '删除项目',
      content: '删除后将无法恢复',
      handler: async () => {
        const response = await domain.post('/projects/delete', {
          id: this.projectId
        })

        await this.$emit('refresh')
        this.$router.push(this.baseURL)
      }
    })
  }
}
</script>
