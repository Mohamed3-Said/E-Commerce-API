namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<ICasheService> _casheService;
        public  ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository,
            UserManager<User> userManager, IOptions<JwtOptions> options, IConfiguration configuration,ICasheRepository casheRepository)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork,mapper));

            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, options, mapper));

            _orderService = new Lazy<IOrderService>(() => new OrderService(mapper,basketRepository,unitOfWork));

            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(basketRepository, configuration, unitOfWork, mapper));

            _casheService = new Lazy<ICasheService>(() => new CasheService(casheRepository));
        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

        public ICasheService CasheService => _casheService.Value;
    }
}
