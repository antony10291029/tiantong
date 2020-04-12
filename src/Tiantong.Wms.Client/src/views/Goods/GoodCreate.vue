<template>
  <GoodTemplate
    :good="good"
    :warehouseId="warehouseId"
  >
    <template #header>
      <nav class="breadcrumb is-medium">
        <ul>
          <li>
            <router-link :to="`/warehouses/${warehouseId}/goods`">
              库存
            </router-link>
          </li>
          <li class="is-active">
            <a>货品添加</a>
          </li>
        </ul>
      </nav>

      <hr>
    </template>

    <template #footer>
      <div>
        <AsyncButton
          :handler="handleSubmit"
          class="button is-info"
        >
          提交
        </AsyncButton>
      </div>
    </template>
  </GoodTemplate>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncButton from '@/components/AsyncButton.vue'
import GoodTemplate from './GoodTemplate/index.vue'
import { Good, Item } from '@/Entities'
import axios from '@/providers/axios'

@Component({
  name: 'GoodCreate',
  components: {
    AsyncButton,
    GoodTemplate,
  }
})
export default class extends Vue {
  @Prop({ required: true})
  warehouseId!: number

  good: Good = new Good(
    [new Item]
  )

  handleCancel () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    this.good.items.forEach((item, index) => item.index = index)

    try {
      await axios.post('/goods/create', this.good)
      this.handleCancel()
    } finally {}
  }

  created () {
    this.good.warehouse_id = this.warehouseId
  }
}
</script>
