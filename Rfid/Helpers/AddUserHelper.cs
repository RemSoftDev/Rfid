using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Rfid.Models;

namespace Rfid.Helpers
{
    internal class AddUserHelper
    {
        public static M_Users GetNewUserInfo(Grid userInfoGrid)
        {
            var user = new M_Users
            {
                P_Names = new List<M_Names>
                {
                    new M_Names
                    {
                        NameFirst = ((TextBox) userInfoGrid.FindName("TB_UserFN")).Text,
                        NameLast = ((TextBox) userInfoGrid.FindName("TB_UserSN")).Text,
                        NameThird = ((TextBox) userInfoGrid.FindName("TB_UserTN")).Text
                    }
                },
                Address = ((TextBox) userInfoGrid.FindName("TB_UsesrAddr")).Text,
                D_Birth = ((DateTimePicker) userInfoGrid.FindName("DP_UserBithday")).SelectedDate,
                IsAdmin = ((CheckBox) userInfoGrid.FindName("CB_IsAdmin")).IsChecked,
                IsDirector = ((CheckBox) userInfoGrid.FindName("CB_IsDirecor")).IsChecked,
                IsUser = true,
                P_Phones = GetNewPhones((TextBox) userInfoGrid.FindName("PhoneNumber1"),
                    (TextBox) userInfoGrid.FindName("PhoneNumber2"), (TextBox) userInfoGrid.FindName("PhoneNumber3"),
                    (TextBox) userInfoGrid.FindName("TB_UserPhone"))
            };

            return user;
        }

        public static void UpdateUserInfo(Grid userInfoGrid, M_Users user)
        {
            user.P_Names.FirstOrDefault().NameFirst = ((TextBox) userInfoGrid.FindName("TB_UserFN")).Text;
            user.P_Names.FirstOrDefault().NameLast = ((TextBox) userInfoGrid.FindName("TB_UserSN")).Text;
            user.P_Names.FirstOrDefault().NameThird = ((TextBox) userInfoGrid.FindName("TB_UserTN")).Text;
            user.Address = ((TextBox) userInfoGrid.FindName("TB_UsesrAddr")).Text;
            user.D_Birth = ((DateTimePicker) userInfoGrid.FindName("DP_UserBithday")).SelectedDate;
            user.IsAdmin = ((CheckBox) userInfoGrid.FindName("CB_IsAdmin")).IsChecked;
            user.IsDirector = ((CheckBox) userInfoGrid.FindName("CB_IsDirecor")).IsChecked;
            var newPhones = GetNewPhones((TextBox) userInfoGrid.FindName("PhoneNumber1"),
                (TextBox) userInfoGrid.FindName("PhoneNumber2"), (TextBox) userInfoGrid.FindName("PhoneNumber3"),
                (TextBox) userInfoGrid.FindName("TB_UserPhone"));
            var oldPhones = user.P_Phones.ToList();
            for (var i = 0; i < user.P_Phones.Count; i++)
            {
                oldPhones[i].PhoneNumber = newPhones[i].PhoneNumber;
            }
            user.P_Phones = oldPhones;
        }

