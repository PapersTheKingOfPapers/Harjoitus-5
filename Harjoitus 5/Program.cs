using Harjoitus_5;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Web-palvelu
builder.Services.AddDbContext<SuperAdventure>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API-Kutsu
app.MapGet("/", () => "T‰m‰ on GET API-Kutsu");

// Palauttaa kutsujalle kaikki tilatiedot taulusta Stats
app.MapGet("/superadventure", async(SuperAdventure context) => await context.Stats.ToListAsync());

// Palauttaa kutsujalle (POST ja PUT -kutsut) kaikki tilatiedot
async Task<List<Stat>> GetAllStats(SuperAdventure context) => await context.Stats.ToListAsync();

// P‰ivitt‰‰ kutsujan pyynnˆst‰ tilatiedot Stats -tauluun
app.MapPut("/superadventure/{id}", async(SuperAdventure context, Stat stat, int id) =>
{
    // Haetaan p‰‰avaimen (id) perusteella tietueen tietokannasta
    var dbStat = await context.Stats.FindAsync(id);
    if (dbStat is null) return Results.NotFound("Ei tilatietoja. :/");

    // M‰‰ritell‰‰n p‰ivitett‰v‰t tiedot
    dbStat.CurrentHitPoints = stat.CurrentHitPoints;
    dbStat.MaxHitPoints = stat.MaxHitPoints;
    dbStat.Gold = stat.Gold;
    dbStat.Exp = stat.Exp;

    // P‰ivitet‰‰n tilatiedot
    dbStat.CurrentLocationID = stat.CurrentLocationID;
    await context.SaveChangesAsync();

    // Jos tallennus meni hyvin, niin kutustaan metodia GetAllStats().
    return Results.Ok(await GetAllStats(context));
});

app.Run();