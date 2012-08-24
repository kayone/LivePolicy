using System;
using System.Collections.Generic;

namespace LivePolicy
{
    public class PolicyInfo : Dictionary<string, string>
    {
        public DateTime PublishDate { get; set; }
        public int Version { get; set; }
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

        public void Load()
        {
            PolicyInfo newPolicy;

            if (TryFetchPolicyFromSource(out newPolicy))
            {
                _policyStorage.Write(newPolicy);
            }
            else
            {
                newPolicy = _policyStorage.Read();
            }

            Current = newPolicy;
        }

        private bool TryFetchPolicyFromSource(out PolicyInfo sourcePolicy)
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

        public PolicyInfo Current { get; private set; }
    }
}
