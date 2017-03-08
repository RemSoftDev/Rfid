using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rfid.Sql;
using Rfid.AutoCompliteTextBox;

namespace Rfid.Helpers
{
    
    public class DirectorSearcherHelper
    {
        public List<Directors> Directors { get; set; } = new List<Directors>();

        public void FactoryDirectors()
        {
            RfidContext db = new RfidContext();

            Directors = (from der in db.C_Users
                         where (bool)der.IsDirector
                         select new Directors
                         {

                             NameDepart = der.P_Departments.Name,
                             CodeDepart = der.P_Departments.CodeFull,

                             First = der.P_Departments.DepartmentDirectorName.FirstOrDefault().NameFirst,
                             Last = der.P_Departments.DepartmentDirectorName.FirstOrDefault().NameLast,
                             Third = der.P_Departments.DepartmentDirectorName.FirstOrDefault().NameThird,
                             PhoneNumber = der.P_Departments.DepartmentDirectorPhone.FirstOrDefault().PhoneNumber
                         }).ToList();

            foreach (var dir in Directors)
            {
                dir.CreateFullSearchString();
               
            }

            db.Dispose();
        }
        public void SetAutoComplite(AutoCompleteTextBox tb)
        {
            foreach (var dir in Directors)
            {
                tb.AddItem(dir.GetAutoCompliteEntityt());
            }
        }

    }

    public class Directors
    {
        public string NameDepart { get; set; }
        public string CodeDepart { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Third { get; set; }
        public string PhoneNumber { get; set; }
        public string FullSearchString { get; set; }
        public void CreateFullSearchString()
        {
            FullSearchString = $"{NameDepart} {CodeDepart} {First} {Last} {Third}";
        }


        public AutoCompleteEntry GetAutoCompliteEntityt()
        {
            var list = new List<string>();
            string[] split = FullSearchString.Split(' ');
            List<string> list2 = CombinationsWithRepetitions(2, split);
            List<string> list3 = CombinationsWithRepetitions(3, split);
            List<string> list4 = CombinationsWithRepetitions(4, split);
            List<string> list5 = CombinationsWithRepetitions(5, split);
            list.AddRange(list2);
            list.AddRange(list3);
            list.AddRange(list4);
            list.AddRange(list5);
            AutoCompleteEntry res = new AutoCompleteEntry(FullSearchString, list.ToArray());
            return res;
        }
        public static List<string> CombinationsWithRepetitions(int length, string[] chars)
        {
            string result = String.Empty;
            List<string> res = new List<string>();
            int[] index = new int[length];
            bool work = true;

            while (work)
            {
                foreach (int i in index)
                {
                    result += chars[i];
                }

                res.Add(result);
                result = String.Empty;
                index[index.Length - 1]++;

                for (int i = (index.Length - 1); i >= 0; i--)
                {
                    if (index[i] > (chars.Length - 1))
                    {
                        index[i] = 0;
                        if (--i < 0)
                        {
                            work = false;
                            break;
                        }
                        index[i]++;
                        i++;
                    }
                }
            }

            return res;
        }
    }

}



