using System;
using System.Threading.Tasks;

using ForConsumption.Common;
using ForConsumption.Common.Common;

namespace ForConsumption.ViewModels.Models
{
    public sealed class ForConsumptionModel : BaseMode
    {


        private ForConsumptionModel()
        {

        }
        public static ForConsumptionModel Instance { get; } = new ForConsumptionModel();


        public async Task<JsonResult> UpdateConsumptionItemAsync(ConsumptionItem item)
        {
            string? jsonString = JsonMapper.Serialize(item);
            JsonResult<int> result = await CreateRequest(item)
                .UseUrl("Consumption/Edit")
                .ExecuteAsync<JsonResult<int>>();

            if (result != null && result.Result)
            {
                item.ID = result.Data;
            }
            return result;
        }

        public async Task<JsonResult<ConsumptionItem[], int>> LoadTodayInfosAsync()
        {
            JsonResult<string>? result = await CreateRequest(null)

                .UseUrl("consumption/today")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<ConsumptionItem[], int>, ConsumptionItem[]>(result, (s, e) =>
             {
                 s.Data = e;
                 s.Value = e.Length;
             });


        }


        public async Task<JsonResult<ConsumptionItem[], int>> GetPageInfosAsync(int currentPage, int pageSize = 20)
        {
            int[]? ints = new[] { currentPage, pageSize };

            JsonResult<string>? result = await CreateRequest(ints)

                .UseUrl("consumption/PageInfo")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<ConsumptionItem[], int>, ConsumptionItem[]>(result, (s, e) =>
            {
                s.Data = e;
                s.Value = e.Length;
            });



        }

        public async Task<JsonResult<ConsumptionItem[]>> GetConsumptionInfosByMouth(DateTime currentMouth)
        {
            string? time = JsonMapper.Serialize(currentMouth);
            JsonResult<string>? result = await CreateRequest(time)

                .UseUrl("consumption/ToMouth")
                .ExecuteAsync<JsonResult<string>>();

            return ResultMapper.Deserialize<JsonResult<ConsumptionItem[], int>, ConsumptionItem[]>(result, (s, e) =>
             {
                 s.Data = e;
                 s.Value = e.Length;
             });
        }
    }



}
