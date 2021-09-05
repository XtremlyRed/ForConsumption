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
        public static string CurrentToken { get; set; }

        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const string ApplicationKey = "#￥%…&**64%…￥%55#5#&((564%^!@@65--+/-*(&&*(8{}）&545";

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
         
        public string Sign()
        {
            using (MD5 mi = MD5.Create())
            {

                string stringBuffer = $"Key={ApplicationKey}&Time={TimeStamp}&Context={Context}&Random={Random}";

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
                
                Random = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now.Ticks,
                Context = content is null ? "" : JsonMapper.Serialize(content),
                OwnerId = ownerId
            };
        }

    }
}
