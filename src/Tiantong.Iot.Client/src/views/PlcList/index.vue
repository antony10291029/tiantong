<template>
  <div class="is-flex">
    <aside
      class="menu has-border-right is-unselectable"
      style="min-width: 220px; max-width: 220px; height: 100%"
    >
      <ul class="menu-list">
        <li v-for="plc in plcs" :key="plc.id">
          <router-link
            :to="`/plcs/${plc.id}`"
            active-class="is-active"
          >
            <span>{{plc.name}}</span>
          </router-link>
        </li>
      </ul>
    </aside>
    <router-view
      :key="$route.params.plcId"
      baseURL="/plcs"
      class="is-flex-auto"
      style="overflow: auto"
    />
  </div>
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
  plcs: object[] = []

  async created () {
    let response = await axios.post('/plcs/all')
    this.plcs = response.data
  }
}
</script>
