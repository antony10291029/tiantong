<template>
  <div>never</div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import localStorage from '@/providers/local-storage'

@Component({
  name: 'ChanjetHome',
  beforeRouteEnter (to, from, next) {
    const orgCode = localStorage.chanjet.orgCode.get()
    const bookCode = localStorage.chanjet.bookCode.get()

    if (orgCode && bookCode) {
      next(`/chanjet-hkj/${orgCode}/${bookCode}`)
    } else {
      next(`/chanjet-login`)
    }
  }
})
export default class extends Vue {

}
</script>
