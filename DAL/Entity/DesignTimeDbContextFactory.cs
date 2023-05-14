﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.Entity
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WBSTOContext>
    {
        public WBSTOContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new
            ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new
            DbContextOptionsBuilder<WBSTOContext>();
            var connectionString =
            configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new WBSTOContext(builder.Options);
        }
    }
}
