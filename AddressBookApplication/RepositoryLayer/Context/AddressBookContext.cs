using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options) { }

        // Define DbSet for Address Book

        public DbSet<UserEntity> AddressBook { get; set; }
        //public DbSet<AddressBookEntry> AddressBookEntries { get; set; }
    }
}
