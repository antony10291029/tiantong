import axios from './_axios'

const domain = axios.create(process.env.VUE_APP_API_URL)

export default domain;
