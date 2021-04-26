import axios from './_axios'

const url = 'http://' + window.location.hostname + ":5100"

const domain = axios.create(url)

export default domain;
