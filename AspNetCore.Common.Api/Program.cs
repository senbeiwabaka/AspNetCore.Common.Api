using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Common.Api;
using AspNetCore.Common.Api.Validations.V1;
using AspNetCore.Common.Data;
using AspNetCore.Common.Domain;
using AspNetCore.Common.Models;
using AspNetCore.Common.Shared.Models;
using FluentValidation;

var inMemoryDbId = Guid.NewGuid();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(inMemoryDbId.ToString());

    //options.UseNpgsql(
    //    builder.Configuration.GetConnectionString("postgres"),
    //    options =>
    //    {
    //        options.MigrationsAssembly("AspNetCore.Common.Domain");
    //    });
});

builder.Services.AddScoped<ISchoolReadonlyRepository, SchoolReadonlyRepository>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IValidator<School>, SchoolValidator>();
builder.Services.AddScoped<RequestModelValidator<School>, SchoolRequestModelValidator>();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});

builder.Services.AddOpenApiDocument();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
    {
        context.Database.EnsureCreated();

        context.Add(new School
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Created = DateTime.Now,
            Updated = DateTime.Now,
        });

        context.Add(new School
        {
            Id = Guid.NewGuid(),
            Name = "Test1",
            Created = DateTime.Now,
            Updated = DateTime.Now,
        });

        context.Add(new School
        {
            Id = Guid.NewGuid(),
            Name = "abc",
            Created = DateTime.Now,
            Updated = DateTime.Now,
        });

        _ = context.SaveChanges();
    }
}

await app.RunAsync();
