using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
   public DbSet<Alquiler> Alquiler { get; set; }
   public DbSet<AlquilarItem> AlquilarItem { get; set; }
    DbSet<Fabricante> Fabricante { get; set; }
    DbSet<Oferta> Oferta { get; set; }
    DbSet<OfertaItem> OfertaItem { get; set; }
    public DbSet<Herramienta> Herramienta { get; set; }
    DbSet<Compra> Compra { get; set; }
    DbSet<CompraItem> CompraItem { get; set; }
    DbSet<ReparaciónItem> ReparaciónItem { get; set; }
    DbSet<Reparación> Reparación { get; set; }
}