        public static List<M_Users> GetAllContactsInfo(Grid contactManInfoGrid, M_Users newUser)
        {
            var contacts = new List<M_Users>();

            if (((Grid) contactManInfoGrid.FindName("CMFirstShow")).Visibility == Visibility.Visible)
            {
                contacts.Add(GetContactMan(
                    (TextBox) contactManInfoGrid.FindName("CMFirstName1"),
                    (TextBox) contactManInfoGrid.FindName("CMSecondName1"),
                    (TextBox) contactManInfoGrid.FindName("CMThirdName1"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone11"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone12"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone13"), newUser));
            }

            if (((Grid) contactManInfoGrid.FindName("CMSecondShow")).Visibility == Visibility.Visible)
            {
                contacts.Add(GetContactMan(
                    (TextBox) contactManInfoGrid.FindName("CMFirstName2"),
                    (TextBox) contactManInfoGrid.FindName("CMSecondName2"),
                    (TextBox) contactManInfoGrid.FindName("CMThirdName2"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone21"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone22"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone23"), newUser));
            }

            if (((Grid) contactManInfoGrid.FindName("CMThirdShow")).Visibility == Visibility.Visible)
            {
                contacts.Add(GetContactMan(
                    (TextBox) contactManInfoGrid.FindName("CMFirstName3"),
                    (TextBox) contactManInfoGrid.FindName("CMSecondName3"),
                    (TextBox) contactManInfoGrid.FindName("CMThirdName3"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone31"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone32"),
                    (TextBox) contactManInfoGrid.FindName("CMPhone33"), newUser));
            }

            if (contacts.Count == 0)
            {
                contacts.Add(GetContactMan(
                    (TextBox) contactManInfoGrid.FindName("TB_FN"), (TextBox) contactManInfoGrid.FindName("TB_SN"),
                    (TextBox) contactManInfoGrid.FindName("TB_TN"),
                    (TextBox) contactManInfoGrid.FindName("TB_ContacnManPhone"),
                    new TextBox {Text = string.Empty}, new TextBox {Text = string.Empty}, newUser));
            }

            return contacts;
        }

        public static void UpdateContactsInfo(Grid contactManInfoGrid, M_Users User)
        {
            var newContactsList = GetAllContactsInfo(contactManInfoGrid, User);
            var baseContactsList = User.P_ManForContact.ToList();

            for (var i = 0; i < baseContactsList.Count; i++)
            {
                baseContactsList[i].P_Names.FirstOrDefault().NameFirst =
                    newContactsList[i].P_Names.FirstOrDefault().NameFirst;
                baseContactsList[i].P_Names.FirstOrDefault().NameLast =
                    newContactsList[i].P_Names.FirstOrDefault().NameLast;
                baseContactsList[i].P_Names.FirstOrDefault().NameThird =
                    newContactsList[i].P_Names.FirstOrDefault().NameThird;
                var basePhones = baseContactsList[i].P_Phones.ToList();
                var newPhones = newContactsList[i].P_Phones.ToList();
                for (var j = 0; j < basePhones.Count; j++)
                {
                    basePhones[i].PhoneNumber = newPhones[i].PhoneNumber;
                }
            }
            User.P_ManForContact = baseContactsList;
        }

        public static M_Departments GetDepartmentInfo(Grid departmentInfoGrid, M_Users user)
        {
            var department = new M_Departments
            {
                Name = ((TextBox)departmentInfoGrid.FindName("TB_DepName")).Text,
                CodeFull = ((TextBox)departmentInfoGrid.FindName("TB_DepCode")).Text
            };
            if (user.IsDirector != null && (bool) user.IsDirector)
            {
                department.DepartmentDirectorName = user.P_Names;
                department.DepartmentDirectorPhone = user.P_Phones;
            }
            else
            {
                department.DepartmentDirectorName = new List<M_Names>
                {
                    new M_Names
                    {
                        NameFirst = ((TextBox) departmentInfoGrid.FindName("TB_DepFN")).Text,
                        NameLast = ((TextBox) departmentInfoGrid.FindName("TB_DepSN")).Text,
                        NameThird = ((TextBox) departmentInfoGrid.FindName("TB_DepTN")).Text
                    }
                };
                department.DepartmentDirectorPhone =
                    GetNewPhones((TextBox) departmentInfoGrid.FindName("DepartmentPhoneNumber1"),
                        (TextBox) departmentInfoGrid.FindName("DepartmentPhoneNumber2"),
                        (TextBox) departmentInfoGrid.FindName("DepartmentPhoneNumber3"),
                        (TextBox) departmentInfoGrid.FindName("TB_DirectorPhone"));
                department.P_Users = new List<M_Users>
                {
                    new M_Users
                    {
                        IsDirector = true
                    }
                };
            }

            return department;
        }

        public static void UpdateDepartmentInfo(Grid departmentInfoGrid, M_Users user)
        {
            user.P_Departments.CodeFull = ((TextBox) departmentInfoGrid.FindName("TB_DepCode")).Text;
            user.P_Departments.Name = ((TextBox) departmentInfoGrid.FindName("TB_DepName")).Text;
            user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameFirst =
                ((TextBox) departmentInfoGrid.FindName("TB_DepFN")).Text;
            user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameLast =
                ((TextBox) departmentInfoGrid.FindName("TB_DepSN")).Text;
            user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameThird =
                ((TextBox) departmentInfoGrid.FindName("TB_DepTN")).Text;
        }

        public static M_InOutValidTimes GetInOutValidTimes(Grid workTimeInfoGrid)
        {
            var validInOutTime = new M_InOutValidTimes
            {
                Start = GetValidTime((TimePicker) workTimeInfoGrid.FindName("TB_Start")),
                End = GetValidTime((TimePicker) workTimeInfoGrid.FindName("TB_End")),
                Valid = GetValidTime((TimePicker) workTimeInfoGrid.FindName("TB_Valid")),
                Dinner = GetValidTime((TimePicker) workTimeInfoGrid.FindName("TB_Dinner"))
            };

            return validInOutTime;
        }

        public static void UpdateInOutTimes(Grid workTimeInfoGrid, M_Users user)
        {
            var newTimes = GetInOutValidTimes(workTimeInfoGrid);

            user.P_InOutValidTimes.Start = newTimes.Start;
            user.P_InOutValidTimes.End = newTimes.End;
            user.P_InOutValidTimes.Valid = newTimes.Valid;
            user.P_InOutValidTimes.Dinner = newTimes.Dinner;
        }

        public static M_Rfids GetRfidInfo(Grid rfidGrid)
        {
            var rfid = new M_Rfids
            {
                RfidID = Convert.ToInt64(((TextBox) rfidGrid.FindName("ShowRfid")).Text),
                Date = DateTime.Now
            };

            return rfid;
        }

        public static string GetPhotoPath(M_Users user, bool isImageExists, string img)
        {
            if (!isImageExists) return null;
            if (string.IsNullOrEmpty(img)) return null;
            var imageFile = new FileInfo(img);
            var imageName = string.Empty;
            imageName = string.Format("{0}_{1}_{2}.{3}",
                user.ID, 
                user.P_Names.FirstOrDefault().NameFirst.Replace(" ", string.Empty),
                user.P_Names.FirstOrDefault().NameLast.Replace(" ", string.Empty),
                imageFile.ToString().Split('.').Last()
                );
           
            var dir = new DirectoryInfo(Singelton.PathToPhoto);

            if (!dir.Exists)
                dir.Create();
            var source = img;
            var dest = Path.Combine(dir.FullName, imageName);
            //File.Delete(dest);
            System.IO.File.Copy(img, dest, true);
            //imageFile.CopyTo(Path.Combine(dir.FullName, imageName), true);
            return imageName;
        }

        private static List<M_Phones> GetNewPhones(TextBox textBox1, TextBox textBox2, TextBox textBox3,
            TextBox textBox4)
        {
            var phonesList = new List<M_Phones>();
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    phonesList.Add(new M_Phones {PhoneNumber = textBox1.Text});
                }
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    phonesList.Add(new M_Phones {PhoneNumber = textBox2.Text});
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    phonesList.Add(new M_Phones {PhoneNumber = textBox3.Text});
                }
            }
            else
            {
                phonesList.Add(new M_Phones {PhoneNumber = textBox4.Text});
            }

            return phonesList;
        }

        private static DateTime GetValidTime(TimePicker tp)
        {
            var standartDate = new DateTime(2000, 01, 01);
            var time1 = (TimeSpan) tp.SelectedTime;
            return (standartDate + time1);
        }

        private static M_Users GetContactMan(TextBox firstName, TextBox secondName, TextBox thirdName,
            TextBox contactPhone1, TextBox contactPhone2, TextBox contactPhone3, M_Users newUser)
        {
            var newContact = new M_Users
            {
                P_Names = new List<M_Names>
                {
                    new M_Names
                    {
                        NameFirst = firstName.Text,
                        NameLast = secondName.Text,
                        NameThird = thirdName.Text
                    }
                },
                IsUser = false,
                IsDirector = false,
                isInside = true,
                Address = "",
                Photo = "",
                D_Birth = null,
                P_Rfids = null,
                P_ManForContact = null,
                P_Phones = GetNewPhones(contactPhone1, contactPhone2, contactPhone3, new TextBox {Text = string.Empty}),
                P_UserTime = null,
                P_InOutValidTimes = null
            };
            //Рядок в юзерах, що вказує на нейм
            newContact.P_Names.FirstOrDefault().P_Users = newContact;

            //для якого юзера цей контакт
            newContact.P_Users = newUser;
            return newContact;
        }
    }
}
