import Vue from 'vue'

declare module 'vue/types/vue' {
  interface Vue {
    $notify: {
      add (text: string, type?: string, duration?: number): void
      info (text: string, duration?: number): void
      link (text: string, duration?: number): void
      danger (text: string, duration?: number): void
      success (text: string, duration?: number): void
      warning (text: string, duration?: number): void
    } 
  }
}
