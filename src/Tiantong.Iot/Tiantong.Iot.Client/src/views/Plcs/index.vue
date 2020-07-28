<template>
  <AsyncLoader
    class="is-flex"
    :handler="getPlcs"
  >
    <aside
      class="menu is-unselectable"
      style="height: 100%; overflow-y: auto; min-width: 260px"
    >
      <ul class="menu-list">
        <li>
          <router-link
            :to="`/plcs`"
            active-class="none"
            exact-active-class="is-active"
          >
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-manage"></i>
            </span>
            <span>服务管理</span>
          </router-link>
        </li>
        <li v-for="id in plcs.result" :key="id">
          <router-link :to="`/plcs/${id}`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <span class="iconfont icon-device"></span>
            </span>
            <span>{{plcs.data[id].name}}</span>
          </router-link>
        </li>
        <li>
          <router-link :to="`/plcs/create`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-add"></i>
            </span>
            <span>添加设备</span>
          </router-link>
        </li>
      </ul>
    </aside>
    <router-view
      :key="$route.params.plcId"
      :plc="plcs.data[$route.params.plcId]"
      baseURL="/plcs"
      class="is-flex-auto"
      @refresh="getPlcs"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcList',
  components: {

  }
})
export default class extends Vue {
  plcs = {
    result: [] as any[],
    data: {} as any
  }

  async getPlcs () {
    const response = await axios.post('/plcs/all')
    const result = [] as any
    const data = {} as any

    response.data.forEach((plc: any) => {
      result.push(plc.id)
      data[plc.id] = plc
    })

    this.plcs = { result, data }
  }
}
</script>
