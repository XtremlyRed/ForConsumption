using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using ForConsumption.Common.Common;
using ForConsumption.Common.Services;

using Plugins.ToolKits;

namespace ForConsumption.Service.Common
{
    public static class RequestVerify
    {
        private static readonly ConcurrentDictionary<string, DateTime> TokenTimeoutMapper = new ConcurrentDictionary<string, DateTime>();

        static RequestVerify()
        {
            EasyTimer.Share
              .UseInterval(30000)
              .UseAutoReset(true)
              .UesCallback(() =>
              {
                  DateTime @new = DateTime.Now;

                  KeyValuePair<string, DateTime>[] mappers = TokenTimeoutMapper.ToArray();

                  foreach (KeyValuePair<string, DateTime> item in mappers)
                  {
                      TimeSpan span = @new - item.Value;
                      if (span.TotalMinutes > 15)
                      {
                          TokenTimeoutMapper.Remove(item.Key, out DateTime _);
                      }
                  }

              })
              .RunAsync();
        }


        public static bool Verify(this PostContext context, out string message)
        {
            if (TokenTimeoutMapper.TryGetValue(context.Random, out DateTime _))
            {
                message = "随机序列验证失败";
                return false;
            }

            context.ApplicationKey = CommonKeys.ApplicationKey;

            string signToken = context.Sign();

            if (context.SignToken != signToken)
            {
                message = "数据验证失败";
                return false;
            }


            TokenTimeoutMapper[context.Random] = DateTime.Now;

            message = string.Empty;

            return true;

        }

    }
}
