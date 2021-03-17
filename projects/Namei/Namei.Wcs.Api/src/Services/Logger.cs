using System;

namespace Namei.Wcs.Api
{
  public class Logger
  {
    protected DomainContext _domain;

    public Logger(DomainContext domain)
    {
      _domain = domain;
    }

    public void Save(
      Action<Log> level,
      string klass,
      string operation,
      string message,
      string index = "",
      string data = ""
    ) => Save(Log.From(
      level,
      Log.UseClass(klass),
      Log.UseOperation(operation),
      Log.UseIndex(index),
      Log.UseData(data),
      Log.UseMessage(message)
    ));

    public ScopedLogger UseScope(
      string klass,
      string operation,
      string index
    ) => new ScopedLogger(
      logger: this,
      klass: klass,
      operation: operation,
      index: index
    );

    public void Save(Log log)
    {
      _domain.Add(log);
      _domain.SaveChanges();
    }
  }

  public class ScopedLogger
  {
    private Logger _logger;

    private string _klass;

    private string _operation;

    private string _index;

    public ScopedLogger(
      Logger logger,
      string klass,
      string operation,
      string index
    ) {
      _logger = logger;
      _klass = klass;
      _operation = operation;
      _index = index;
    }

    public void Info(string message, string data)
      => _logger.Save(
        klass: _klass,
        operation: _operation,
        index: _index,
        message: message,
        data: data,
        level: Log.UseInfo()
      );

    public void Success(string message, string data)
      => _logger.Save(
        klass: _klass,
        operation: _operation,
        index: _index,
        message: message,
        data: data,
        level: Log.UseSuccess()
      );


    public void Danger(string message, string data)
      => _logger.Save(
        klass: _klass,
        operation: _operation,
        index: _index,
        message: message,
        data: data,
        level: Log.UseDanger()
      );

  }
}
