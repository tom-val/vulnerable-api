﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VulnerableAPI.Database;

#nullable disable

namespace VulnerableAPI.Migrations.UserDb
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("VulnerableAPI.Database.Models.Friend", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FriendUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FriendUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.Ledger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Balance")
                        .HasColumnType("REAL");

                    b.Property<int>("Currency")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ledgers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("264d0484-3bf2-4786-bc3c-cb7196843e46"),
                            Balance = 154.0,
                            Currency = 0,
                            Iban = "LT750065400000000421",
                            UserId = new Guid("15a61231-a186-4fc9-ae24-a83d582eaa4d")
                        },
                        new
                        {
                            Id = new Guid("271260f8-8651-44ff-b381-7dc66edce1f6"),
                            Balance = 342.0,
                            Currency = 0,
                            Iban = "LT760065400000000306",
                            UserId = new Guid("f373061d-5a04-4725-9140-243236afd26a")
                        },
                        new
                        {
                            Id = new Guid("7228d1b6-a3af-4f94-b4e9-e58d7397674a"),
                            Balance = 46.0,
                            Currency = 0,
                            Iban = "LT020065400000000668",
                            UserId = new Guid("ff9763ee-b247-48a3-84bd-7099a54a2074")
                        },
                        new
                        {
                            Id = new Guid("b542d1b0-98be-4746-8aa4-3a95a31f0d18"),
                            Balance = 278.0,
                            Currency = 0,
                            Iban = "LT870065400000000593",
                            UserId = new Guid("fd4bd871-01e0-46fc-bbbd-8b0311659f57")
                        },
                        new
                        {
                            Id = new Guid("297027c7-6365-4165-ba82-94a0abd9fed9"),
                            Balance = 604.0,
                            Currency = 0,
                            Iban = "LT340065400000000480",
                            UserId = new Guid("842b911c-7000-4714-b615-52a8849d569f")
                        },
                        new
                        {
                            Id = new Guid("c7cfde57-ef57-464a-8cef-cc8399d0cd7a"),
                            Balance = 578.0,
                            Currency = 0,
                            Iban = "LT020065400000000959",
                            UserId = new Guid("a28af4ba-d62c-4a7a-8109-9ff9fac19423")
                        },
                        new
                        {
                            Id = new Guid("3392142f-1198-44fe-a355-29348133eb99"),
                            Balance = 610.0,
                            Currency = 0,
                            Iban = "LT570065400000000260",
                            UserId = new Guid("ed807edb-f6bb-4dcf-b293-72d5e4262157")
                        },
                        new
                        {
                            Id = new Guid("72b1f917-8b0b-4ee2-918e-c5459e71fd56"),
                            Balance = 502.0,
                            Currency = 0,
                            Iban = "LT490065400000000598",
                            UserId = new Guid("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d")
                        },
                        new
                        {
                            Id = new Guid("39411cad-26f3-45d7-95ca-8ecc64def4c5"),
                            Balance = 231.0,
                            Currency = 0,
                            Iban = "LT940065400000000952",
                            UserId = new Guid("92b23f71-e891-4616-8e62-f28c761feb25")
                        },
                        new
                        {
                            Id = new Guid("97d44550-b8e5-42ad-8d81-fcb8bedd37e9"),
                            Balance = 111.0,
                            Currency = 0,
                            Iban = "LT030065400000000553",
                            UserId = new Guid("82f6754f-5d67-4b59-a2c1-936e1d72396d")
                        },
                        new
                        {
                            Id = new Guid("67927d1b-31c2-42f1-88f4-dc124bd360b2"),
                            Balance = 416.0,
                            Currency = 0,
                            Iban = "LT320065400000000516",
                            UserId = new Guid("fc66e5ef-d1e8-4f7d-a845-903f93a90387")
                        },
                        new
                        {
                            Id = new Guid("f6d2fb41-973f-4b41-b93e-a86a1d027939"),
                            Balance = 591.0,
                            Currency = 0,
                            Iban = "LT960065400000000237",
                            UserId = new Guid("8adb7e81-d14a-45d7-9b5d-dfc222643029")
                        },
                        new
                        {
                            Id = new Guid("57267dc3-fdbf-4e1e-8c13-28cdefb03e6b"),
                            Balance = 530.0,
                            Currency = 0,
                            Iban = "LT740065400000000148",
                            UserId = new Guid("fcffdaad-83bb-4ba1-83e9-90caeecf147d")
                        },
                        new
                        {
                            Id = new Guid("bab03ce3-7b50-4c71-8e5d-c37922c1dc05"),
                            Balance = 513.0,
                            Currency = 0,
                            Iban = "LT580065400000000824",
                            UserId = new Guid("133e3479-43b7-484a-8449-0ce31bc82f4c")
                        });
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.MoneyRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RequestedById")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RequestedFromId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RequestedById");

                    b.HasIndex("RequestedFromId");

                    b.ToTable("MoneyRequests");
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("15a61231-a186-4fc9-ae24-a83d582eaa4d"),
                            Email = "gertrude_gutmann@pagackonopelski.ca",
                            FirstName = "Kristopher",
                            IsAdmin = false,
                            LastName = "Sipes",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("f373061d-5a04-4725-9140-243236afd26a"),
                            Email = "jameson_parisian@hilpert.ca",
                            FirstName = "Cesar",
                            IsAdmin = false,
                            LastName = "Fahey",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("ff9763ee-b247-48a3-84bd-7099a54a2074"),
                            Email = "torrey_williamson@white.biz",
                            FirstName = "Hildegard",
                            IsAdmin = false,
                            LastName = "Batz",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("fd4bd871-01e0-46fc-bbbd-8b0311659f57"),
                            Email = "bernhard@lemke.biz",
                            FirstName = "Tobin",
                            IsAdmin = false,
                            LastName = "Ledner",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("842b911c-7000-4714-b615-52a8849d569f"),
                            Email = "andreanne@goldner.us",
                            FirstName = "Celestino",
                            IsAdmin = false,
                            LastName = "Auer",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("a28af4ba-d62c-4a7a-8109-9ff9fac19423"),
                            Email = "soledad_cummings@bodemitchell.biz",
                            FirstName = "Karley",
                            IsAdmin = false,
                            LastName = "Hilpert",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("ed807edb-f6bb-4dcf-b293-72d5e4262157"),
                            Email = "lukas.klein@dietrich.com",
                            FirstName = "Bryon",
                            IsAdmin = false,
                            LastName = "Cartwright",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d"),
                            Email = "aletha_schimmel@daugherty.com",
                            FirstName = "Simone",
                            IsAdmin = false,
                            LastName = "Parisian",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("92b23f71-e891-4616-8e62-f28c761feb25"),
                            Email = "liana@romaguera.com",
                            FirstName = "Antone",
                            IsAdmin = false,
                            LastName = "Cartwright",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("82f6754f-5d67-4b59-a2c1-936e1d72396d"),
                            Email = "theodore@runteyundt.us",
                            FirstName = "Adrienne",
                            IsAdmin = false,
                            LastName = "Purdy",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("fc66e5ef-d1e8-4f7d-a845-903f93a90387"),
                            Email = "malika@maggiolemke.com",
                            FirstName = "Evie",
                            IsAdmin = false,
                            LastName = "Braun",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("8adb7e81-d14a-45d7-9b5d-dfc222643029"),
                            Email = "brycen@hilllrutherford.ca",
                            FirstName = "Jamey",
                            IsAdmin = false,
                            LastName = "Von",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("fcffdaad-83bb-4ba1-83e9-90caeecf147d"),
                            Email = "brannon_lebsack@labadie.uk",
                            FirstName = "Silas",
                            IsAdmin = false,
                            LastName = "Hammes",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        },
                        new
                        {
                            Id = new Guid("133e3479-43b7-484a-8449-0ce31bc82f4c"),
                            Email = "haleigh@zboncak.co.uk",
                            FirstName = "Etha",
                            IsAdmin = false,
                            LastName = "Stark",
                            PasswordHash = "hash",
                            PasswordSalt = "salt"
                        });
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.Friend", b =>
                {
                    b.HasOne("VulnerableAPI.Database.Models.User", "FriendUser")
                        .WithMany()
                        .HasForeignKey("FriendUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VulnerableAPI.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FriendUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.Ledger", b =>
                {
                    b.HasOne("VulnerableAPI.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VulnerableAPI.Database.Models.MoneyRequest", b =>
                {
                    b.HasOne("VulnerableAPI.Database.Models.User", "RequestedBy")
                        .WithMany()
                        .HasForeignKey("RequestedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VulnerableAPI.Database.Models.User", "RequestedFrom")
                        .WithMany()
                        .HasForeignKey("RequestedFromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequestedBy");

                    b.Navigation("RequestedFrom");
                });
#pragma warning restore 612, 618
        }
    }
}
