using System;
using System.Collections.Generic;
using System.Globalization;

namespace LivePolicy
{
    public class PolicyInfo : Dictionary<string, string>
    {
        public DateTime PublishDate { get; set; }
        public int Version { get; set; }

        public void Add(string key, int value)
        {
            Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Add(string key, DateTime value)
        {
            Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Add(string key, bool value)
        {
            Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public string GetString(string key)
        {
            return this[key];
        }

        public int GetInt(string key)
        {
            return Convert.ToInt32(this[key], CultureInfo.InvariantCulture);
        }

        public DateTime GetDate(string key)
        {
            return Convert.ToDateTime(this[key], CultureInfo.InvariantCulture);
        }

        public bool GetBoolean(string key)
        {
            return Convert.ToBoolean(this[key], CultureInfo.InvariantCulture);
        }
    }

    public interface IPolicySource
    {
        PolicyInfo FetchPolicy();
    }

    public interface IPolicyStorage
    {
        PolicyInfo Read();
        void Write(PolicyInfo policyInfo);
    }

    public class PolicyService
    {
        private readonly IPolicySource _policySource;
        private readonly IPolicyStorage _policyStorage;

        public PolicyService(IPolicySource policySource, IPolicyStorage policyStorage)
        {
            _policySource = policySource;
            _policyStorage = policyStorage;
        }

        public bool Load()
        {
            try
            {
                PolicyInfo newPolicy;

                if (TryFetchPolicy(out newPolicy))
                {
                    _policyStorage.Write(newPolicy);
                }
                else
                {
                    newPolicy = _policyStorage.Read();
                }

                Current = newPolicy;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryFetchPolicy(out PolicyInfo sourcePolicy)
        {
            sourcePolicy = null;

            try
            {
                sourcePolicy = _policySource.FetchPolicy();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PolicyInfo Current { get; internal set; }
    }
}
