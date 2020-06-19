<template>
  <div>
    <a
      :disabled="disabled"
      class="button is-success"
      @click="!disabled && (isShow = true)"
    >
      <slot></slot>
    </a>
    <AsyncLoader
      v-if="isShow"
      :handler="getLocations"
      class="modal is-active"
    >
      <div
        @click="handleClose"
        class="modal-background"
      ></div>
      <div
        class="modal-card"
        style="width: 480px"
      >
        <header class="modal-card-head">
          <p class="modal-card-title">
            选择货位
          </p>
        </header>
        <section class="modal-card-body">
          <table class="table is-hoverable is-bordered is-fullwidth">
            <thead>
              <th>#</th>
              <th>货位</th>
              <th>备注</th>
              <th style="width: 1px">
                <span class="icon" style="color: transparent">
                  <i class="iconfont icon-tick"></i>
                </span>
              </th>
            </thead>
            <tbody>
              <tr
                v-for="(location, index) in locations"
                @click="locationId = location.id"
                :key="location.id"
              >
                <td>{{index + 1}}</td>
                <td>{{location.name}}</td>
                <td>{{location.comment}}</td>
                <td>
                  <span
                    v-if="locationId == location.id"
                    class="icon has-text-success"
                  >
                    <i class="iconfont icon-tick"></i>
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
        <footer class="modal-card-foot">
          <AsyncButton
            :disabled="locationId === 0"
            class="button is-success"
            :handler="handleSubmit"
          >提交</AsyncButton>
        </footer>
      </div>
    </AsyncLoader>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/providers/axios'
import { Location } from '@/Entities'
import AsyncLoader from '@/components/AsyncLoader.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import OrderEntity from './OrderEntity'

export default Vue.extend({
  components: {
    AsyncButton,
    AsyncLoader
  },

  props: {
    warehouseId: {
      type: Number,
      required: true
    },

    orderId: {
      type: Number,
      required: true
    },

    entity: {
      type: Object,
      required: true
    },

    disabled: {
      type: Boolean,
      required: true
    }
  },

  data: () => ({
    isShow: false,
    entities: {
      keys: [] as number[],
      data: {} as { [ key: string ]: Location }
    },
    locationId: 0
  }),

  computed: {
    locations () {
      let { keys, data } = this.entities

      return keys.map(key => data[key])
    }
  },

  methods: {
    handleClose () {
      this.isShow = false
    },

    handleSelect (location: Location) {
      this.locationId = location.id
    },

    async getLocations () {
      let response = await axios.post('/locations/all', {
        warehouse_id: this.warehouseId
      })

      this.entities = response.data
    },

    handleSubmit () {
      this.$confirm({
        width: '360px',
        title: '提示',
        content: `提交后，订单将不可编辑或删除`,
        handler: async () => {
          await this.entity.finish(this.locationId)
          this.isShow = false
          this.$emit('finished')
        }
      })
    }
  }
})
</script>
