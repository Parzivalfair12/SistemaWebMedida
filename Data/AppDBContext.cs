using Microsoft.EntityFrameworkCore;
using SistemaWebMedida.Models;

namespace SistemaWebMedida.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            
        }

        //Hace referencia a las tablas de mi base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReporteDatos> ReporteDatos { get; set; }

        //La configuracion de mis tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col => col.IdUsuario);
                tb.Property(col => col.IdUsuario)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.NombreCompleto).HasMaxLength(50);
                tb.Property(col => col.CorreoUsuario).HasMaxLength(50);
                tb.Property(col => col.Clave).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            // Configuración de la entidad ReporteDatos
            modelBuilder.Entity<ReporteDatos>(tb =>
            {
                tb.HasKey(col => col.Id);  

                tb.Property(col => col.Id)
                    .UseIdentityColumn()   
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.PresionAtm)
                    .HasColumnType("decimal(18,2)"); 

                tb.Property(col => col.PresionDif)
                    .HasColumnType("decimal(18,2)"); 

                tb.Property(col => col.Co2)
                    .HasColumnType("decimal(18,2)"); 

                tb.Property(col => col.Temperatura)
                    .HasColumnType("decimal(18,2)"); 

                tb.Property(col => col.Fecha)
                    .HasColumnType("datetime")       
                    .IsRequired();                   
            });

            modelBuilder.Entity<ReporteDatos>().ToTable("ReporteDatos");
        }

    }
}
