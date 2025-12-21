using Microsoft.EntityFrameworkCore;
using TodoList.Data;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
        .AddControllers().ConfigureApiBehaviorOptions(options => {
            options.SuppressModelStateInvalidFilter = true;
        });

builder
    .Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ToDoListDataContext>(options => {
    options.UseSqlite(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
