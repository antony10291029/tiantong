export interface IConfirmOptions {
  title: string;
  content: string;
  handler?: () => {};
  beforeClose?: () => {};
  width?: string;
}
