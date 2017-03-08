using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Context
{
    class ContextInit : CreateDatabaseIfNotExists<RfidContext>
    {
        protected override void Seed(RfidContext context)
        {
            M_Setting startSetting = new M_Setting();
            startSetting.ExportInBdSetting();
            context.C_Setting.Add(startSetting);

            M_Users FirstAdmin = new M_Users
            {
                P_Names = new List<M_Names> { new M_Names { NameFirst = "admin", NameLast = "admin", NameThird = "admin" } },
                IsUser = true,
                isInside = false,
                IsAdmin = true,
                P_Rfids = new List<M_Rfids> { new M_Rfids { Date = DateTime.Now, IsArhive = false, RfidID = 002427123 } },
                P_Departments = new M_Departments { Name = "admins", CodeFull = "001" },
                P_InOutValidTimes = new M_InOutValidTimes
                {
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Valid = DateTime.Now,
                    Dinner = DateTime.Now
                }
            };
           
            context.C_Users.Add(FirstAdmin);
            context.SaveChanges();
        }
    }
}
