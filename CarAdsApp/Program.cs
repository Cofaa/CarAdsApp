using CarAdsApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); // Dodavanje keširanja u memoriji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Postavljanje vremena isteka sesije
    options.Cookie.HttpOnly = true; // Postavljanje kolacica kao HttpOnly
    options.Cookie.IsEssential = true; // Postavljanje kolacica kao obaveznog
});

// Add services to the container.
//builder.Services.AddAuthentication("CookieAuth")
//    .AddCookie("CookieAuth", options =>
//    {
//        options.Cookie.Name = "UserLoginCookie"; // Putanja za prijavu
//        options.LoginPath = "/User/Login"; // Putanja za odjavu
        
    
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserServices>(); // Registracija servisa za korisnike
builder.Services.AddScoped<OglasiServices>(); // Registracija servisa za oglase

var app = builder.Build();

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
//app.UseAuthentication(); // Korisnicka autentifikacija

app.UseSession(); // KorisnicSke sesije

app.UseAuthorization();

app.MapControllerRoute(
     //name: "default",
     //pattern: "{controller=User}/{action=Register}/{id?}");
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
