using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Renet.Web
{
  public class HttpModelValidationException : HttpValidationException
  {
    public HttpModelValidationException(ModelStateDictionary state)
    {
      foreach (var field in state) {
        var messages = field.Value.Errors
          .Select(error => error.ErrorMessage)
          .ToArray();

        Details.Add(field.Key, messages);
      }
    }
  }
}