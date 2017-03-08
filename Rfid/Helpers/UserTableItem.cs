using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rfid.Helpers
{
    public class UserTableItem
    {
        public int Id { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameThird { get; set; }
        public string DepartmentName { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Valid { get; set; }
        public string Dinner { get; set; }

        public static List<UserTableItem> GenerateList(DataGrid dg)
        {
            List<UserTableItem> res = new List<UserTableItem>();

            foreach (var obj in dg.ItemsSource)
            {
                UserTableItem item = new UserTableItem();
                System.Type type = obj.GetType();
                item.Id = (int)type.GetProperty("ID").GetValue(obj, null);
                item.NameFirst = (string)type.GetProperty("NameFirst").GetValue(obj, null);
                item.NameLast = (string)type.GetProperty("NameLast").GetValue(obj, null);
                item.NameThird = (string)type.GetProperty("NameThird").GetValue(obj, null);
                item.DepartmentName = (string)type.GetProperty("DepartmentName").GetValue(obj, null);
                item.Start = ((DateTime)type.GetProperty("Start").GetValue(obj, null)).ToString("hh:mm");
                item.End = ((DateTime)type.GetProperty("End").GetValue(obj, null)).ToString("hh:mm");
                item.Valid = ((DateTime)type.GetProperty("Valid").GetValue(obj, null)).ToString("hh:mm");
                res.Add(item);
            }

            return res;
        }
    }

    public class TimeTableItem
    {
        public int ID { get; set; }
        public DateTime SingleDate { get; set; }
        public string Day { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; } 
        public string LengthOfInside { get; set; }
        public string LengthOfOutside { get; set; }

        public static List<TimeTableItem> GenerateList(DataGrid dg)
        {
            List<TimeTableItem> res = new List<TimeTableItem>();

            foreach (var obj in dg.ItemsSource)
            {
                TimeTableItem item = new TimeTableItem();
                System.Type type = obj.GetType();

                item.ID = (int)type.GetProperty("ID").GetValue(obj, null);
                item.SingleDate = (DateTime)type.GetProperty("SingleDate").GetValue(obj, null);
                item.Day = (string)type.GetProperty("Day").GetValue(obj, null);
                item.TimeIn = ((DateTime)type.GetProperty("TimeIn").GetValue(obj, null)).ToString("hh:mm");

                if (type.GetProperty("TimeOut").GetValue(obj, null) == null)
                {
                    item.TimeOut = string.Empty;
                }
                else
                {
                    item.TimeOut = ((DateTime)type.GetProperty("TimeOut").GetValue(obj, null)).ToString("hh:mm");
                }

                if (type.GetProperty("LengthOfInside").GetValue(obj, null)==null)
                {
                    item.LengthOfInside = string.Empty;
                }
                else
                {
                    item.LengthOfInside = ((DateTime)type.GetProperty("LengthOfInside").GetValue(obj, null)).ToString("hh:mm");
                }

                if (type.GetProperty("LengthOfOutside").GetValue(obj, null) == null)
                {
                    item.LengthOfInside = string.Empty;
                }
                else
                {
                    item.LengthOfOutside = ((DateTime)type.GetProperty("LengthOfOutside").GetValue(obj, null)).ToString("hh:mm");
                }
                
                res.Add(item);
            }
            return res;
        }
    }

    public class DepartmentTableItem
    { 
        public string Date { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int CountInside { get; set; }
        public int CountOutside { get; set; }

        public static List<DepartmentTableItem> GenerateList(DataGrid dg)
        {
            List<DepartmentTableItem> res = new List<DepartmentTableItem>();

            foreach (var obj in dg.ItemsSource)
            {
                DepartmentTableItem item = new DepartmentTableItem();
                System.Type type = obj.GetType();
                item.Date = (string)type.GetProperty("date").GetValue(obj, null);
                item.Name = (string)type.GetProperty("name").GetValue(obj, null);
                item.Count = (int)type.GetProperty("count").GetValue(obj, null);
                item.CountInside = (int)type.GetProperty("countOfInside").GetValue(obj, null);
                item.CountOutside = (int)type.GetProperty("countOfOutside").GetValue(obj, null);    
                res.Add(item);
            }

            return res;
        }
    }

    public class BasicInformation
    {
        public int ID { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameThird { get; set; }
        public string DepartmentName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Valid { get; set; }
        public DateTime? Dinner { get; set; }
    }


}
