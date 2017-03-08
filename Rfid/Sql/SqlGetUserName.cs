using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Sql
{
    public class SqlGetUserName
    {
        public readonly string SQl_GetUserFirstName = @"SELECT * FROM[M_Names]
                                                             WHERE[P_Users_ID] = {0}";

        public readonly string SQl_BasicInformation_Test = @"SELECT * FROM[M_Names]
                                                             WHERE[P_Users_ID] = 1";
    }
}
