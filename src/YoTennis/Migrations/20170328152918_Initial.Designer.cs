using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using YoTennis.Data;

namespace YoTennis.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20170328152918_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YoTennis.Data.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("YoTennis.Data.MatchEvent", b =>
                {
                    b.Property<Guid>("MatchId");

                    b.Property<int>("Version");

                    b.Property<string>("Event");

                    b.HasKey("MatchId", "Version");

                    b.ToTable("MatchEvents");
                });
        }
    }
}
