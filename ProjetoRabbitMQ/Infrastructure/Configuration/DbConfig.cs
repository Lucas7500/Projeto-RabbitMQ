﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoRabbitMQ.Models.Joins;
using ProjetoRabbitMQ.Models.Manga;
using ProjetoRabbitMQ.Models.MangaOrder;
using ProjetoRabbitMQ.Models.User;

namespace ProjetoRabbitMQ.Infrastructure.Configuration
{
    public static class DbConfig
    {
        public static void ConfigureUserEntity(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(254)
                .IsRequired();
            
            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .IsRequired();

            builder.HasData(MigrationData.AdminUser);
        }

        public static void ConfigureMangaEntity(EntityTypeBuilder<MangaEntity> builder)
        {
            builder.ToTable("mangas");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .IsRequired();

            builder.Property(x => x.Author)
                .HasColumnName("author")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ReleaseDate)
                .HasColumnName("release_date")
                .IsRequired();

            builder.Property(x => x.Genres)
                .HasColumnName("genres")
                .HasColumnType("json");

            builder.Property(x => x.Aliases)
                .HasColumnName("aliases")
                .HasColumnType("json");
        }
        
        public static void ConfigureMangaOrderEntity(EntityTypeBuilder<MangaOrderEntity> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever()
                .HasConversion(
                    ulid => ulid.ToString(),
                    str => Ulid.Parse(str))
                .HasColumnType("VARCHAR(26)")
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();
            
            builder.Property(x => x.CancellationReason)
                .HasColumnName("cancellation_reason")
                .IsRequired(false);

            builder
                .HasOne(mo => mo.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(mo => mo.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void ConfigureMangaOrderItemEntity(EntityTypeBuilder<MangaOrderItemEntity> builder)
        {
            builder.ToTable("ordered_items");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder
                .HasOne(moi => moi.OrderedManga)
                .WithMany()
                .HasForeignKey(moi => moi.MangaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        public static void ConfigureUserMangaEntity(EntityTypeBuilder<UserMangaEntity> builder)
        {
            builder.ToTable("user_mangas");

            builder.HasKey(x => new { x.UserId, x.MangaId });

            builder
                .HasOne(um => um.User)
                .WithMany(u => u.UserMangas)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(moi => moi.Manga)
                .WithMany(m => m.UserMangas)
                .HasForeignKey(moi => moi.MangaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}