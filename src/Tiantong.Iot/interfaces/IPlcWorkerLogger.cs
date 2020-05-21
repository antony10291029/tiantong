namespace Tiantong.Iot
{
  public interface IPlcWorkerLogger: ILogger
  {
    void Log(int id, string message);

    void Dispose();
  }
}
