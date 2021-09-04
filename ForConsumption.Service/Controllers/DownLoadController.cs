
using System;
using System.IO;
using System.Text;

using ForConsumption.Common.Common;
using ForConsumption.Common.Services;
using ForConsumption.Service.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using JsonAjax = ForConsumption.Common.Common.JsonResult;


namespace ForConsumption.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DownLoadController : ControllerBase
    {
        private static readonly string dirName = Path.Combine(Environment.CurrentDirectory, "config");

        [HttpGet]
        [Route("[action]")]
        public ActionResult DownLoad()
        {
            string dirName = Path.Combine(Environment.CurrentDirectory, "config");

            if (System.IO.Directory.Exists(dirName) == false)
            {
                System.IO.Directory.CreateDirectory(dirName);
            }

            string fileName = Path.Combine(dirName, "update.config");
            string content = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
            UpdateInfo updateInfo = JsonMapper.Deserialize<UpdateInfo>(content);

            string file = System.IO.Path.Combine(dirName, updateInfo.AppName);

            const string contentType = "appliation/vnd.android.package-archive";

            return PhysicalFile(file, contentType, updateInfo.AppName);
        }


        [HttpPost]
        [Route("[action]")]
        public JsonAjax GetVersion([FromBody] PostContext postContext)
        {
            try
            {
                if (postContext.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }



                if (System.IO.Directory.Exists(dirName) == false)
                {
                    System.IO.Directory.CreateDirectory(dirName);
                }

                string fileName = Path.Combine(dirName, "update.config");
                UpdateInfo updateInfo = null;
                if (!System.IO.File.Exists(fileName))
                {
                    updateInfo = new UpdateInfo();
                    System.IO.File.WriteAllText(fileName, JsonMapper.Serialize(updateInfo), Encoding.UTF8);
                }
                else
                {
                    string content = System.IO.File.ReadAllText(fileName, Encoding.UTF8);

                    updateInfo = JsonMapper.Deserialize<UpdateInfo>(content);
                }

                return JsonAjax.Success(updateInfo.CurrentVersion);
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }
        }


        public class UpdateInfo
        {
            public int CurrentVersion { get; set; } = 1;

            public string AppName { get; set; } = "1.apk";
        }

    }
}
