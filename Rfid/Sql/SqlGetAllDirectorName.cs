using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Sql
{
    public class SqlGetAllDirectorName
    {
        public readonly string SQl_GetAllDirectorName = @"SELECT * FROM [M_Names]
                                                             WHERE [P_Users_ID] IN
                                                             (SELECT [ID] FROM [M_Users]
                                                             WHERE [M_Users].[IsDirector] = 0)";

        public readonly string SQl_GetAllDirectorName_Test = @"SELECT * FROM [M_Names]
                                                             WHERE [P_Users_ID] IN
                                                             (SELECT [ID] FROM [M_Users]
                                                             WHERE [M_Users].[IsDirector] = 0)";
    }
}
