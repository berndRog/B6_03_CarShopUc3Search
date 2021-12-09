using System.Diagnostics;
using CarShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System;

[assembly: InternalsVisibleToAttribute("CarShop.Application.Test")]

namespace CarShop.Persistence {

   internal class CDbContext : DbContext {

      #region fields
      private readonly ILogger<CDbContext> _log        = null!;
      private readonly string              _connection = string.Empty;
      #endregion

      #region properties
      public DbSet<User>    Users     { get => Set<User>();    }
      public DbSet<Address> Addresses { get => Set<Address>(); }
      public DbSet<Car>     Cars      { get => Set<Car>(); }
      #endregion

      #region ctor
      public CDbContext() : base() { }
      
      public CDbContext(DbContextOptions<CDbContext> options) : base(options) { }
      
      public CDbContext(
         DbContextOptions<CDbContext> options,
         IConfiguration configuration,
         ILogger<CDbContext> log
      ) : base(options) {
         _log = log;
         _log.LogInformation($"Ctor() {GetHashCode()}");
         _connection = configuration.GetConnectionString("LocalDb");
         _log.LogInformation($"Connection {_connection}");
        
      }
      #endregion


      protected override void OnConfiguring(
         DbContextOptionsBuilder optionsBuilder
      ) {
         optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Database=CarShop;Integrated Security=true;"
         );
         optionsBuilder
//          .LogTo(Console.WriteLine)
            .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
            .EnableSensitiveDataLogging();

         //optionsBuilder.LogTo(message => Debug.WriteLine(message));  
  

      }

      protected override void OnModelCreating(ModelBuilder modelBuilder) {
         // https://docs.microsoft.com/de-de/ef/core/modeling/relationships?tabs=data-annotations%2Cdata-annotations-simple-key%2Csimple-key
         base.OnModelCreating(modelBuilder);
         //CarConfiguration(modelBuilder);
         //UserConfiguration(modelBuilder);
         //AddressConfiguration(modelBuilder);
         //MarkedCarConfiguration(modelBuilder);

      }


      private static void AddressConfiguration(ModelBuilder modelBuilder) {
         modelBuilder.Entity("CarShop.Domain.Entities.Address", entity => {
            entity.ToTable("Addresses");
            entity.HasKey("Id");
            entity.Property<long>("Id")
                  .HasColumnType("bigint")
                  .ValueGeneratedNever();
            entity.Property<string>("StreetNr")
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");

            entity.Property<string>("ZipCity")
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");
         });
      }

      private static void CarConfiguration(ModelBuilder modelBuilder) {
         modelBuilder.Entity("CarShop.Domain.Entities.Car", entity => {
            
            entity.ToTable("Cars");             // tablename
            entity.HasKey("Id");                // primary key
            entity.Property<long>("Id")         // primary key type long
                  .HasColumnType("bigint")     
                  .ValueGeneratedNever();       // no autoincrement 
            entity.Property<string>("Make")
                  .IsRequired()
                  .HasColumnType("nvarchar(32)");
            entity.Property<string>("Model")
                  .IsRequired()
                  .HasColumnType("nvarchar(32)");
            entity.Property<double>("Price")
                  .HasColumnType("float");

            // Foreign Key
            entity.Property<long?>("UserId")    //
                  .HasColumnType("bigint");
            entity.HasIndex("UserId");

            entity.HasOne("CarShop.Domain.Entities.User", "User")
                  .WithMany("OfferedCars")
                  .HasForeignKey("UserId");
            entity.Navigation("User");
         });
      }

      private static void UserConfiguration(ModelBuilder modelBuilder) {

         modelBuilder.Entity("CarShop.Domain.Entities.User", entity => {
            entity.ToTable("Users");            // tablename 
            entity.HasKey("Id");                // primary key
            entity.Property<long>("Id")         // primary key type long
                  .HasColumnType("bigint")
                  .ValueGeneratedNever();       // no autoincrement 
            entity.Property<string>("Name")
                  .IsRequired()
                  .HasColumnType("nvarchar(32)");
            entity.Property<string>("Email")
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");
            entity.Property<string>("UserName")
                  .IsRequired()
                  .HasColumnType("nvarchar(32)");
            entity.Property<string>("Password")
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");
            entity.Property<string>("Salt")
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");
            entity.Property<int>("Role")
                  .HasColumnType("int");

            entity.Property<long?>("AddressId")
                .HasColumnType("bigint");
            entity.HasIndex("AddressId");

            entity.HasOne("CarShop.Domain.Entities.Address", "Address")
                  .WithMany()
                  .HasForeignKey("AddressId");
            entity.Navigation("Address");


            entity.Navigation("OfferedCars");
         });

      }
   }
}