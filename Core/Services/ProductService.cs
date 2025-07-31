namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {

        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var brandsResult = _mapper.Map<IEnumerable<BrandResultDTO>>(brands);

            return brandsResult;
        }

        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(
                new ProductWithBrandAndTypeSpecifications(parameters));

            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(
                new ProductCountSpecifications(parameters));



            var productsResult = _mapper.Map<IEnumerable<ProductResultDTO>>(products);
            
            var result= new PaginatedResult<ProductResultDTO>(
                productsResult.Count(),
                parameters.PageIndex,
                totalCount,
                productsResult);

            return result;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            
            var typesResult = _mapper.Map<IEnumerable<TypeResultDTO>>(types);

            return typesResult;
        }

        public async Task<ProductResultDTO?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(
                new ProductWithBrandAndTypeSpecifications(id));
            
            return product is null ? throw new ProductNotFoundException(id): _mapper.Map<ProductResultDTO>(product);
       


        }
    }
}
