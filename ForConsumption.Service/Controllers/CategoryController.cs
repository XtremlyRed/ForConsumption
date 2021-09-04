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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDataContext DataContext = new CategoryDataContext();


        [HttpPost]
        [Route("[action]")]
        public JsonAjax Update([FromBody] PostContext context)
        {
            if (context.Verify(out string message) == false)
            {
                return JsonAjax.Error(message);
            }

            try
            {
                CategoryMode categoryMode = JsonMapper.Deserialize<CategoryMode>(context.Context);

                DataContext.CategoryModes.AddOrUpdate(categoryMode);
                DataContext.SaveChanges();
                return JsonAjax.Success(categoryMode.ID);
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }


        [HttpPost]
        [Route("[action]")]
        public JsonAjax GetList([FromBody] PostContext context)
        {
            if (context.Verify(out string message) == false)
            {
                return JsonAjax.Error(message);
            }
            try
            {
                System.Collections.Generic.List<CategoryMode> list = DataContext.CategoryModes.Select.ToList();

                return JsonAjax.Success<string>(ResultMapper.Serialize(list.ToArray()));

            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }
    }
}
