using System.Threading.Tasks;

using ForConsumption.Common;
using ForConsumption.Common.Common;

using Plugins.ToolKits;
using Plugins.ToolKits.EasyHttp;

namespace ForConsumption.ViewModels.Models
{
    public class MemberModel : BaseMode
    {
        public async Task<JsonResult<string[]>> Login(string loginName, string password)
        {
            IRestClient client = Injecter.Resolve<IRestClient>();


            string[]? strings = new[] { loginName, password };

            IRestRequest? request = CreateRequest(strings)
                .UseUrl("Member/AccessToken");

            JsonResult<string>? jsonResult = await request

                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<string[]>, string[]>(jsonResult, (s, e) =>
            {
                s.Data = e;
            });


        }


        public async Task<JsonResult<Member[]>> GetListAsync()
        {
            JsonResult<string>? jsonResult = await CreateRequest()

                .UseUrl("Member/getlist")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<Member[]>, Member[]>(jsonResult, (s, e) =>
            {
                s.Data = e;
            });

        }
    }
}
