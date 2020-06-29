<template>
  <AsyncLoader
    :handler="getProfile"
    style="padding: 1.25rem"
  >
    <div class="box" style="width: 640px;">
      <div>
        <p class="label is-size-5 mg-0">个人资料</p>
      </div>
      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.7rem 0"
      >
        <label class="label mb-0" style="width: 80px">
          邮箱
        </label>

        <span>{{profile.email}}</span>
      </div>

      <hr>

      <NameField
        :name="profile.name"
        :refresh="getProfile"
      />

      <hr>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import NameField from './NameField.vue'
import account from '@/providers/contexts/account'

@Component({
  name: 'UserProfile',
  components: {
    NameField
  }
})
export default class extends Vue {
  profile = {
    id: 0,
    name: '',
    email: ''
  }

  async getProfile () {
    this.profile = await account.getProfile()
  }

  created () {
    this.getProfile()
  }
}
</script>
