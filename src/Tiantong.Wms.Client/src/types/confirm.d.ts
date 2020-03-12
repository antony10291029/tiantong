import Vue from 'vue'

interface ConfirmOptions {
  title: string,
  content: string,
  handler: string,
  beforeClose: Function,
  width: string
}

declare module 'vue/types/vue' {
  interface Vue {
    $confirm (options: ConfirmOptions) : void
  }
}
