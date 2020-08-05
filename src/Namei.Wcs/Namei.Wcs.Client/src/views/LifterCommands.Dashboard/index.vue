<template>
  <div>
    <div class="box">
      <p class="title is-size-5">
        指令面板
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          目标楼层
        </div>

        <div class="field has-addons">
          <div
            v-for="key in 4" :key="key"
            class="control"
          >
            <a
              class="button"
              @click="setDestination(key.toString())"
              v-class:is-info="key == destination"
            >
              {{key}} 楼
            </a>
          </div>
        </div>

      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          请求开门
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'requested.open')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 请求开门
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          请求关门
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'requested.close')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 请求关门
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          放货完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'imported')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 放货完成
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          扫码完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'scanned')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 扫码完成
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          请求取货
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'exported')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 请求取货
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          取货完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(key.toString(), 'taken')"
            class="button is-info"
            v-for="key in 4" :key="key"
          >
            {{key}}F - 取货完成
          </a>
        </div>
      </div>

      <hr>

    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'LiftersDebugView'
})
export default class extends Vue {
  @Prop({ required: true })
  lifterId!: string

  destination = '1'

  async getDestination () {
    const response = await domain.post('/test/lifters/destination')

    this.destination = response.data.destination
  }

  async setDestination (value: number) {
    await domain.post('/test/lifters/set-destination', {
      destination: value
    })
    await this.getDestination()
  }

  async publishMessage (floor: string, message: string) {
    await domain.post('/test/lifters/publish-message', {
      lifter_id: this.lifterId,
      floor: floor,
      message: message,
    })
  }

  async created () {
    this.getDestination()
  }
}
</script>
