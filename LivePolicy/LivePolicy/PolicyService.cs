using System;

namespace LivePolicy
{
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
