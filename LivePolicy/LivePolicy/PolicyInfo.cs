using System;
using System.Collections.Generic;
using System.Globalization;

namespace LivePolicy
{
    public class PolicyInfo
    {
        public DateTime PublishDate { get; set; }
        public int Version { get; set; }

        private readonly Dictionary<string, object> _dictionary;

        public PolicyInfo()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public void Add(string key, int value)
        {
            AddKey(key, value);
        }

        public void Add(string key, DateTime value)
        {
            AddKey(key, value);
        }

        public void Add(string key, bool value)
        {
            AddKey(key, value);
        }

        public void Add(string key, string value)
        {
            AddKey(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return GetValue(key, defaultValue);
        }

        public int GetInt(string key, int defaultValue = default(int))
        {
            return GetValue(key, defaultValue);
        }

        public DateTime GetDate(string key, DateTime defaultValue = default(DateTime))
        {
            return GetValue(key, defaultValue);
        }

        public bool GetBoolean(string key, bool defaultValue = default(bool))
        {
            return GetValue(key, defaultValue);
        }

        private void AddKey(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        private T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (key == string.Empty)
            {
                throw new ArgumentException("Key cannot be empty", "key");
            }

            object value;

            if (_dictionary.TryGetValue(key, out value))
            {
                var result = Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);

                if (result != null)
                {
                    return (T)result;
                }
            }
            return defaultValue;
        }

    }
}