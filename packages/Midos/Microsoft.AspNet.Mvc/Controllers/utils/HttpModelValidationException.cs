using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc
{
  public class HttpModelValidationException : HttpValidationException
  {
    public HttpModelValidationException(ModelStateDictionary state)
    {
      // foreach (var field in state) {
      //   var messages = field.Value.Errors
      //     .Select(error => error.ErrorMessage)
      //     .ToArray();

      //   Details.Add(field.Key, messages);
      // }

      _message = state.First().Value.Errors.First().ErrorMessage;
    }
  }
}
