using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Sql
{
    public class SqlSearchRfidForRfidNumber
    {
        public readonly string SQl_SearchUserForRfid = @"SELECT * FROM [M_Rfids]
                                                         WHERE [RfidID] = {0} AND [P_Users_ID] IS NOT NULL ";

        public readonly string SQl_SearchUserForRfid_Test = @"SELECT * FROM [M_Rfids]
                                                            WHERE [RfidID] = 3357176 AND [P_Users_ID] IS NOT NULL ";
    }
}
