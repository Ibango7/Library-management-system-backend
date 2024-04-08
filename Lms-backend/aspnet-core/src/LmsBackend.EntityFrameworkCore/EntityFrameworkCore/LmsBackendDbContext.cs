using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using LmsBackend.Authorization.Roles;
using LmsBackend.Authorization.Users;
using LmsBackend.MultiTenancy;
using LmsBackend.Entities;

namespace LmsBackend.EntityFrameworkCore
{
    public class LmsBackendDbContext : AbpZeroDbContext<Tenant, Role, User, LmsBackendDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Book> Books { get; set; }
        public DbSet<BookManagement> BookManagement { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<BadgeManagement> BadgesManagement { get; set; }
        public DbSet<LibraryEvent> LibraryEvents { get; set; }
        public DbSet<WaitList> WaitList { get; set; }
        public DbSet<Notification> Notification { get; set; }


        public LmsBackendDbContext(DbContextOptions<LmsBackendDbContext> options)
            : base(options)
        {

        }
    }
}
