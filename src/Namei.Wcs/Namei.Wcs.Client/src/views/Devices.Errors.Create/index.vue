<template>
  <div class="box">
    <p class="title is-size-5">
      添加设备
    </p>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        设备代码
      </label>

      <div class="is-flex-auto">
        <input
          v-model="device.key"
          type="text" class="input"
          style="width: 320px"
        >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        设备名称
      </label>

      <div class="is-flex-auto">
        <input
          v-model="device.name"
          type="text" class="input"
          style="width: 320px"
        >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        设备类型
      </label>

      <div class="is-flex-auto">
        <input
          v-model="device.type"
          type="text" class="input"
          style="width: 320px"
        >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
      </label>

      <AsyncButton
        class="button is-info is-small"
        :handler="handleSubmit"
      >添加</AsyncButton>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import domain from '@/providers/contexts/domain'

export default Vue.extend({
  name: 'DevicesErrorsCreate',

  data: () => ({
    device: {
      key: '',
      name: '',
      type: '',
    }
  }),

  methods: {
    async handleSubmit () {
      await domain.post('/devices/add', this.device)
      this.$router.push('/devices/errors')
      this.$emit('created')
    }
  }
})
</script>
