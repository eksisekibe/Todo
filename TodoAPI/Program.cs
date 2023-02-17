using Microsoft.OpenApi.Models;
using System.Text;
using TodoAPI.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Todo.Data;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContextPool<DatabaseContexts>((options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("TodoConn"));
    options.EnableSensitiveDataLogging();
}, poolSize: 20);
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DatabaseContexts>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<DatabaseContexts>(opt => opt.UseInMemoryDatabase("Users"));


builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

builder.Services.AddSwaggerGen(c =>
{
    //Burası Swagger'da görünen tema
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI.API", Version = "v1" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Todo.API.xml");
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.API v1"));
    
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
