<template>
  <div class="box" style="min-height: 400px">
    <p class="title is-size-5">
      调试选项
    </p>

    <hr>

    <div class="is-flex is-vcentered">
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
            @click="setDestination(key)"
            v-class:is-info="key == destination"
          >
            {{key}} 楼
          </a>
        </div>
      </div>

    </div>

    <hr>

    <div class="is-flex is-vcentered">
      <div
        class="label"
        style="width: 100px"
      >
        放货指令
      </div>

      <div class="buttons">
        <a
          @click="handleImport(key.toString())"
          class="button is-info"
          v-for="key in 4" :key="key"
        >
          {{key}}F - 放货完成
        </a>
      </div>
    </div>

    <hr>

    <div class="is-flex is-vcentered">
      <div
        class="label"
        style="width: 100px"
      >
        取货指令
      </div>

      <div class="buttons">
        <a
          @click="handleExport(key.toString())"
          class="button is-info"
          v-for="key in 4" :key="key"
        >
          {{key}}F - 取货完成
        </a>
      </div>
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
  lifterId!: number

  destination = 1

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

  async handleImport (floor: string) {
    await domain.post('/test/lifters/import', {
      lifter_id: this.lifterId,
      floor: floor
    })
  }

  async handleExport (floor: string) {
    await domain.post('/test/lifters/export', {
      lifter_id: this.lifterId,
      floor: floor
    })
  }

  async created () {
    this.getDestination()
  }
}
</script>
