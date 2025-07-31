namespace Services.Specifications
{
    public class ProductCountSpecifications : Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters) :
            base(product =>
                            (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value) &&
                            (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value) &&
                            (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim()))
            )
        {
        }

    }
}
