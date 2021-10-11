export interface NotifyParams {
  content: string;
  duration?: number;
}

export interface OpenNotifyParams extends NotifyParams {
  type: string;
}

export interface Notify {
  open (params: OpenNotifyParams): void
  info(params: NotifyParams): void
  link(params: NotifyParams): void
  danger(params: NotifyParams): void
  warning(params: NotifyParams): void
  success(params: NotifyParams): void
}
