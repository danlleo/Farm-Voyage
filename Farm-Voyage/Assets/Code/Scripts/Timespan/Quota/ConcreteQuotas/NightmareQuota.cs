namespace Timespan.Quota.ConcreteQuotas
{
    public class NightmareQuota : Quota
    {
        public NightmareQuota(QuotaDataSO quotaDataSO) : base(quotaDataSO)
        {
            
        }

        public override void OnMetQuota()
        {
            throw new System.NotImplementedException();
        }
    }
}