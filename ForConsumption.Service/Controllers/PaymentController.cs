using System;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.Common.Services;
using ForConsumption.Service.Common;
using ForConsumption.Service.DAO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using JsonAjax = ForConsumption.Common.Common.JsonResult;

namespace ForConsumption.Service.Controllers
{


    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDataContext DataContext = new PaymentDataContext();


        [HttpPost]
        [Route("[action]")]
        public JsonAjax Update([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                PaymentMode PaymentMode = JsonMapper.Deserialize<PaymentMode>(context.Context);

                DataContext.PaymentModes.AddOrUpdate(PaymentMode);
                DataContext.SaveChanges();
                return JsonAjax.Success(PaymentMode.ID);
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }




        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public JsonAjax GetList([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                System.Collections.Generic.List<PaymentMode> list = DataContext.PaymentModes.Select.ToList();

                return JsonAjax.Success<string>(ResultMapper.Serialize(list.ToArray()));
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }
    }
}
