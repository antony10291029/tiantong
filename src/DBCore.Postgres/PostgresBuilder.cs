using Microsoft.EntityFrameworkCore;

namespace DBCore.Postgres
{
  public class PostgresBuilder
  {
    private string _host = "localhost";

    private int _port = 5432;

    private string _username = "postgres";

    private string _password = "123456";

    private string _database = "postgres";

    public PostgresBuilder Host(string host)
    {
      _host = host;

      return this;
    }

    public PostgresBuilder Port(int port)
    {
      _port = port;

      return this;
    }

    public PostgresBuilder Username(string username)
    {
      _username = username;

      return this;
    }

    public PostgresBuilder Password(string password)
    {
      _password = password;

      return this;
    }

    public PostgresBuilder Database(string database)
    {
      _database = database;

      return this;
    }

    public void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql($"Host={_host};Port={_port};Database={_database};Username={_username};Password={_password}");
    }
  }
}
