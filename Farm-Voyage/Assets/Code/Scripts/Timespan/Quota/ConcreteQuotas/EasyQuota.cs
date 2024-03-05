namespace Timespan.Quota.ConcreteQuotas
{
    public class EasyQuota : Quota
    {
        public EasyQuota(QuotaDataSO quotaDataSO) : base(quotaDataSO)
        {
            
        }

        public override void OnMetQuota()
        {
            throw new System.NotImplementedException();
        }
    }
}