using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entities
{
    public class AppUser:IdentityUser<Guid>,IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid ImageId { get; set; } = Guid.Parse("8e6b041c-edd8-40b5-bdde-147c079ecbf8");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
