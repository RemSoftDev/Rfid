using Rfid.Models;
using System.Data.Entity;

namespace Rfid.Context
{
    public class RfidContext : DbContext
    {
        public RfidContext ()
            :base("DefaultConnection")
        {

        }

        public DbSet<M_Users> C_Users { get; set; }
        public DbSet<M_Rfids> C_Rfids { get; set; }
        public DbSet<M_Phones> C_Phones { get; set; }
        public DbSet<M_Names> C_Names { get; set; }
        public DbSet<M_Departments> C_Departments { get; set; }
        public DbSet<M_UserTime> C_UserTime { get; set; }
        public DbSet<M_InOutValidTimes> C_InOutValidTimes { get; set; }

    }
}
