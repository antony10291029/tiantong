export interface IConfirmOptions {
  title: string;
  content: string;
  handler?: () => {};
  beforeClose?: () => {};
  width?: string;
}

export interface IConfirm {
  open: (options: IConfirmOptions) => void;
}
