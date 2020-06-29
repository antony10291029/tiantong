<template>
  <AsyncButton
    class="button is-info"
    style="width: 80px"
    :disabled="isDisabled"
    :handler="handleClick"
  >
    <slot>发送</slot>
  </AsyncButton>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import account from '@/providers/contexts/account'

@Component({
  name: 'VerificationCode',
})
export default class extends Vue {
  @Prop({ required: true })
  address!: string

  get isDisabled () {
    return this.address.trim() === ''
  }

  async handleClick () {
    if (this.isDisabled) return

    const key = await account.sendVerificationEmail(this.address, '注册账户')

    this.$emit('change', key)
  }
}
</script>
