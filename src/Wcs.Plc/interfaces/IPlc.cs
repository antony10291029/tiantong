namespace Wcs.Plc
{
  public interface IPlc : IPlcWorker
  {
    PlcContainer Container { get; }
  }
}
