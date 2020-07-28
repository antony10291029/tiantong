import * as signalR from '@microsoft/signalr'

export default function (path: string) {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${process.env.VUE_APP_API_URL}/${path}`)
    .withAutomaticReconnect()
    .build()
}
