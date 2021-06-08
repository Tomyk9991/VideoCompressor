using System.Collections.Generic;
using System.ComponentModel;

namespace VideoCompressor.Commands
{
    [System.Serializable]
    public class BitRateHolder
    {
        public int dc { get; set; }
        
        public IDictionary<string, object> ToDictionary()
        {
            return this.ToDictionary<object>();
        }

        public IDictionary<string, T> ToDictionary<T>()
        {
            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
                AddPropertyToDictionary(property, dictionary);
            return dictionary;
        }

        private void AddPropertyToDictionary<T>(PropertyDescriptor property, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(this);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }
    }
}