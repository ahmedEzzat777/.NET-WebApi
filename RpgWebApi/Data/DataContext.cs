using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgWebApi.Models;

namespace RpgWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CharacterSkill> CharacterSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkill>()
                .HasKey(cs => new { cs.CharacterId, cs.SkillId });

            modelBuilder.Entity<User>()
                .Property(user => user.Role).HasDefaultValue("Player");

            modelBuilder.Entity<Skill>().HasData(
                    new Skill { Id = 1, Name = "Fireball", Damage = 30 },
                    new Skill { Id = 2, Name = "Frenzy", Damage = 20 },
                    new Skill { Id = 3, Name = "Blizzard", Damage = 50 }
                );

            Utility.CreatePassword("123456", out byte[] hash, out byte[] salt);

            modelBuilder.Entity<User>().HasData(
                    new User {Id = 1, PasswordHash = hash, PasswordSalt = salt, UserName = "User1" },
                    new User {Id = 2, PasswordHash = hash, PasswordSalt = salt, UserName = "User2" }
                );

            modelBuilder.Entity<Character>().HasData(
                new Character {Id = 1, UserId = 1},
                new Character {Id = 2, UserId = 2}
                );

            modelBuilder.Entity<Weapon>().HasData(
                new Weapon { Id = 1, CharacterId = 1 , Name = "sword", Damage = 30},
                new Weapon { Id = 2, CharacterId = 2 , Name = "short sword", Damage = 40}
                );


            modelBuilder.Entity<CharacterSkill>().HasData(
                new CharacterSkill {  CharacterId = 1, SkillId = 1 },
                new CharacterSkill {  CharacterId = 2, SkillId = 3 },
                new CharacterSkill {  CharacterId = 2, SkillId = 2 }
                );
        }
    }
}
