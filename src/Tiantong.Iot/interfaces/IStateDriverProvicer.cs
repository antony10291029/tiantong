namespace Tiantong.Iot
{
  public interface IStateDriverProvider
  {
    IStateDriver Resolve();

    void Boot();

    void Stop();
  }
}
