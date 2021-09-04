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
    public class CategoryMode : SettingBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get => GetValue<int>(0, "i");
            set => SetValue(value, "i");
        }

        public string Mode
        {
            get => GetValue<string>(null, "m");
            set => SetValue(value, "m");
        }

        public DateTime CreateTime
        {
            get => GetValue<DateTime>(DateTime.Now, "ct");
            set => SetValue(value, "ct");
        }

        public int Counter
        {
            get => GetValue(0, "c");
            set => SetValue(value, "c");
        }

        public CategoryMode Copy()
        {
            CategoryMode item = new();
            item.ID = ID;
            item.Mode = Mode;
            item.CreateTime = CreateTime;
            return item;
        }
    }
}
