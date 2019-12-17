﻿// <auto-generated />
using System;
using ConsumerOne.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsumerOne.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190131020828_VerificationCode")]
    partial class VerificationCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConsumerOne.Api.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<int>("Code");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("DesiredLanguage");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<long?>("FacebookId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PictureUrl");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("Type");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.AppUserFollowerRelation", b =>
                {
                    b.Property<string>("AppUserFollowerId");

                    b.Property<string>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.HasKey("AppUserFollowerId", "AppUserId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("AppUserId1");

                    b.ToTable("AppUserFollowerRelation");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.AppUserFriendRelation", b =>
                {
                    b.Property<string>("AppUserFriendId");

                    b.Property<string>("AppUserId");

                    b.HasKey("AppUserFriendId", "AppUserId");

                    b.HasIndex("AppUserId");

                    b.ToTable("AppUserFriendRelation");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("NameEnUs");

                    b.Property<string>("NameEs");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.Property<string>("Description");

                    b.Property<bool>("IsPaused");

                    b.Property<double?>("Latitude");

                    b.Property<int>("LikeCount");

                    b.Property<double?>("Longitude");

                    b.Property<double?>("Price");

                    b.Property<string>("Tags");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId1");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.Property<string>("Description");

                    b.Property<Guid>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId1");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostMedia", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("PostMedias");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.Property<string>("Description");

                    b.Property<Guid>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId1");

                    b.HasIndex("PostId");

                    b.ToTable("PostMessages");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.Property<string>("Description");

                    b.Property<Guid>("PostId");

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId1");

                    b.HasIndex("PostId");

                    b.ToTable("PostRatings");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<string>("AppUserId1");

                    b.Property<string>("Description");

                    b.Property<Guid>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId1");

                    b.HasIndex("PostId");

                    b.ToTable("PostReports");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PrivacyPolicy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.Property<string>("ValueEnUs");

                    b.Property<string>("ValueEs");

                    b.HasKey("Id");

                    b.ToTable("PrivacyPolicies");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.Translation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Culture");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.UsersByDateReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("NumberOfUsers");

                    b.HasKey("Id");

                    b.ToTable("UsersByDate");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.UseTerm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.Property<string>("ValueEnUs");

                    b.Property<string>("ValueEs");

                    b.HasKey("Id");

                    b.ToTable("UseTerms");

                    b.HasData(
                        new { Id = new Guid("e0de41b5-dd01-49ec-b246-dd671cae7b57"), Value = @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est eopksio laborum.Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium,
                totam rem aperiam,
                eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo.Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit,
                sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt.Neque porro quisquameo est,
                qui dolorem ipsum quia dolor sit amet,
                eopsmiep consectetur,
                adipisci velit,
                seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu" }
                    );
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.AppUserFollowerRelation", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUserFollower")
                        .WithMany("Followers")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.AppUserFriendRelation", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUserFriend")
                        .WithMany()
                        .HasForeignKey("AppUserFriendId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany("Friends")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.Post", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostComment", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");

                    b.HasOne("ConsumerOne.Api.Models.Post", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostMedia", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.Post", "Post")
                        .WithMany("PostMedias")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostMessage", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");

                    b.HasOne("ConsumerOne.Api.Models.Post", "Post")
                        .WithMany("PostMessages")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostRating", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");

                    b.HasOne("ConsumerOne.Api.Models.Post", "Post")
                        .WithMany("PostRatings")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ConsumerOne.Api.Models.PostReport", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");

                    b.HasOne("ConsumerOne.Api.Models.Post", "Post")
                        .WithMany("PostReports")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ConsumerOne.Api.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ConsumerOne.Api.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
