namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDTO>()
                .ForMember(d => d.BrandName, op => op.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.TypeName, op => op.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, op => op.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand, BrandResultDTO>();
            CreateMap<ProductType, TypeResultDTO>();
        }
    }
}
