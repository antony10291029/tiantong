<template>
  <div
    class="panel-block"
    style="height: 60px"
  >
      <div class="label"># {{door.id}}</div>

      <div class="is-flex-auto"></div>

      <span
        v-if="door.isError"
        class="tag is-danger is-light"
        style="margin-right: 0.5rem"
      >
        <span>异常</span>
      </span>

      <span
        class="tag is-light"
        style="margin-right: 0.5rem"
        v-class:is-info="isRequesting"
      >
        <span v-if="isRequesting">请求中</span>
        <span v-else>无请求</span>
      </span>

      <a
        v-if="isOpened"
        @click="handleClose"
        class="tag is-light is-info"
      >
        关门
      </a>

      <a
        v-else
        @click="handleOpen"
        class="tag is-light"
      >
        开门
      </a>
  </div>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'
import domain from '@/providers/contexts/domain'

export default Vue.extend({
  name: 'Door',

  props: {
    door: {
      type: Object as PropType<any>,
      required: true
    }
  },

  computed: {
    isRequesting () {
      return (this.door as any ).requestingTasks.length !== 0
    },

    isOpened () {
      return (this.door as any).isOpened
    }
  },

  methods: {
    async handleOpen () {
      this.$confirm({
        title: '开门',
        content: '手动执行关门指令',
        handler: async () => await domain.post('/doors/control', {
          door_id: (this as any).door.id,
          command: 'open'
        })
      })
    },

    async handleClose () {
      this.$confirm({
        title: '关门',
        content: '手动执行关门指令',
        handler: async () => await domain.post('/doors/control', {
          door_id: (this as any).door.id,
          command: 'close'
        })
      })
    }
  }
})
</script>
