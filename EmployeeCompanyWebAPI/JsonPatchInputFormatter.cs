using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace EmployeeCompanyWebAPI
{
    public static class JsonPatchInputFormatter
    {
        public static NewtonsoftJsonPatchInputFormatter Get()
        {
            var provider = new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson().Services.BuildServiceProvider();
            var formatter = provider.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
            return formatter;
        }
    }
}
