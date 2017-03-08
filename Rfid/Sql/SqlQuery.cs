using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Sql
{
    public class SqlBasicInformation
    {
        public readonly string SQl_BasicInformation = @"SELECT
                                                                [M_Users].[ID],
                                                                [M_Names].[NameFirst],
                                                                [M_Names].[NameLast],
                                                                [M_Names].[NameThird],
                                                                [M_Departments].[Name]
                                                                        As DepartmentName,
                                                                [M_InOutValidTimes].[Start],
                                                                [M_InOutValidTimes].[End],
                                                                [M_InOutValidTimes].[Valid],
                                                                [M_InOutValidTimes].[Dinner]
                                                                        FROM
                                                                [M_Users]
                                                                INNER JOIN[M_Names]
                                                                ON[M_Users].[ID] = [M_Names].[P_Users_ID]
                                                                        INNER JOIN[M_InOutValidTimes]
                                                                ON[M_InOutValidTimes].[ID] = [M_Users].[P_InOutValidTimes_ID]
                                                                        INNER JOIN[M_Departments]
                                                                ON[M_Departments].[ID] = [M_Users].[P_Departments_ID]
                                                                        WHERE[M_Users].[IsUser] = 1 AND [M_Users].[ID] != 1";

        public readonly string SQl_BasicInformation_Test = @"SELECT
                                                                [M_Users].[ID],
                                                                [M_Names].[NameFirst],
                                                                [M_Names].[NameLast],
                                                                [M_Names].[NameThird],
                                                                [M_Departments].[Name]
                                                                        As DepartmentName,
                                                                [M_InOutValidTimes].[Start],
                                                                [M_InOutValidTimes].[End],
                                                                [M_InOutValidTimes].[Valid],
                                                                [M_InOutValidTimes].[Dinner]
                                                                        FROM
                                                                [M_Users]
                                                                INNER JOIN[M_Names]
                                                                ON[M_Users].[ID] = [M_Names].[P_Users_ID]
                                                                        INNER JOIN[M_InOutValidTimes]
                                                                ON[M_InOutValidTimes].[ID] = [M_Users].[P_InOutValidTimes_ID]
                                                                        INNER JOIN[M_Departments]
                                                                ON[M_Departments].[ID] = [M_Users].[P_Departments_ID]
                                                                        WHERE[M_Users].[IsUser] = 1";
    }
}
