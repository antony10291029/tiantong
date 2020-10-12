<template>
  <div
    class="is-flex"
    style="height: 110px"
  >
    <div
      class="is-flex is-flex-column"
      style="width: 100px"
    >
      <div class="is-flex-auto"></div>

      <div class="is-flex is-centered">
        <span
          v-if="hasPalletCodeB"
          class="tag is-light is-info"
        >
          {{palletCodeB}}
        </span>
        <span v-else
          class="tag is-light"
        >
          无托盘
        </span>
      </div>

      <div
        class="has-background-primary is-flex is-vcentered is-centered has-text-white"
        style="height: 30px; width: 100px"
      >
        B 段
      </div>
    </div>

    <div
      class="is-flex is-flex-column"
      style="width: 100px"
    >
      <div class="is-flex-auto"></div>

      <div class="is-flex is-centered">
        <span
          v-if="hasPalletCodeA"
          class="tag is-info"
        >
          {{palletCodeA}}
        </span>
        <span v-else
          class="tag"
        >
          无托盘
        </span>
      </div>

      <div
        class="has-background-primary is-flex is-vcentered is-centered is-radius has-text-white"
        style="height: 30px; width: 100px;"
        v-style:border-top-left-radius="0"
        v-style:border-bottom-left-radius="0"
      >
        A 段
      </div>
    </div>

    <div
      class="is-flex is-flex-column"
      style="width: 100px"
    >
      <div class="is-flex-auto"></div>

      <div
        class="is-flex is-flex-column"
        style="width: 100px; margin-left: 0.5rem"
      >
        <div class="is-flex-auto"></div>

        <div class="is-flex is-centered">
          <a
            class="tag"
            v-class:is-success="isExported"
            style="margin-right: 0.125rem"
            @click="handleTaken"
          >
            取货
          </a>
          <a
            class="tag"
            v-class:is-success="isImportAllowed"
            @click="handleImported"
          >
            放货
          </a>
        </div>

        <div class="is-flex is-centered is-vcentered" style="height: 30px">
          <span
            v-if="!isAgcRequesting"
            class="tag is-light"
          >
            AGC 请求通过
          </span>
          <a
            v-else
            @click="handleOpenDoor"
            class="tag is-info"
          >
            AGC 请求通过
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
import domain from '@/providers/contexts/domain'

export default Vue.extend({
  name: 'LifterFloor',

  props: {
    lifterId: {
      type: String,
      required: true
    },
    floor: {
      type: Number,
      required: true
    },
    floorState: {
      type: Object,
      required: true
    },
    door: {
      type: Object,
      required: true
    }
  },

  computed: { 
    isAgcRequesting () {
      return !!this.door.requestingTasks.length
    },

    isImportAllowed () {
      return this.floorState.isImportAllowed
    },

    isExported () {
      return this.floorState.isExported
    },

    palletCodeA () {
      return this.floorState.palletCodeA
    },

    palletCodeB () {
      return this.floorState.palletCodeB
    },

    hasPalletCodeA () {
      return parseInt(this.palletCodeA) && this.floorState.palletCodeA.length == 6
    },

    hasPalletCodeB () {
      return parseInt(this.palletCodeB) && this.floorState.palletCodeB.length == 6
    }
  },

  methods: {
    handleOpenDoor () {
      this.$confirm({
        title: '确认',
        content: '确认后将直接允许 AGC 通过',
        handler: () => domain.post('/doors/control', {
          command: 'open',
          door_id: this.door.id,
        }),
      })
    },

    handleImported () {
      this.$confirm({
        title: '放货',
        content: '发送放货完成信号',
        handler: () => domain.post('/lifters/imported', {
          lifterId: this.lifterId,
          floor: this.floor.toString()
        })
      })
    },

    handleTaken () {
      this.$confirm({
        title: '取货',
        content: '发送取货完成信号',
        handler: () => domain.post('/lifters/taken', {
          lifterId: this.lifterId,
          floor: this.floor.toString()
        })
      })
    }
  }
})
</script>
