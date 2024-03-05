namespace Timespan.Quota.ConcreteQuotas
{
    public class MediumQuota : Quota
    {
        public MediumQuota(QuotaDataSO quotaDataSO) : base(quotaDataSO)
        {
            
        }

        public override void OnMetQuota()
        {
            throw new System.NotImplementedException();
        }
    }
}