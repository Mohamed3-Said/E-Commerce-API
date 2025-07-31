namespace E_CommerceG01
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Services
            var builder = WebApplication.CreateBuilder(args);
            //presentation services
            builder.Services.AddPresentitionServices();

            //infrastructure services
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //core services
            builder.Services.AddCoreServices(builder.Configuration);


            var app = builder.Build(); 
            #endregion

            #region Pipelines
            app.UseCustomMiddleware();
            await app.SeedDbAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
