namespace Tiantong.Iot
{
  public interface IStateDriverProvider
  {
    IStateDriver Resolve();

    void Connect();

    void Close();

  }

}
