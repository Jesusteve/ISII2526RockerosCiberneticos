using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Alquiler> Alquiler { get; set; }
    public DbSet<AlquilarItem> AlquilarItem { get; set; }
    public DbSet<Fabricante> Fabricante { get; set; }
    public DbSet<Oferta> Oferta { get; set; }
    public DbSet<OfertaItem> OfertaItem { get; set; }
    public DbSet<Herramienta> Herramienta { get; set; }
    public DbSet<Compra> Compra { get; set; }
    public DbSet<CompraItem> CompraItem { get; set; }
    public DbSet<ReparaciónItem> ReparaciónItem { get; set; }
    public DbSet<Reparación> Reparación { get; set; }
    }
