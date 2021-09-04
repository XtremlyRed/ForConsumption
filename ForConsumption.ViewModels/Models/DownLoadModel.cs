using System.Threading.Tasks;

using ForConsumption.Common.Common;

namespace ForConsumption.ViewModels.Models
{
    public class DownLoadModel : BaseMode
    {
        internal async Task<JsonResult<int>> GetNewestVersionAsync()
        {
            JsonResult<int>? result = await CreateRequest(null)

                .UseUrl("download/getversion")
                .ExecuteAsync<JsonResult<int>>();

            return result;
        }
    }
}
