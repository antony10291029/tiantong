using System.IO;
using System.Threading.Tasks;

namespace Namei.Wcs.Aggregates
{
  public interface ILifterCommandService
  {
    void Import(string lifterId, string floor, string barcode, string destination);

    void SetDestination(string lifterId, string floor, string destination);

    void SetTaken(string lifterId, string floor);

    string GetBarcode(string lifterId, string floor);

    string GetTaskDestination(string lifterId, string floor);

    void Clear(string lifterId, string floor);

    bool IsBarcodeValid(string barcode);

    bool IsDestinationValid(string destination);
  }

  public class LifterCommandService: ILifterCommandService
  {
    private readonly FirstLifterCommand _lifter1;

    private readonly SecondLifterCommand _lifter2;

    private readonly ThirdLifterCommand _lifter3;

    public LifterCommandService(
      FirstLifterCommand lifter1,
      SecondLifterCommand lifter2,
      ThirdLifterCommand lifter3
    ) {
      _lifter1 = lifter1;
      _lifter2 = lifter2;
      _lifter3 = lifter3;
    }

    public ILifterCommand GetLifter(string lifterId) => lifterId switch {
      "1" => _lifter1,
      "2" => _lifter2,
      "3" => _lifter3,
      _ => throw new InvalidDataException($"提升机编号不存在: {lifterId}")
    };

    public void Import(string lifterId, string floor, string barcode, string destination)
      => GetLifter(lifterId).Import(
        floor,
        IsBarcodeValid(barcode) ? barcode : "0",
        IsDestinationValid(destination) ? destination : "0"
      );

    public void SetDestination(string lifterId, string floor, string destination)
      => GetLifter(lifterId).SetDestination(floor, destination);

    public void SetTaken(string lifterId, string floor)
    {
      var lifter = GetLifter(lifterId);

      GetLifter(lifterId).SetTaken(floor, true);

      Task.Delay(2000).ContinueWith(async _ => {
        await _;
        lifter.SetTaken(floor, false);
      });
    }

    public void Clear(string lifterId, string floor)
    {
      var lifter = GetLifter(lifterId);

      lifter.SetImported(floor, false);
      lifter.SetDestination(floor, "0");
      lifter.SetBarcode(floor, "0");
    }

    public string GetBarcode(string lifterId, string floor)
      => GetLifter(lifterId).GetBarcode(floor);

    public string GetTaskDestination(string lifterId, string floor)
      => GetLifter(lifterId).GetTaskDestination(floor);

    public bool IsBarcodeValid(string barcode)
      => barcode != null && barcode.Length == 6 && int.TryParse(barcode, out _);

    public bool IsDestinationValid(string dest)
      => dest == "1" || dest == "2" || dest == "3" || dest == "4";
  }
}
