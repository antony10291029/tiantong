import Vue from 'vue'
import axios from './axios'
import localStorage from './local-storage'

const state = Vue.observable({
  token: '',
  groups: [] as string[],
  isAuthed: false,
  isLoaded: false,
  user: {
    id: 0,
    name: '',
    email: '',
    username: '',
    phone_number: '',
  }
})

export default {

}
