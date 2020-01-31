﻿// <auto-generated />
using System;
using FootballPredictor.Data.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FootballPredictor.Data.EFCore.PostgreSQL.Migrations
{
    [DbContext(typeof(FootballDbContext))]
    [Migration("20200130113520_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("FootballPredictor.Domain.Model.Match", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AwayTeam")
                        .HasColumnType("text");

                    b.Property<int>("AwayTeamGoals")
                        .HasColumnType("integer");

                    b.Property<long>("AwayTeamId")
                        .HasColumnType("bigint");

                    b.Property<string>("HomeTeam")
                        .HasColumnType("text");

                    b.Property<int>("HomeTeamGoals")
                        .HasColumnType("integer");

                    b.Property<long>("HomeTeamId")
                        .HasColumnType("bigint");

                    b.Property<long>("MatchId")
                        .HasColumnType("bigint");

                    b.Property<long>("Matchday")
                        .HasColumnType("bigint");

                    b.Property<long>("SeasonId")
                        .HasColumnType("bigint");

                    b.Property<string>("Winner")
                        .HasColumnType("text");

                    b.Property<long?>("WinnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });
#pragma warning restore 612, 618
        }
    }
}