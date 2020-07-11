import axios from './_axios'

let context = axios.create('https://iaccount.als-yuchuan.com')

export default {
  async sendVerificationEmail (address: string, subject: string, duration: number = 300) {
    const response = await context.post('/email-verifications/send', {
      address, subject, duration
    })

    return response.data.key
  },
}
