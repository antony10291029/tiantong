<template>
  <AsyncLoader :handler="getState">
    <PlcStateForm
      :handler="getState"
      :state="state"
    />

    <div class="is-flex" style="padding: 0.75rem 0">
      <div style="width: 100px"></div>

      <AsyncButton
        :handler="handleSave"
        :disabled="!isChanged"
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
import PlcStateForm from '@/components/form/PlcStateForm.vue'
import isEqual from 'lodash/isEqual'
import cloneDeep from 'lodash/cloneDeep'
import axios from '@/providers/axios'
import { PlcState } from '@/entities'

@Component({
  name: 'StateUpdate',
  components: {
    PlcStateForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  stateId!: object

  state = new PlcState()

  sourceData = new PlcState()

  get isChanged () {
    return !isEqual(this.state, this.sourceData)
  }

  async getState () {
    let response = await axios.post('/plcs/states/find', {
      state_id: this.stateId
    })

    this.state = response.data
    this.sourceData = cloneDeep(this.state)
  }

  async handleSave () {
    await axios.post('/plcs/states/update', this.state)
    this.$emit('refresh')
    this.getState()
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
