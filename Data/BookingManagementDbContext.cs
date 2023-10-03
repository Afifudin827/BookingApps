using BookingApps.Models;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class BookingManagementDbContext : DbContext
{
    /*
     * Didalam class BookingManagementDbContext kita melakukan db set yang nantinya akan membuat database pada sqlServer, 
     * pada code ini akan membuat sebuah tabel database di antaranya 
     * Account, AccountRole, Role, Employee, Booking, Room, Univercity dan Education. 
     * 
     */
    public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<AccountRole> AccouRoles { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    /*
     * Pada code ini juga akan membuat pengaturan relasi antar table di antaranya 
         * Univercity yang akan berelasi dengan Education melalui UnivercityGuid,
         * Education yang akan berelasi dengan Employee dengan menggunakan Guid Mereka, 
         * Account yang akan berelasi dengan employee dengan menggunaakn Guid, 
         * Role yang akan berelasi dengan AccountRole dengan RoleGuid, 
         * Booking yang akan berelasi dengan employee dengan EmployeeGuid dan 
         * Booking juga yang akan berelasi dengan room dengan menggunakan RoomGuid.D
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Data Atribut yang memiliki data unique
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>().HasIndex(e => new
        {
            e.NIK,
            e.Email,
            e.PhoneNumber
        }).IsUnique();

        modelBuilder.Entity<University>()
                    .HasMany(e => e.Educations)
                    .WithOne(u => u.University)
                    .HasForeignKey(e => e.UniversityGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Education>()
                    .HasOne(e => e.Employee)
                    .WithOne(e => e.Education)
                    .HasForeignKey<Education>(e => e.Guid);

        modelBuilder.Entity<Account>()
                    .HasOne(e => e.Employee)
                    .WithOne(a => a.Account)
                    .HasForeignKey<Account>(a => a.Guid);

        modelBuilder.Entity<AccountRole>()
                    .HasOne(ar => ar.Account)
                    .WithOne(a => a.AccountRole)
                    .HasForeignKey<AccountRole>(ar => ar.AccountGuid);

        modelBuilder.Entity<Role>()
                    .HasMany(ar => ar.AccountRole)
                    .WithOne(r => r.Role)
                    .HasForeignKey(ar => ar.RoleGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
                    .HasOne(e => e.Employee)
                    .WithMany(b => b.Booking)
                    .HasForeignKey(e => e.EmployeeGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
                    .HasOne(r => r.Room)
                    .WithOne(b => b.Booking)
                    .HasForeignKey<Booking>(r => r.RoomGuid);
    }



}
