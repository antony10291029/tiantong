export default class Invoice {
  id: number = 0

  name: string = ''

  specification: string = ''

  unit: string = ''

  quantity: number = 0

  price: number = 0

  amount: number = 0

  tax_rate: number = 13

  tax_amount: number = 0

  invoice_number: string = ''

  invoice_type: string = '增值税专用发票'

}
