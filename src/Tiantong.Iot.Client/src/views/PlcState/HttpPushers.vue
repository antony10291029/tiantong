<template>
  <AsyncLoader :handler="getPushers" #default="{ isPending }">
    <template v-if="!isPending">
      <HttpPusherForm
        v-for="pusher in pushers" :key="pusher.id"
        :pusher="pusher"
      >
        <template #footer>
          <div class="field">
            <div class="control">
              <AsyncButton
                class="button is-info is-small"
                style="margin-right: 0.5rem"
                :handler="() => handleSave(stateId, pusher)"
              >保存</AsyncButton>

              <AsyncButton
                class="button is-danger is-light is-small"
                :handler="() => handleDelete(stateId, pusher.id)"
              >删除</AsyncButton>
            </div>
          </div>
        </template>
      </HttpPusherForm>
    </template>

    <HttpPusherCreate
      v-if="isCreateShow"
      :stateId="stateId"
      @created="handleCreated"
    />

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <a
        @click="isCreateShow = true"
        class="button is-white has-text-dark"
        style="border: none; border-radius: 0; width: 100%"
      >
        添加推送
      </a>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import HttpPusherForm from '@/components/form/HttpPusherForm.vue'
import HttpPusherCreate from './HttpPusherCreate.vue'
import axios from '@/providers/axios'
import { HttpPusher } from '@/entities'

@Component({
  name: 'PlcStateHttpPosters',
  components: {
    HttpPusherForm,
    HttpPusherCreate
  }
})
export default class extends Vue {
  @Prop({ required: true })
  stateId!: number

  pushers: any[] = []

  isCreateShow = false

  async getPushers() {
    let response = await axios.post('/plcs/states/http-pushers/all', {
      state_id: this.stateId
    })

    this.pushers = response.data
  }

  async handleSave (state_id: number, pusher: HttpPusher) {
    await axios.post('/plcs/states/http-pushers/update', {
      state_id, pusher
    })
  }

  handleDelete (state_id: number, pusher_id: number) {
    this.$confirm({
      title: '提示',
      content: '删除后将无法恢复',
      handler: async () => {
        await axios.post('/plcs/states/http-pushers/delete', {
          state_id, pusher_id
        })

        this.getPushers()
      }
    })
  }

  handleCreated () {
    this.isCreateShow = false
    this.getPushers()
  }
}
</script>
