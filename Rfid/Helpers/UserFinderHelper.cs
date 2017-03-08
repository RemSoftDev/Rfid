using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Helpers
{
    public class UserFinderHelper
    {
        private IQueryable<M_Users> _query;
        private bool _findForFN;
        private bool _findForSN;
        private bool _findForTN;
        private bool _findForPN;
        private bool _findForDN;
        private bool _findForDD;
        private bool _findForAdrs;
        public RfidContext Context { get; set; }

        public string FirstName { get; set; }
        public string SecondName{ get; set; }
        public string ThirdName { get; set; }
        public string PhoneNumber { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDirector { get; set; }
        public string Address { get; set; }


        private void SetFinder(string str,out bool finder)
        {
            finder = !String.IsNullOrEmpty(str);
        }
        private void SetAllFinder()
        {
            SetFinder(FirstName,out  _findForFN);
            SetFinder(SecondName, out _findForSN);
            SetFinder(ThirdName, out _findForTN);
            SetFinder(PhoneNumber, out _findForPN);
            SetFinder(DepartmentName, out _findForDN);
            SetFinder(DepartmentDirector, out _findForDD);
            SetFinder(Address, out _findForAdrs);
        }
        private void SetQuery()
        {
                _query = from x in Context.C_Users
                         where x.IsUser == true
                         select x;

                if (_findForFN)
                {
                    _query = from x in _query
                             where x.P_Names.FirstOrDefault().NameFirst.Contains(FirstName)
                             select x;
                }
                if (_findForSN)
                {
                    _query = from x in _query
                             where x.P_Names.FirstOrDefault().NameLast.Contains(SecondName)
                             select x;
                }
                if (_findForTN)
                {
                    _query = from x in _query
                             where x.P_Names.FirstOrDefault().NameThird.Contains(ThirdName)
                             select x;
                }
                if (_findForPN)
                {
                    _query = from x in _query
                             where x.P_Phones.Contains((from x_p in x.P_Phones
                                                        where x_p.PhoneNumber.Contains(PhoneNumber)
                                                        select x_p).FirstOrDefault())
                             select x;
                }
                if (_findForDN)
                {
                    _query = from x in _query
                             where x.P_Departments.Name.Contains(DepartmentName)
                             select x;
                }
                if (_findForDD)
                {
                    _query = from x in _query
                             where x.P_Departments
                             .DepartmentDirectorName
                             .FirstOrDefault()
                             .NameFirst
                             .Contains(DepartmentDirector) ||

                             x.P_Departments
                             .DepartmentDirectorName
                             .FirstOrDefault()
                             .NameLast
                             .Contains(DepartmentDirector) ||

                             x.P_Departments
                             .DepartmentDirectorName
                             .FirstOrDefault()
                             .NameThird
                             .Contains(DepartmentDirector)

                             select x;
                }
            if (_findForAdrs)
            {
                _query = from x in _query
                         where x.Address.Contains(Address)
                         select x;
            }
        }
        public bool TryFind()
        {
            if (this.Find().Count() == 0)
                return false;
            else
                return true;
        }
        public IQueryable<M_Users> Find()
        {
            SetAllFinder();
            SetQuery();
            return _query;
        }
    }
}
