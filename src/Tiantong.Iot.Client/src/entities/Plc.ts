import DateTime from '@/utils/DateTime'

export class Plc {
  id = 0
  name = ''
  model = 'test'
  host = '127.0.0.1'
  port = 8000
  comment = ''
  created_at = DateTime.now
}

export enum PlcModel {
  test = 'test',
  mc3e = 'mc3e',
  s7200Smart = 's7200smart'
}
