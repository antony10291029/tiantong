using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Midos.Web.Config
{
  public class MidosConfigProvider: ConfigurationProvider
  {
    struct Config
    {
      public string key { get; set; }

      public string value { get; set; }
    }

    public override void Load()
    {
      var uri = GetMidosUrl();
      var path = "/midos/configs";
      var http = new HttpClient() {
        BaseAddress = new Uri(uri),
        Timeout = TimeSpan.FromSeconds(5),
      };

      try {
        var response = http.PostAsync("/midos/configs", null).GetAwaiter().GetResult();
        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var configs = JsonSerializer.Deserialize<List<Config>>(content);
        Data = configs.ToDictionary(config => config.key, config => config.value);
      } catch (TaskCanceledException) {
        throw KnownException.Error($"Fail to load config from midos server: \"{uri + path}\"");
      } catch(Exception e) {
        throw e;
      }
    }

    private string GetMidosUrl()
    {
      var jsonConfig = new JsonConfigurationSource();

      jsonConfig.FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
      jsonConfig.Path = "appsettings.json";

      var jsonProvider = new JsonConfigurationProvider(jsonConfig);

      jsonProvider.Load();
      jsonProvider.TryGet("midos", out string uri);

      if (uri == null) {
        throw KnownException.Error("Fail to load midos url in appsettings.*.json");
      }

      return uri;
    }
  }
}
