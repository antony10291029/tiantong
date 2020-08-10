<template>
  <nav class="pagination is-centered is-rounded">
    <ul class="pagination-list">
      <li>
        <a
          @click="change(page - 1)"
          class="pagination-link"
          :disabled="page === 1"
        >
          <span class="icon">
            <i class="iconfont vue-bulma-arrow-left"></i>
          </span>
        </a>
      </li>
      <li v-for="(i, index) in pages" :key="index">
        <span
          v-if="i === 0"
          class="pagination-ellipsis"
        >&hellip;</span>
        <a
          v-else
          @click="change(i)"
          class="pagination-link"
          :class="i === page ? 'is-current' : 'has-background-white'"
        >{{i}}</a>
      </li>
      <li>
        <a
          @click="change(page + 1)"
          class="pagination-link"
          :disabled="page === lastPage"
        >
          <span class="icon">
            <i class="iconfont vue-bulma-arrow-right"></i>
          </span>
        </a>
      </li>
    </ul>
  </nav>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Pagination'
})
export default class extends Vue {
  @Prop({ required: true })
  page!: number

  @Prop({ required: true })
  pageSize!: number

  @Prop({ required: true })
  total!: number

  @Prop({ default: 7 })
  length!: number

  get lastPage () {
    return Math.max(Math.ceil(this.total / this.pageSize), 1)
  }

  get pages () {
    const N = 9
    const P = this.page
    const L = this.lastPage
    const No = L < N
    const HasLeft = P >= N / 2
    const HasRight = L - P >= N / 2 - 1

    if (this.length == 9) {
      if (No) {
        return [...new Array(L).keys()].map(i => i + 1)
      } else if (HasLeft && HasRight) {
        return [1, 0, P - 2, P - 1, P, P + 1, P + 2, 0, L]
      } else if (HasLeft && !HasRight) {
        return [1, 0, L - 6, L - 5, L - 4, L - 3, L - 2, L - 1, L]
      } else if (!HasLeft && HasRight) {
        return [1, 2, 3, 4, 5, 6, 7, 0, L]
      }
    } else if (this.length == 7) {
      if (No) {
        return [...new Array(L).keys()].map(i => i + 1)
      } else if (HasLeft && HasRight) {
        return [1, 0,P - 1, P, P + 1, 0, L]
      } else if (HasLeft && !HasRight) {
        return [1, 0, L - 4, L - 3, L - 2, L - 1, L]
      } else if (!HasLeft && HasRight) {
        return [1, 2, 3, 4, 5, 0, L]
      }
    }

  }

  change (page: number) {
    if (page !== this.page && page >= 1 && page <= this.lastPage) {
      this.$emit('change', page)
    }
  }
}
</script>
