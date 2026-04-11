using System.Globalization;
using AppCore.Interfaces;
using AppCore.Models;
using AppCore.ValueObjects;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Infrastructure.Context;

public class ContactsDbContext: IdentityDbContext<CrmUser, CrmRole, string>
{

    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=database.db");
    }

    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) :
        base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // wymagane przez Identity
        
        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });
        
        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });
        
// Konfiguracji mapowania dziedziczenia TPH
// Jedna tabela do przechowywnia wszystkich typów kontaktów
        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company")
            .HasValue<Organization>("Organization");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(20);
            entity.Property(p => p.Status).HasConversion<string>();
            entity.Property(p => p.CreatedAt).HasColumnType("datetime");
            entity.Property(p => p.UpdatedAt).HasColumnType("datetime");
        });
        
        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.BirthDate).HasColumnType("date");
            entity.Property(p => p.Gender).HasConversion<string>();
            entity.Property(p => p.Status).HasConversion<string>();
        });
        
        // definicja związku
        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(e => e.Employees);

        
        // definicja związku        
        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization);
        
        // przykładowa firma
        builder.Entity<Company>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                    Name = "WSEI",
                    Industry = "edukacja",
                    Phone = "123567123",
                    Email = "biuro@wsei.edu.pl",
                    Website = "https://wsei.edu.pl",
                    Status = ContactStatus.Active,
                    CreatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture),
                    UpdatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture)
                }
            );
        });
        
        var address = new
        {
            Id = Guid.Parse("403e05df-1d6c-48eb-ac9f-0d3be2537de7"),
            City = "Kraków",
            Country = "Poland",
            PostalCode = "25-009",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence,
            // id osoby, która dodana jest niżej
            ContactId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f")
        };
        
        // przykładowe kontakty typu Person
        builder.Entity<Person>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Gender = Gender.Male,
                    Status = ContactStatus.Active,
                    Email = "adam@wsei.edu.pl",
                    Phone = "123456789",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Programista",
                    CreatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture),
                    UpdatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture)
                },
                new 
                {
                    Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    Gender = Gender.Female,
                    Status = ContactStatus.Blocked,
                    Email = "ewa@wsei.edu.pl",
                    Phone = "123123123",
                    BirthDate = DateTime.Parse("2001-01-11", CultureInfo.InvariantCulture),
                    Position = "Tester",
                    CreatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture),
                    UpdatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture)
                });
        });
        //mapowanie adresu jako typu osadzonej w encji Contact 
        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(address);

        builder.Entity<CrmUser>(entity =>
        {
            entity.HasData( new
                {
                    Id = "4d54091d-abc8-49ec-9590-93ad3ed5458f",
                    FirstName = "Adam",
                    LastName = "Nowak",
                    FullName = "Adam Nowak",
                    Email = "adam@wsei.edu.pl",
                    Department = "IT",
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    Status =  SystemUserStatus.Active,
                    CreatedAt =  DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture),
                    UpdatedAt = DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture)
                },
                new 
                {
                    Id = "5d54091d-abc8-49ec-9590-93ad3ed5458f",
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    FullName = "Ewa Kowalska",
                    Email = "ewa@wsei.edu.pl",
                    Department = "HR",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    AccessFailedCount = 0,
                    Status =  SystemUserStatus.Active,
                    CreatedAt =  DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture),
                    UpdatedAt =  DateTime.Parse("2026-01-01", CultureInfo.InvariantCulture)
                });
        });

        builder.Entity<CrmRole>().HasData(
            new
            {
                Id = "28c6f528-e942-4caa-ac76-8efec19b1cad",
                Name = UserRole.Administrator.ToString(),
                NormalizedName = UserRole.Administrator.ToString().ToUpper(),
                ConcurrencyStamp = "1"
            },
            new
            {
                Id = "77cb19fd-4709-44c1-891d-8c17167629b4",
                Name = UserRole.SalesManager.ToString(),
                NormalizedName = UserRole.SalesManager.ToString().ToUpper(),
                ConcurrencyStamp = "2"
            },
            new
            {
                Id = "dd31c666-df97-411e-9454-67fedc452045",
                Name = UserRole.Salesperson.ToString(),
                NormalizedName = UserRole.Salesperson.ToString().ToUpper(),
                ConcurrencyStamp = "3"
            },
            new
            {
                Id = "3975770f-b9a5-4dc6-af25-5460d27160a2",
                Name = UserRole.SupportAgent.ToString(),
                NormalizedName = UserRole.SupportAgent.ToString().ToUpper(),
                ConcurrencyStamp = "4"
            },
            new
            {
                Id = "036d4821-65d7-4fd9-9038-3c00ccc7eb58",
                Name = UserRole.ReadOnly.ToString(),
                NormalizedName = UserRole.ReadOnly.ToString().ToUpper(),
                ConcurrencyStamp = "5"
            }
        );
    }
}
