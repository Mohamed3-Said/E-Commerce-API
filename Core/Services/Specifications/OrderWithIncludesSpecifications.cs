namespace Services.Specifications
{
    class OrderWithIncludesSpecifications : Specifications<Order>
    {
        public OrderWithIncludesSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
        }
        public OrderWithIncludesSpecifications(string email) : base(o => o.UserEmail == email)  
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.OrderDate);
        }
    }
}
