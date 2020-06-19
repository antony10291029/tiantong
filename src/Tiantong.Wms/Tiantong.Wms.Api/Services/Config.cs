using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Tiantong.Wms.Api
{
  public class Config
  {
    public string ENV { get; }

    public bool IsProduction { get => ENV == "Production"; }

    public bool IsDevelopment { get => ENV == "Development"; }

    public string APP_NAME { get; }

    public string APP_VERSION { get; }

    public string APP_KEY { get; }

    //

    public string ROOT_EMAIL { get; }

    public string ROOT_PASSWORD { get; }

    //

    public string JWT_KEY { get; }

    public int JWT_TTL { get; }

    public int JWT_RFT { get; }

    //

    public string STMP_HOST { get; }

    public int STMP_PORT { get; }

    public string STMP_ADDRESS { get; }

    public string STMP_PASSWORD { get; }

    //

    public string PG_HOST { get; }

    public int PG_PORT { get; }

    public string PG_USERNAME { get; }

    public string PG_PASSWORD { get; }

    public string PG_DATABASE { get; }

    public Config(IConfiguration config, IHostEnvironment env)
    {
      ENV = env.EnvironmentName;
      
      APP_NAME = config.GetValue("app_name", "APP");
      APP_VERSION = config.GetValue("app_version", "0.1.0");
      APP_KEY = "base64:t33sHNNsdml4eq2IdROC9I1aLQTasGQy6ecnLWzqsXo=";

      ROOT_EMAIL = config.GetValue("root_email", "root@wms.com");
      ROOT_PASSWORD = config.GetValue("root_password", "123456");

      JWT_KEY = config.GetValue("jwt:key", "rPqfeL2Y4cqMErd4J4e1ghrb2rg03mDd");
      JWT_TTL = config.GetValue("jwt:ttl", 864000000);
      JWT_RFT = config.GetValue("jwt:rft", 864000000);

      STMP_HOST = config.GetValue("stmp:host", "smtpdm.aliyun.com");
      STMP_PORT = config.GetValue("stmp:port", 25);
      STMP_ADDRESS = config.GetValue("stmp:address", "");
      STMP_PASSWORD = config.GetValue("stmp:password", "");

      PG_HOST = config.GetValue("pg:host", "localhost");
      PG_PORT = config.GetValue("pg:port", 5432);
      PG_USERNAME = config.GetValue("pg:username", "postgres");
      PG_PASSWORD = config.GetValue("pg:password", "password");
      PG_DATABASE = config.GetValue("pg:database", "postgres");
    }
  }
}
