using Products.Web.Services;

namespace Products.Web
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddHttpClient<IProductAPIServiceClient, ProductServiceClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7239/");
            });
            builder.Services.AddControllersWithViews() 
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                    .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            builder.Services.AddRazorPages();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
