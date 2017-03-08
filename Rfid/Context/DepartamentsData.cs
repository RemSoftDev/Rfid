using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Context
{
    class DepartamentsData
    {
        public string date { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public int countOfInside { get; set; }
        public int countOfOutside { get; set; }

        public DepartamentsData(string date, string name, int count, int countOfInside, int countOfOutside)
        {
            this.date = date;
            this.name = name;
            this.count = count;
            this.countOfInside = countOfInside;
            this.countOfOutside = countOfOutside;
        }


    }
}
