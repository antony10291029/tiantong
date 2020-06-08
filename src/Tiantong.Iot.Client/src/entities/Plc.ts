import DateTime from '@/utils/DateTime'

export class Plc {
  id = 0
  name = ''
  model = PlcModel.mc3eBinary
  host = '127.0.0.1'
  port = 8000
  comment = ''
  created_at = DateTime.now
}

export enum PlcModel {
  mc3eBinary = 'mc3e-binary',
  s7200Smart = 's7200smart',
  test = 'test',
}
