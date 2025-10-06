using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    DbSet<Alquiler> Alquiler { get; set; }
    DbSet<AlquilarItem> AlquilarItem { get; set; }
    DbSet<Herramienta> Herramienta { get; set; }
    DbSet<Compra> Compra { get; set; }
    DbSet<CompraItem> CompraItem { get; set; }

}
