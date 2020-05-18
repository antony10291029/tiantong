import Vue from 'vue'

declare module 'vue/types/vue' {
  interface Vue {
    $confirmAdminPassword(handler: (password: string) => {}): void
  }
}
