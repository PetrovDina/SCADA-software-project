﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }

    
}
