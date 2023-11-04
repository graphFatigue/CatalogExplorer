﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entity.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar(80)")
                        .HasColumnName("name");

                    b.Property<int?>("ParentCatalogId")
                        .HasColumnType("int")
                        .HasColumnName("parent_catalog_id");

                    b.HasKey("Id");

                    b.HasIndex("ParentCatalogId");

                    b.ToTable("catalogs", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Creating Digital Images"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Resources",
                            ParentCatalogId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "Evidence",
                            ParentCatalogId = 1
                        },
                        new
                        {
                            Id = 4,
                            Name = "Graphic Products",
                            ParentCatalogId = 1
                        },
                        new
                        {
                            Id = 5,
                            Name = "Primary Sources",
                            ParentCatalogId = 2
                        },
                        new
                        {
                            Id = 6,
                            Name = "Secondary Sources",
                            ParentCatalogId = 2
                        },
                        new
                        {
                            Id = 7,
                            Name = "Process",
                            ParentCatalogId = 4
                        },
                        new
                        {
                            Id = 8,
                            Name = "Final Product",
                            ParentCatalogId = 4
                        });
                });

            modelBuilder.Entity("Core.Entity.Catalog", b =>
                {
                    b.HasOne("Core.Entity.Catalog", "ParentCatalog")
                        .WithMany("ChildrenCatalogs")
                        .HasForeignKey("ParentCatalogId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("ParentCatalog");
                });

            modelBuilder.Entity("Core.Entity.Catalog", b =>
                {
                    b.Navigation("ChildrenCatalogs");
                });
#pragma warning restore 612, 618
        }
    }
}
