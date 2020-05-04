namespace Tiantong.Iot
{
  public interface IStatePlugin
  {
    void Install<T>(State<T> state);
  }
}
