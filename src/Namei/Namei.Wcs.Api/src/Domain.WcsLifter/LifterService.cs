namespace Namei.Wcs.Aggregates
{
  public interface ILifterService
  {
    void HandleImported(
      string lifterId, string floor, string barcode = "",
      string destination = "", string data = "", string from = ""
    );

    void HandleScanned(string lifterId, string floor);

    void HandleExported(string lifterId, string floor);

    void HandleTaken(string lifterId, string floor, string barcode);
  }

  public class LifterService: ILifterService
  {
    private readonly ILifterCommandService _command;

    private readonly ILifterTaskRepository _repository;

    private readonly ILifterTaskSourceService _taskSource;

    public LifterService(
      ILifterCommandService command,
      ILifterTaskRepository repository,
      ILifterTaskSourceService taskSource
    ) {
      _command = command;
      _repository = repository;
      _taskSource = taskSource;
    }

    public void HandleImported(
      string lifterId, string floor, string barcode,
      string destination, string data, string from
    ) {
      if (_command.IsBarcodeValid(barcode)) {
        _repository.Add(lifterId, floor, barcode, destination, data, from);
        _repository.SaveChanges();
      }

      _command.Import(lifterId, floor, barcode, destination);
    }

    public void HandleScanned(string lifterId, string floor)
    {
      var barcode = _command.GetBarcode(lifterId, floor);
      var destination = _command.GetTaskDestination(lifterId, floor);
      var task = _repository.FindFromRuntimeBarcode(barcode);

      if (task == null) {
        var source = _taskSource.FindFromBarcode(barcode);

        task = _repository.Add(
          lifterId, floor, barcode,
          source.Destination, source.Data, source.From
        );
        _repository.SaveChanges();
      }

      if (!_command.IsDestinationValid(destination)) {
        _command.SetDestination(lifterId, floor, task.Destination);
      }
    }

    public void HandleExported(string lifterId, string floor)
    {
      var barcode = _command.GetBarcode(lifterId, floor);
      var task = _repository.FindFromRuntimeBarcode(barcode);

      if (task != null) {
        task.SetExported();
        _repository.SaveChanges();
        _taskSource.RequestToPick(lifterId, floor, barcode, task.TaskCode, task.From);

        return;
      }

      var source = _taskSource.FindFromBarcode(barcode);

      if (source != null) {
        _taskSource.RequestToPick(lifterId, floor, barcode, source.Data, source.From);
      }
    }

    public void HandleTaken(string lifterId, string floor, string barcode)
    {
      if (_command.IsBarcodeValid(barcode)) {
        var task = _repository.FindFromRuntimeBarcode(barcode);

        if (task != null) {
          task.SetTaken();
          _repository.SaveChanges();
        }
      } else {
        var tasks = _repository.FindRangeFromExported(lifterId, floor);

        foreach (var task in tasks) {
          task.SetTaken();
        }

        _repository.SaveChanges();
      }

      _command.SetTaken(lifterId, floor);
    }
  }
}
