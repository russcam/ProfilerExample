using Elastic.Apm;
using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.DiagnosticSource;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Note: the agent instance has already been instantiated by the profiler by this point
// so subscribe the diagnostic subscribers for ASP.NET Core events
Agent.Subscribe(new AspNetCoreDiagnosticSubscriber());
Agent.Subscribe(new AspNetCoreErrorDiagnosticsSubscriber());
Agent.Subscribe(new HttpDiagnosticsSubscriber());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
