<template>
  <div class="box">
    <p class="title is-size-6">
      指令面板
    </p>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 1.25rem 0"
    >
      <p class="label" style="width: 120px">
        控制指令
      </p>

      <div class="buttons">
        <a
          @click="publishMessage('requested.open')"
          class="button is-info"
        >
          请求开门
        </a>

        <a
          @click="publishMessage('requested.close')"
          class="button is-info"
        >
          请求关门
        </a>

        <a
          @click="publishMessage('opened')"
          class="button is-info"
        >
          开门完毕
        </a>

        <a
          @click="publishMessage('closed')"
          class="button is-info"
        >
          关门完毕
        </a>
      </div>
    </div>

    <hr>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'DoorCommandsDashboard'
})
export default class extends Vue {
  @Prop({ required: true })
  doorId!: string

  async publishMessage (message: string) {
    await domain.post('/test/doors/publish-message', {
      door_id: this.doorId,
      message: message,
    })
  }
}
</script>
