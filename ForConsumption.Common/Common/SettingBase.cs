using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Plugins.ToolKits;

namespace ForConsumption.Common.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class SettingBase : INotifyPropertyChanged, IDisposable
    {
        [JsonExtensionData] private JObject _divt = new JObject();

        public event PropertyChangedEventHandler PropertyChanged;

        public object this[string key]
        {
            get => GetValue<object>(default, key);
            set => SetValue(value, key);
        }

        public virtual void Dispose()
        {
            _divt.RemoveAll();
            _divt = null;
        }

        public override string ToString()
        {
            return JsonMapper.Serialize(_divt);
        }

        protected void SetValue<TType>(TType value, [CallerMemberName] string propertyName = null)
        {
            Thrower.IfNull(() => propertyName);



            if (value == null)
            {
                _divt[propertyName] = null;
            }
            else
            {
                _divt[propertyName] = JToken.FromObject(value);
            }


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected TType GetValue<TType>(TType defaultValue = default, [CallerMemberName] string propertyName = null)
        {
            Thrower.IfNull(() => propertyName);

            if (!_divt.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out JToken value))
            {
                if (defaultValue == null)
                {
                    return defaultValue;
                }

                _divt[propertyName] = JToken.FromObject(defaultValue);
                return defaultValue;
            }

            if (Equals(value, null))
            {
                return defaultValue;
            }

            return value.ToObject<TType>();
        }


        public void CopyTo<TSetting>(TSetting setting) where TSetting : SettingBase
        {
            if (_divt.DeepClone() is JObject jObject)
            {
                setting._divt = jObject;
            }
        }


        protected void Remove(string key)
        {
            if (_divt.ContainsKey(key))
            {
                _divt.Remove(key);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Target GetUniqueObject<Target>(Func<Target> func, [CallerMemberName] string propertyName = null)
        {
            return (Target)ValuePairs.GetOrAdd(propertyName, (key) => func.Invoke());
        }

        private readonly ConcurrentDictionary<string, object> ValuePairs = new ConcurrentDictionary<string, object>();
    }
}
