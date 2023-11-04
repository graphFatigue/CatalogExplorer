using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    internal class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.ToTable("catalogs");

            builder
                .Property(c => c.Id)
                .HasColumnName("id");

            builder.HasKey(b => b.Id);

            builder
                .Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(80)")
                .HasMaxLength(80)
                .IsRequired();

            builder
                .Property(c => c.ParentCatalogId)
                .HasColumnName("parent_catalog_id");

            builder
                .HasOne(s => s.ParentCatalog)
                .WithMany(x => x.ChildrenCatalogs)
                .HasForeignKey(s => s.ParentCatalogId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
