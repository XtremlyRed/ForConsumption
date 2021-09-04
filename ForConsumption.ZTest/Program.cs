using System.Text;
using System.Threading.Tasks;

using ForConsumption.Common.Common;
using ForConsumption.Common.Services;

using Plugins.ToolKits.EasyHttp;

namespace ForConsumption.ZTest
{
    internal class Program
    {
        public static string IpAddress = "192.168.3.4"; // "121.5.79.53";
        private static async Task Main(string[] args)
        {
            IRestClient client = RestConfig.Default()
               .UseBaseUrl($"http://{IpAddress}:5001")
               .UseDeserializer((@string, type) => JsonMapper.Deserialize(@string, type))
               .UseSerializer(o => JsonMapper.Serialize(o))
               .UseTimeout(100000)
               .UseEncoding(Encoding.UTF8)
               .Build();

            PostContext context = PostContext.Create(null, 0);

            context.Context = JsonMapper.Serialize(new[] { "admin", "admin" });

            context.SignToken = context.Sign();

            JsonResult<string> json = await client.Create()
                .UseMethod(Method.POST)
                .UseUrl("Login/AccessToken")
                .AddParameter(context)
                .ExecuteAsync<JsonResult<string>>();

        }
    }
}
