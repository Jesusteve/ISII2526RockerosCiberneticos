using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    DbSet<Alquiler> Alquiler { get; set; }
    DbSet<AlquilarItem> AlquilarItem { get; set; }
    DbSet<Fabricante> Fabricante { get; set; }
    DbSet<Oferta> Oferta { get; set; }
    DbSet<OfertaItem> OfertaItem { get; set; }
    DbSet<Herramienta> Herramienta { get; set; }
    DbSet<Compra> Compra { get; set; }
    DbSet<CompraItem> CompraItem { get; set; }

}
