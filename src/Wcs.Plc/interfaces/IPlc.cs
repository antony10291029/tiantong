namespace Wcs.Plc
{
  public interface IPlc : IPlcWorker
  {
    IPlcContainer Container { get; }
  }
}
