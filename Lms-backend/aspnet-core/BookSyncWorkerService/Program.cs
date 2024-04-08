using BookSyncWorkerService;
using BookSyncWorkerService.Context;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BookService>();


// Dependency Injection Database context
// Inject Database context for the API
//builder.Services.AddDbContext<LmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LMSConnectionString")));



// Configure HttpClient instances
builder.Services.AddHttpClient("Book_API", client => {
    client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
});

builder.Services.AddHttpClient("Custom_Book_EndPoint", client => {
    client.BaseAddress = new Uri("https://localhost:44311/api/services/app/");
});

builder.Services.AddHttpClient("Open_AI_API", client => {
    client.BaseAddress = new Uri("https://api.openai.com/");
});

// Inject Worker service to do heavy lifting
builder.Services.AddHostedService<BookService>();

var host = builder.Build();
host.Run();
