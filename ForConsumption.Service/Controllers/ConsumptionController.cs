using System;
using System.Linq;
using System.Text;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.Common.Services;
using ForConsumption.Service.Common;
using ForConsumption.Service.DAO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Plugins.ToolKits;

using JsonAjax = ForConsumption.Common.Common.JsonResult;

namespace ForConsumption.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConsumptionController : ControllerBase
    {
        private readonly ConsumptionDataContext DataContext = new ConsumptionDataContext();

        public ConsumptionController()
        {

        }

        [HttpPost]
        [Route("[action]")]
        public JsonAjax Edit([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }


                ConsumptionItem consumption = JsonMapper.Deserialize<ConsumptionItem>(context.Context);
                bool isAdd = consumption.ID <= 0;
                DataContext.ConsumptionItems.AddOrUpdate(consumption);
                DataContext.SaveChanges();
                if (isAdd)
                {
                    try
                    {
                        IFreeSql sql = Injecter.Resolve<IFreeSql>();
                        sql.Update<PaymentMode>(consumption.PaymentId)
                            .Set(i => i.Counter + 1)
                            .ExecuteAffrows();
                        sql.Update<CategoryMode>(consumption.CategoryId)
                           .Set(i => i.Counter + 1)
                           .ExecuteAffrows();
                    }
                    catch (Exception)
                    {
                    }
                }

                return JsonAjax.Success(consumption.ID);
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }


        [HttpPost]
        [Route("[action]")]
        public JsonAjax Today([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                DateTime now = DateTime.Now;

                DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                System.Collections.Generic.List<ConsumptionItem> list = DataContext.ConsumptionItems.Where(i => i.CreateTime > start && i.CreateTime < end).ToList();

                ConsumptionItem[] aray = list.OrderByDescending(i => i.CreateTime).ToArray();

                return JsonAjax.Success<string>(ResultMapper.Serialize(aray));
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }

        [HttpPost]
        [Route("[action]")]
        public JsonAjax ToMouth([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                string currentMouthString = JsonMapper.Deserialize<string>(context.Context);

                DateTime now = JsonMapper.Deserialize<DateTime>(currentMouthString);

                DateTime start = new DateTime(now.Year, now.Month, 1, 0, 0, 0);

                DateTime end = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

                System.Collections.Generic.List<ConsumptionItem> list = DataContext.ConsumptionItems.Where(i => i.CreateTime > start && i.CreateTime < end).ToList();

                ConsumptionItem[] aray = list.OrderByDescending(i => i.CreateTime).ToArray();

                string arr = JsonMapper.Serialize(aray);

                byte[] buffer = Encoding.UTF8.GetBytes(arr);

                string base64 = Convert.ToBase64String(GzipAssist.Compress(buffer));

                return JsonAjax.Success<string>(base64);

            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }



        [HttpPost]
        [Route("[action]")]
        public JsonAjax PageInfo([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                int[] ints = JsonMapper.Deserialize<int[]>(context.Context);

                int currentPage = ints[0];
                int pageSize = ints[1];

                System.Collections.Generic.List<ConsumptionItem> list = DataContext.ConsumptionItems.Select.OrderByDescending(i => i.CreateTime).Page(currentPage, pageSize).ToList();

                byte[] buffer = Encoding.UTF8.GetBytes(JsonMapper.Serialize(list.ToArray()));

                string base64 = Convert.ToBase64String(GzipAssist.Compress(buffer));

                return JsonAjax.Success<string>(base64);

            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }
    }
}
