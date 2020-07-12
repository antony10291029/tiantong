<template>
  <div>
    <HttpPusherForm :pusher="pusher">
      <template #footer>
        <div class="is-flex" style="padding: 0.75rem 0">
          <div style="width: 100px"></div>
          <AsyncButton
            :handler="handleSubmit"
            class="button is-info is-small"
            style="margin-right: 0.5rem"
          >
            提交
          </AsyncButton>
          <a
            @click="$emit('close')"
            class="button is-info is-light is-small"
          >
            关闭
          </a>
        </div>

        <div class="has-border-bottom" style="margin: 1.25rem -1.25rem"></div>
      </template>
    </HttpPusherForm>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import HttpPusherForm from '@/components/form/HttpPusherForm.vue'
import { HttpPusher } from '@/entities'
import axios from '@/providers/axios'

@Component({
  name: 'PlcStateHttpPusherCreate',
  components: {
    HttpPusherForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  stateId!: number

  pusher = new HttpPusher()

  async handleSubmit () {
    await axios.post('/plcs/states/http-pushers/create', {
      state_id: this.stateId,
      pusher: this.pusher
    })

    this.$emit('created')
  }
}
</script>
