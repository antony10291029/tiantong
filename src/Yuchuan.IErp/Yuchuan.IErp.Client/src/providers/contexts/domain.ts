import axios from './_axios'
import { account } from '../local-storage'

const context = axios.create(
  process.env.VUE_APP_API_URL as string,
  () => account.token.get()
)

export default context
