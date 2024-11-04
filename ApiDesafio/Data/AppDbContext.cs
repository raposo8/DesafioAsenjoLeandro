using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDesafio.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDesafio.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Usuarios { get; set; }

        public DbSet<Bar> Bares { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
}