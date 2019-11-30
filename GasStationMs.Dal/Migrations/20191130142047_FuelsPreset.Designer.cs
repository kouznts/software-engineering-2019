﻿// <auto-generated />
using GasStationMs.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GasStationMs.Dal.Migrations
{
    [DbContext(typeof(GasStationContext))]
    [Migration("20191130142047_FuelsPreset")]
    partial class FuelsPreset
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GasStationMs.Dal.Entities.Fuel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.ToTable("Fuels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "АИ-92",
                            Price = 42.299999999999997
                        },
                        new
                        {
                            Id = 2,
                            Name = "АИ-92+",
                            Price = 43.299999999999997
                        },
                        new
                        {
                            Id = 3,
                            Name = "АИ-95",
                            Price = 45.899999999999999
                        },
                        new
                        {
                            Id = 4,
                            Name = "АИ-95+",
                            Price = 46.899999999999999
                        },
                        new
                        {
                            Id = 5,
                            Name = "АИ-98",
                            Price = 48.100000000000001
                        },
                        new
                        {
                            Id = 6,
                            Name = "АИ-100",
                            Price = 51.799999999999997
                        },
                        new
                        {
                            Id = 7,
                            Name = "ДТ",
                            Price = 46.299999999999997
                        },
                        new
                        {
                            Id = 8,
                            Name = "Пропан",
                            Price = 20.899999999999999
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
