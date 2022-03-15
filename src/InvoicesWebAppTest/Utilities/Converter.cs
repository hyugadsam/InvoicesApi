using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvoicesWebAppTest.Utilities
{
    public static class Converter
    {
        public static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }

    public static class LoggerMok<T>
    {
        public static ILogger<T> ConfigureLogger()
        {
            var mok = Mock.Of<ILogger<T>>();

            return mok;
        }
    }


}
