import Vue from 'vue'

interface ConfirmOptions {

}

declare module 'vue/types/vue' {
  interface Vue {
    $confirm (params: {
      title?: string,
      content?: string,
      handler?: Function,
      beforeClose?: Function,
      width?: string | number
    }) : void
  }
}
