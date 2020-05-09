<template>
  <AsyncLoader :handler="getState">
    <PlcStateForm
      :handler="getState"
      :state="state"
    />

    <div style="height: 0.75rem"></div>

    <div>
      <AsyncButton
        :handler="handleSave"
        class="button is-info is-small"
        style="margin-right: 0.5rem"
      >
        保存
      </AsyncButton>

      <AsyncButton
        :handler="handleDelete"
        class="button is-danger is-light is-small"
      >
        删除
      </AsyncButton>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import PlcStateForm from '@/components/form/PlcStateForm.vue'

@Component({
  name: 'StateUpdate',
  components: {
    PlcStateForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  stateId!: object

  state: any = null

  async getState () {
    let response = await axios.post('/plcs/states/find', {
      state_id: this.stateId
    })

    this.state = response.data
  }

  async handleSave () {
    await axios.post('/plcs/states/update', this.state)
    this.$emit('refresh')
  }

  handleDelete () {
    this.$confirm({
      title: '提示',
      content: '删除后将无法恢复',
      handler: async () => {
        await axios.post('/plcs/states/delete', {
          state_id: this.stateId
        })

        this.$emit('delete')
      }
    })

  }
}
</script>
