﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatForum.Models;

namespace CatForum.Data
{
    public class CatForumContext : DbContext
    {
        public CatForumContext (DbContextOptions<CatForumContext> options)
            : base(options)
        {
        }

        public DbSet<CatForum.Models.Discussion> Discussion { get; set; } 
        public DbSet<CatForum.Models.Comment> Comment { get; set; } 
    }
}
