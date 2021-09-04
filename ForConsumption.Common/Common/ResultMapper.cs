using System;
using System.Text;

namespace ForConsumption.Common.Common
{
    public abstract class ResultMapper
    {
        public static TRetuenType Deserialize<TRetuenType, Target>(JsonResult<string> result, Action<TRetuenType, Target> callback) where TRetuenType : JsonResult, new()
        {
            TRetuenType returnType = new TRetuenType();

            if (result.Result == false)
            {
                returnType.Result = false;
                returnType.Message = result.Message;
                return returnType;
            }

            returnType.Result = true;
            returnType.Message = result.Message;

            byte[] buffer = GzipAssist.Decompress(Convert.FromBase64String(result.Data));

            string jsonStirng = Encoding.UTF8.GetString(buffer);

            Target array = JsonMapper.Deserialize<Target>(jsonStirng);

            callback?.Invoke(returnType, array);

            return returnType;
        }

        public static string Serialize<Target>(Target targetObject)
        {
            string arr = JsonMapper.Serialize(targetObject);

            byte[] buffer = Encoding.UTF8.GetBytes(arr);

            string base64 = Convert.ToBase64String(GzipAssist.Compress(buffer));

            return base64;
        }

    }
}
