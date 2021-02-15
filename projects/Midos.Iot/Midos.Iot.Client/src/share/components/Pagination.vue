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
import { defineComponent, computed } from "vue";

export default defineComponent({
  name: "Pagination",

  props: {
    page: {
      type: Number,
      required: true,
    },

    pageSize: {
      type: Number,
      required: true
    },

    total: {
      type: Number,
      required: true
    },

    length: {
      type: Number,
      default: 7
    }
  },

  setup(props, { emit }) {
    const lastPage = computed(() => Math.max(Math.ceil(props.total / props.pageSize), 1));
    const pages = computed((): number[] => {
      const N = 9;
      const P = props.page;
      const L = lastPage.value;
      const No = L < N;
      const HasLeft = P >= N / 2;
      const HasRight = L - P >= N / 2 - 1;

      if (props.length === 9) {
        if (No) {
          return [...new Array(L).keys()].map(i => i + 1);
        } if (HasLeft && HasRight) {
          return [1, 0, P - 2, P - 1, P, P + 1, P + 2, 0, L];
        } if (HasLeft && !HasRight) {
          return [1, 0, L - 6, L - 5, L - 4, L - 3, L - 2, L - 1, L];
        } if (!HasLeft && HasRight) {
          return [1, 2, 3, 4, 5, 6, 7, 0, L];
        }
      } else if (props.length === 7) {
        if (No) {
          return [...new Array(L).keys()].map(i => i + 1);
        } if (HasLeft && HasRight) {
          return [1, 0, P - 1, P, P + 1, 0, L];
        } if (HasLeft && !HasRight) {
          return [1, 0, L - 4, L - 3, L - 2, L - 1, L];
        } if (!HasLeft && HasRight) {
          return [1, 2, 3, 4, 5, 0, L];
        }
      }

      return [];
    });

    function change(page: number) {
      if (page !== props.page && page >= 1 && page <= lastPage.value) {
        emit("change", page);
      }
    }

    return {
      lastPage,
      pages,
      change
    };
  }

});
</script>
