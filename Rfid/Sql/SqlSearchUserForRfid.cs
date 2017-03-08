using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Sql
{
    public class SqlSearchUserForRfid
    {
        public readonly string SQl_SearchUserForRfid = @"SELECT * FROM [M_Users]
                                                            WHERE [M_Users].[ID] IN
                                                            (SELECT DISTINCT [P_Users_ID] FROM [M_Rfids]
                                                                 WHERE [M_Rfids].[RfidID]= {0})";

        public readonly string SQl_SearchUserForRfid_Test = @"SELECT * FROM [M_Users]
                                                            WHERE [M_Users].[ID] IN
                                                            (SELECT DISTINCT [P_Users_ID] FROM [M_Rfids]
                                                                 WHERE [M_Rfids].[RfidID]= 4552259)";
    }
}
