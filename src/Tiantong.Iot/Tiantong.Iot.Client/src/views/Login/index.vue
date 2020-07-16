<template>
  <div>
    LoginPage
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { account } from '@/providers/local-storage'

@Component({
  name: 'LoginPage',

  beforeRouteEnter (to, from, next) {
    const path = atob(to.params.path)
    const token = to.query.token

    account.token.set(token as string)

    next(path)
  }
})
export default class extends Vue {

}
</script>
