
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

using ForConsumption.Common.Common;

using Newtonsoft.Json;

namespace ForConsumption.Common
{
    [DebuggerDisplay("Money:{Money}  {Category}  {Payment}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ConsumptionItem : SettingBase
    {
        private string owner;
        private string category;
        private string payment;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }
        public decimal Money
        {
            get => GetValue<decimal>(0M);
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>(null);
            set => SetValue(value);
        }

        public int PaymentId
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }

        public int CategoryId
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }

        [DefaultValue(1)]
        public int OwnerId
        {
            get => GetValue<int>(0);
            set => SetValue(value);
        }


        public DateTime CreateTime
        {
            get => GetValue(DateTime.Now);
            set => SetValue(value);
        }
        public string Address
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        [NotMapped]
        public string Payment
        {
            get => payment;
            set
            {
                payment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public string Category
        {
            get => category;
            set
            {
                category = value;
                RaisePropertyChanged();
            }
        }
        [NotMapped]
        public string Owner
        {
            get => owner;
            set
            {
                owner = value;
                RaisePropertyChanged();
            }
        }

        public ConsumptionItem Copy()
        {
            ConsumptionItem @new = MemberwiseClone() as ConsumptionItem;
            return @new;
        }

    }


    [DebuggerDisplay("Header:{Header} Count:{Count}")]
    [NotMapped]
    public class ItemsDisplay : ObservableCollection<ConsumptionItem>
    {
        public string Header { get; set; }


        public int TotalCount { get; set; }

        public double Percent { get; set; }

        private decimal totalMoney;
        public decimal TotalMoney
        {
            get => totalMoney;
            set
            {
                totalMoney = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TotalMoney)));
            }
        }
    }
}
