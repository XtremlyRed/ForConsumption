using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using ForConsumption.Common.Common;

using Newtonsoft.Json;

namespace ForConsumption.Common.Services
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PostContext
    {
        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string ApplicationKey { get; set; }

        [JsonProperty("t")]
        public long TimeStamp { get; set; }

        [JsonProperty("r")]
        public string Random { get; set; }

        [JsonProperty("c")]
        public string Context { get; set; }

        [JsonProperty("s")]
        public string SignToken { get; set; }

        [JsonProperty("i")]
        public int OwnerId { get; set; }

        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string FixSignBuffer => $"Time={TimeStamp}&Context={Context}&Random={Random}";

        public string Sign()
        {
            using (MD5 mi = MD5.Create())
            {

                string stringBuffer = $"Key={ApplicationKey}&{FixSignBuffer}";

                byte[] buffer = Encoding.UTF8.GetBytes(stringBuffer);

                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public static PostContext Create(object content, int ownerId)
        {
            return new PostContext()
            {
                ApplicationKey = CommonKeys.ApplicationKey,
                Random = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now.Ticks,
                Context = content is null ? "" : JsonMapper.Serialize(content),
                OwnerId = ownerId
            };
        }

    }
}
