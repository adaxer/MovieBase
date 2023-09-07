
namespace MovieBase.Users;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddIdentityServer()
            .AddDeveloperSigningCredential()  // For dev purposes
            .AddInMemoryApiResources(Config.GetApiResources())  // Your API resources
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryClients(Config.GetClients())            // Your client configurations
            .AddTestUsers(Config.GetUsers());                   // For testing, use in-memory users


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
