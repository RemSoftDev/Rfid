using Rfid.Helpers;
using Rfid.Models;
using System;
using System.Data.Entity;
using System.IO;

namespace Rfid.Context
{
    public class RfidContext : DbContext
    {
        public DbSet<M_Users> C_Users { get; set; }
        public DbSet<M_Rfids> C_Rfids { get; set; }
        public DbSet<M_Phones> C_Phones { get; set; }
        public DbSet<M_Names> C_Names { get; set; }
        public DbSet<M_Departments> C_Departments { get; set; }
        public DbSet<M_UserTime> C_UserTime { get; set; }
        public DbSet<M_InOutValidTimes> C_InOutValidTimes { get; set; }
        public DbSet<M_Calendar> C_Calendar { get; set; }
        public DbSet<M_Setting> C_Setting { get; set; }
        public DbSet<M_EmailSetting> C_EmailSetting { get; set; }
        public DbSet<M_ExcelSetting> C_ExcelSetting { get; set; }
        public DbSet<M_LanguageSetting> C_LanguageSetting { get; set; }
        public DbSet<M_LimitTimerSetting> C_LimitTimerSetting { get; set; }

        public RfidContext ()
            :base("DefaultConnection")
        {
            if (Directory.Exists(Singelton.PathToDb) == false)
            {
                Directory.CreateDirectory(Singelton.PathToDb);
            }

            if (Directory.Exists(Singelton.PathToPhoto) == false)
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid\\Photo"))
                {
                    СryptographerHelper.Encrypt(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid\\Photo");
                }
                else
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid\\Photo");
                    СryptographerHelper.Encrypt(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid\\Photo");
                }
            }

            Database.Connection.ConnectionString = $"Data Source={Singelton.PathToDb}\\RfidDb.sdf";
            Database.SetInitializer<RfidContext>(new ContextInit());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Setting>()
            .HasOptional(f => f.P_ExcelSetting)
            .WithRequired(s => s.P_Setting);
            modelBuilder.Entity<M_Setting>()
            .HasOptional(f => f.P_EmailSetting)
            .WithRequired(s => s.P_Setting);
            modelBuilder.Entity<M_Setting>()
            .HasOptional(f => f.P_LanguageSetting)
            .WithRequired(s => s.P_Setting);
            modelBuilder.Entity<M_Setting>()
            .HasOptional(f => f.P_LimitTimerSetting)
            .WithRequired(s => s.P_Setting);
            base.OnModelCreating(modelBuilder);
        }

        

    }
}
