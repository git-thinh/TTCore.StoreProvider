using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTCore.StoreProvider.Data
{
    public class DbUserContext : DbContext
    {
        public DbUserContext(DbContextOptions<DbUserContext> options) : base(options) { }

        public DbSet<oUser> Users { get; set; }
        public DbSet<oConnection> Connections { get; set; }
        public DbSet<oConversationRoom> Rooms { get; set; }
    }

    public class oUser
    {
        [Key]
        public string UserName { get; set; }
        public ICollection<oConnection> Connections { get; set; }
        public virtual ICollection<oConversationRoom> Rooms { get; set; }
    }

    public class oConnection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }

    public class oConversationRoom
    {
        [Key]
        public string RoomName { get; set; }
        public virtual ICollection<oUser> Users { get; set; }
    }
}