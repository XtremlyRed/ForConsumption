using System;

using Newtonsoft.Json;

namespace ForConsumption.Common.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonResult : SettingBase
    {
        public JsonResult()
        {
        }

        public JsonResult(string message)
        {
            Message = message;
        }

        public string Message
        {
            get => GetUniqueObject(() => GetValue<string>());
            set => SetValue(value);
        }

        public bool Result
        {
            get => GetUniqueObject(() => GetValue(false));
            set => SetValue(value);
        }


        public static JsonResult Error(Exception exception)
        {
            return new JsonResult()
            {
                Message = exception.GetBaseException().Message
            };
        }

        public static JsonResult Error(string message)
        {
            return new JsonResult()
            {
                Message = message
            };
        }

        public static JsonResult Success(string message)
        {
            return new JsonResult()
            {
                Message = message,
                Result = true
            };
        }

        public static JsonResult<TData> Success<TData>(TData result, string message = null)
        {
            return new JsonResult<TData>()
            {
                Message = message,
                Result = true,
                Data = result
            };
        }

        public static JsonResult<TData, TValue> Success<TData, TValue>(TData result, TValue value, string message = null)
        {
            return new JsonResult<TData, TValue>()
            {
                Message = message,
                Result = true,
                Data = result,
                Value = value
            };
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class JsonResult<TData> : JsonResult
    {
        public JsonResult()
        {
        }

        public JsonResult(TData data)
        {
            Data = data;
            Result = true;
        }


        public TData Data
        {
            get => GetUniqueObject(() => GetValue<TData>());
            set => SetValue(value);
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class JsonResult<TData, TValue> : JsonResult<TData>
    {
        public JsonResult(TData data, TValue value) : base(data)
        {
            Value = value;
        }
        public JsonResult()
        {
        }
        public TValue Value
        {
            get => GetUniqueObject(() => GetValue<TValue>());
            set => SetValue(value);
        }
    }
}
