
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ForConsumption.Common.Common;

using Newtonsoft.Json;

namespace ForConsumption.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Member : SettingBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }


        public string LoginName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public string UserName
        {
            get => GetValue<string>(null);
            set => SetValue(value);
        }

        public int LoginCounter
        {
            get => GetValue(0);
            set => SetValue(value);
        }

        public string Password { get; set; }

        public DateTime LoginTime { get; set; }


    }
}
