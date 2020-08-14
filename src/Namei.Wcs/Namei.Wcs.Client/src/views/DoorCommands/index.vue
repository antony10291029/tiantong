<template>
  <AsyncLoader
    :handler="getDoors"
    style="padding: 1.25rem"
  >
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading">
            设备名称
          </div>

          <router-link
            v-for="door in doors" :key="door.id"
            class="panel-block"
            :to="`/doors/commands/${door.id}`"
          >
            {{door.type}} - {{door.id}}
          </router-link>
        </div>
      </div>

      <div class="column">
        <router-view></router-view>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'DoorCommands'
})
export default class extends Vue {
  doors: any[] = []

  async getDoors () {
    const response = await domain.post('/doors/states')

    this.doors = response.data
  }
}
</script>
