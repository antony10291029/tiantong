<template>
  <a
    @click="handleItemClick"
    @mouseout="isHovered = false"
    @mouseover="isHovered = true"
    class="system-item"
    v-active="false"
  >
    <span
      class="icon is-medium"
      :class="{ 'has-text-info': isHovered }">
      <i class="iconfont icon-warehouse"></i>
    </span>

    <p
      class="is-flex is-flex-column"
      style="flex: 2; margin-left: 0.75rem"
    >
      <span
        class="is-size-5"
        v-if="warehouse.name"
      >
        {{warehouse.name}}
      </span>
      <i v-else>[ 未命名 ]</i>
      <span style="height: 0.1rem"></span>
      <span
        v-if="warehouse.comment"
        class="is-size-7"
      >
        {{warehouse.comment}}
      </span>
    </p>
    <div>
      <a
        v-show="isHovered"
        class="icon is-medium has-text-info"
      >
        <i class="iconfont icon-enter"></i>
      </a>
    </div>
  </a>
</template>

<script>
import axios from '@/providers/axios'

export default {
  name: 'WarehouseItem',
  props: {
    warehouse: Object,
  },
  data: () => ({
    isHovered: false,
  }),
  computed: {

  },
  methods: {
    handlePlayClick () {
      const params = {
        hoister_id: this.warehouse.id
      }
      if (this.warehouse.is_running) {
        this.$confirm({
          title: '操作',
          content:  '停止设备后台程序',
          handler: () => axios.post('hoisters/stop', params)
            .then(() => { this.warehouse.is_running = false })
        })
      } else {
        this.$confirm({
          title: '操作',
          content: '启动设备后台程序',
          handler: () => axios.post('hoisters/run', params)
            .then(() => this.warehouse.is_running = true)
        })
      }
    },
    handleDestroyClick () {
      this.$confirm({
        type: 'danger',
        title: '警告',
        content: '删除设备后将无法恢复',
        confirmText: '删除',
        handler: () => axios.post('/hoisters/delete', { hoister_id: this.warehouse.id })
          .then(() => {
            this.$emit('refresh')
            this.$message('设备已删除')
          })
      })
    },
    async handleItemClick () {
      this.isHovered = false
      this.$router.push(`/warehouses/${this.warehouse.id}`)
    },
  }
}
</script>
