namespace Snail.Core.Entity
{
    public class DefaultBaseEntityWithTenant : DefaultBaseEntity, ITenant<string>
    {
        public string TenantId { get; set; }
    }
}
