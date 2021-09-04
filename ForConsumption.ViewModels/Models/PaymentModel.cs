using System.Threading.Tasks;

using ForConsumption.Common;
using ForConsumption.Common.Common;

namespace ForConsumption.ViewModels.Models
{
    public sealed class PaymentModel : BaseMode
    {
        public async Task<JsonResult<PaymentMode[]>> GetListAsync()
        {

            JsonResult<string>? result = await CreateRequest()

                .UseUrl("payment/getlist")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<PaymentMode[]>, PaymentMode[]>(result, (s, e) =>
            {
                s.Data = e;
            });


        }


        public async Task<JsonResult<int>> EditAsync(PaymentMode mode)
        {

            JsonResult<int>? result = await CreateRequest(mode)

                .UseUrl("payment/update")
                .ExecuteAsync<JsonResult<int>>();

            if (result != null && result.Result)
            {
                mode.ID = result.Data;
            }
            return result;
        }
    }
}
