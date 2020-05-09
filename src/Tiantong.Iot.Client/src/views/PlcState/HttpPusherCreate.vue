<template>
  <div>
    <HttpPusherForm :pusher="pusher">
      <template #footer>
        <AsyncButton
          :handler="handleSubmit"
          class="button is-info is-small"
        >
          提交
        </AsyncButton>
      </template>
    </HttpPusherForm>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import HttpPusherForm from '@/components/form/HttpPusherForm.vue'
import { HttpPusher } from '../../entities'
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
