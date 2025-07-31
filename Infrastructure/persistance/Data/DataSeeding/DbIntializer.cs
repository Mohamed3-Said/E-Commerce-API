namespace Persistance.Data.DataSeeding
{
    public class DbIntializer : IDbIntializer
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbIntializer(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task IntializeAsync()
        {
            try
            {
                //if (_context.Database.GetPendingMigrations().Any())
                //{
                    await _context.Database.MigrateAsync();
                    if (!_context.ProductTypes.Any())
                    {
                        var typeData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\types.json");
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                        if (types is not null && types.Any())
                        {
                            await _context.AddRangeAsync(types);
                            await _context.SaveChangesAsync();
                        }

                    }

                    if (!_context.ProductBrands.Any())
                    {
                        var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\brands.json");
                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                        if (brands is not null && brands.Any())
                        {
                            await _context.AddRangeAsync(brands);
                            await _context.SaveChangesAsync();
                        }

                    }

                    if (!_context.Products.Any())
                    {
                        var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\products.json");
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                        if (products is not null && products.Any())
                        {
                            await _context.AddRangeAsync(products);
                            await _context.SaveChangesAsync();
                        }

                    }
                    if (!_context.DeliveryMethods.Any())
                    {
                        var methodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\delivery.json");
                        var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(methodsData);
                        if (methods is not null && methods.Any())
                        {
                            await _context.AddRangeAsync(methods);
                            await _context.SaveChangesAsync();
                        }

                    }
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task IntializeIdentityAsync()
        { 
            //seed roles
            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));    
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));    
            }
            //seed users ,assign user ==> role
            if (!_userManager.Users.Any())
            {
                var adminUser = new User()
                {
                    DisplayName = "Admin",
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "1234567890",
                };
                var superAdminUser = new User()
                {
                    DisplayName = "SuperAdmin",
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "1234567890",
                };

                await _userManager.CreateAsync(adminUser,"P@ssw0rd");
                await _userManager.CreateAsync(superAdminUser, "Passw0rd@");

                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
    }
}
