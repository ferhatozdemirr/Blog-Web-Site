using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blog.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,Guid,AppUserClaim,AppUserRole,AppUserLogin,AppRoleClaim,AppUserToken>
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        public DbSet<Article>Articles { get; set; }   
        public DbSet<Category>Categories { get; set; }
        public DbSet<Image>Images { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        public DbSet<ArticleVisitor> ArticleVisitors { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //assembly dedğimiz şey bulundugumuz katmanın ismine diyoruz

            //IEntityTypeConfiguration<Article> kullandıgımız her classda 
            //aşağıdaki bağlabtıyı teker teker tanımlamak yerine bu şekilde 
            //tanımlama yapabiliyoruz
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            


        }


    }
}
