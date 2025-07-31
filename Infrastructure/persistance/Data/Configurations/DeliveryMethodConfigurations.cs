namespace Persistance.Data.Configurations
{
    class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d=>d.Price).HasColumnType("decimal(18,3)");
        }
    }
}
