export default {
  minValue: '0001-01-01T00:00:00',

  get now () {
    return new Date().toISOString()
  },

  getDate (dateTime: string) {
    return dateTime.substring(0, 10)
  },

}
