using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DBCore
{
  public class DbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    private Assembly _assembly;

    public DbSet<Migration> Migrations { get; set; }

    private string _sqlDirectory = "Sql";

    public DbContext()
    {
      _assembly = GetType().Assembly;
    }

    public bool HasTable(string table)
    {
      try {
        Database.ExecuteSqlRaw($"select 1 from \"{table}\" limit 1");
        return true;
      } catch {
        return false;
      }
    }

    public string SqlDirectory()
    {
      return _sqlDirectory;
    }

    public void UseSqlDirectory(string dir)
    {
      _sqlDirectory = dir;
    }

    public void UseAssembly(Assembly assembly)
    {
      _assembly = assembly;
    }

    public bool HasTable<T>() where T : class
    {
      var set = this.Set<T>();

      try {
        set.FirstOrDefault();
        return true;
      } catch {
        return false;
      }
    }

    public void ExecuteFromSql(string name, Assembly assembly)
    {
      var assemblyName = assembly.GetName().Name;
      var fileName = $"{assemblyName}.{_sqlDirectory}.{name}.sql";
      var stream = assembly.GetManifestResourceStream(fileName);

      if (stream == null) {
        throw new Exception($"Sql file not found: {fileName}");
      }

      var sreader = new StreamReader(stream);

      var sql = sreader.ReadToEnd();

      Database.ExecuteSqlRaw(sql);
    }

    public void ExecuteFromSql(string name)
    {
      var assemblyName = _assembly.GetName().Name;
      var fileName = $"{assemblyName}.{_sqlDirectory}.{name}.sql";
      var stream = _assembly.GetManifestResourceStream(fileName);

      if (stream == null) {
        throw new Exception($"Sql file not found: {fileName}");
      }

      var sreader = new StreamReader(stream);

      var sql = sreader.ReadToEnd();

      Database.ExecuteSqlRaw(sql);
    }

    public void ExecuteSql(string sql)
    {
      Database.ExecuteSqlRaw(sql);
    }
  }
}
