using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinCity.AbstractRepository
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }
    }
}
