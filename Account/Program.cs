using System.Text;

using EmailService.Core.Common.Email.Model;
using EmailService.Core.HostedServices;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Registerservice.Data;

using RegisterService;
using RegisterService.Models;
using RegisterService.Repositories;
using RegisterService.Service;

var builder = WebApplication.CreateBuilder(args);


var key = Encoding.ASCII.GetBytes(Settings.Secret); // Transformanando a chave em array de bytes

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => // informando para api como validar o token
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters // Parametros usados para desincripta o token
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(@"Server=databaseloja.cszmlywukup7.sa-east-1.rds.amazonaws.com;Initial Catalog=lojaDB;Persist Security Info=False;User ID=admin;Password=22112001;")); // Conecta no banco
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Two", Version = "v1" });
    c.ResolveConflictingActions(x => x.First());
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
    options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

builder.Services.AddSingleton<EmailHostedService>();
builder.Services.AddHostedService(provider => provider.GetService<EmailHostedService>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Two V1");

});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapPost("login", (User model) =>
{
    var user = UserRepository.Get(model.Username, model.Password);
    if (user == null)
        return Results.NotFound(new { message = "Usuario inválido" });

    var token = TokenService.GenerateToken(user);
    user.Password = "";
    return Results.Ok(new
    {
        user = user,
        token = token
    });
});


app.MapPost("/teste-email", async (EmailHostedService hostedService) =>
{
    await hostedService.SendEmailAsync(new EmailService.Core.Common.Email.Model.EmailModel
    {
        EmailAnddress = "leonardosonco@gmail.com",
        Subject = "Novo Cliente",
        Body = "<strong> Você foi cadastrado no nosso banco de dados! </strong>",
        Attachment = null
    });
}).WithName("TestEmail");


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

