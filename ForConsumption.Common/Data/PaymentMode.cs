
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

using ForConsumption.Common.Common;

using Newtonsoft.Json;

namespace ForConsumption.Common
{



    [DebuggerDisplay("{Mode}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentMode : SettingBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }

        public string Mode
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime CreateTime
        {
            get => GetValue<DateTime>(DateTime.Now);
            set => SetValue(value);
        }


        public int Counter
        {
            get => GetValue(0);
            set => SetValue(value);
        }

        public PaymentMode Copy()
        {
            PaymentMode item = new();
            item.ID = ID;
            item.Mode = Mode;
            item.CreateTime = CreateTime;
            return item;
        }
    }
}
