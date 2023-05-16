using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Net.Mime.MediaTypeNames;

namespace Blog.Data.Mapping
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {    //Bu şekilde kullanılabilir  && IsRequired(false) diyip null geçilebilir yapablirsin
            //builder.Property(x => x.Title).HasMaxLength(150);
            //builder.Property(x => x.Title).IsRequired(false);

            builder.HasData(new Article
            {
           
                Id = Guid.NewGuid(),
                Title = "Asp.net Core Deneme Makalesi1",
                Content = "Asp.net Core Deneme Makalesi1 Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam vitae facilisis lacus. Phasellus ornare lorem elit, id venenatis ligula tempus sit amet. Proin libero justo, sodales ac blandit nec, malesuada quis sapien. Mauris ornare mauris sit amet risus pulvinar bibendum quis vitae ex. Nam dignissim odio eu velit dictum porta. Nullam mattis a diam vitae suscipit. Fusce consequat lectus metus, in interdum magna iaculis eget. Vestibulum id nulla ac orci tincidunt sodales rhoncus nec odio. In enim quam, fringilla eget ligula nec, egestas maximus purus. Fusce lorem ante, mattis ac metus non, porta convallis neque. Curabitur suscipit, metus vitae bibendum congue, augue lacus maximus turpis, at blandit odio massa vitae magna. Pellentesque hendrerit, augue a efficitur porta, ligula orci ornare quam, quis feugiat dui massa et libero. Quisque aliquam est vitae libero aliquam, in tincidunt enim condimentum.",
                ViewCount = 15,
                CategoryId = Guid.Parse("6720DCDC-6AC5-4F19-8BC4-4FDB812AEA89"),
                ImageId = Guid.Parse("D491F90B-90AC-468A-A076-0ED56AC15E6E"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId= Guid.Parse("45CB1797-8868-4C0D-846D-D7E5C3E4BB26")

            }, new Article
            {
                Id = Guid.NewGuid(),
                Title = "Visual Studio Deneme Makalesi1",
                Content = "Visual Studio Deneme Makalesi1 Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam vitae facilisis lacus. Phasellus ornare lorem elit, id venenatis ligula tempus sit amet. Proin libero justo, sodales ac blandit nec, malesuada quis sapien. Mauris ornare mauris sit amet risus pulvinar bibendum quis vitae ex. Nam dignissim odio eu velit dictum porta. Nullam mattis a diam vitae suscipit. Fusce consequat lectus metus, in interdum magna iaculis eget. Vestibulum id nulla ac orci tincidunt sodales rhoncus nec odio. In enim quam, fringilla eget ligula nec, egestas maximus purus. Fusce lorem ante, mattis ac metus non, porta convallis neque. Curabitur suscipit, metus vitae bibendum congue, augue lacus maximus turpis, at blandit odio massa vitae magna. Pellentesque hendrerit, augue a efficitur porta, ligula orci ornare quam, quis feugiat dui massa et libero. Quisque aliquam est vitae libero aliquam, in tincidunt enim condimentum.",
                ViewCount = 15,
                CategoryId = Guid.Parse("D58C7491-BA0F-46AF-ACC2-53968A7C3648"),
                ImageId = Guid.Parse("8E6B041C-EDD8-40B5-BDDE-147C079ECBF8"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId= Guid.Parse("66DC5149-6B91-462C-8EBC-2CD2C18C7D35")

            }



            );

        }
    }
}
 