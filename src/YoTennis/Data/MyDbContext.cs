﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MatchEvent>().HasKey(matchEvent => new { matchEvent.MatchId, matchEvent.Version });
            builder.Entity<MatchInfo>().HasKey(matchInfo => matchInfo.MatchId);
            //builder.Entity<MatchInfo>().HasIndex(matchInfo => matchInfo.UserId);
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<MatchInfo> MatchInfos { get; set; }
    }
}
