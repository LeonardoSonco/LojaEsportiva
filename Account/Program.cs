using EmailService.Core.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

using Registerservice.Data;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("RegisterConnection");


// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(connectionString)); // Conecta no banco
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Two", Version = "v1" });
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

app.UseAuthorization();

app.MapGet("/teste-email", async (EmailHostedService hostedService) =>
{
    await hostedService.SendEmailAsync(new EmailService.Core.Common.Email.Model.EmailModel
    {
        EmailAnddress = "leonardosonco@gmail.com",
        Subject = "Hello from NET 6",
        Body = "<strong> Hellor From Hosted Service </strong>",
        Attachment = null
    });
}).WithName("TestEmail");

app.UseEndpoints(endpoints =>
{
    Console.WriteLine("Funcionou");
    endpoints.MapControllers();
});

app.Run();

