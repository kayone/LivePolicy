namespace LivePolicy
{
    public interface IPolicyStorage
    {
        PolicyInfo Read();
        void Write(PolicyInfo policyInfo);
    }
}