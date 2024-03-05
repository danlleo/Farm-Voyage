namespace Timespan.Quota.ConcreteQuotas
{
    public class HardQuota : Quota
    {
        public HardQuota(QuotaDataSO quotaDataSO) : base(quotaDataSO)
        {
            
        }

        public override void OnMetQuota()
        {
            throw new System.NotImplementedException();
        }
    }
}