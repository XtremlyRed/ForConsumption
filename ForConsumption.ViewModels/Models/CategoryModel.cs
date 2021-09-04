using System.Threading.Tasks;

using ForConsumption.Common;
using ForConsumption.Common.Common;

namespace ForConsumption.ViewModels.Models
{
    public sealed class CategoryModel : BaseMode
    {
        public async Task<JsonResult<CategoryMode[]>> GetListAsync()
        {


            JsonResult<string>? result = await CreateRequest(null)

                .UseUrl("category/getlist")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<CategoryMode[]>, CategoryMode[]>(result, (s, e) =>
            {
                s.Data = e;
            });

        }


        public async Task<JsonResult<int>> EditAsync(CategoryMode mode)
        {

            JsonResult<int> result = await CreateRequest(mode)
                .UseUrl("category/update")
                .ExecuteAsync<JsonResult<int>>();

            if (result != null && result.Result)
            {
                mode.ID = result.Data;
            }

            return result;
        }
    }
}
