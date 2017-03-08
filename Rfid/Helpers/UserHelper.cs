using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rfid.Helpers
{
    public class UserHelper
    {
        private void setNumberInTextBox(TextBox fOPTextBox, TextBox sOPTextBox, TextBox tOPTextBox, string newNumber)
        {
            if(fOPTextBox.Text == String.Empty )
            {
                fOPTextBox.Text = newNumber;
            }
            else
                if(sOPTextBox.Text == String.Empty)
                {
                    sOPTextBox.Text = newNumber;
                }
                else
                    if (sOPTextBox.Text == String.Empty)
                    {
                        sOPTextBox.Text = newNumber;
                    }
        }
        private void setFullNameInTextBox(TextBox sTextBox,
                                          TextBox fTextBox,
                                          string fName,
                                          string sName,
                                          string tName)
        {
            if (fName != "")
                if (fTextBox.Text.IndexOf("FirstName") != -1)
                {
                    fTextBox.Text = fTextBox.Text.Replace("FirstName", fName);
                }
                else if (sTextBox.Text.IndexOf("FirstName") != -1)
                {
                    sTextBox.Text = sTextBox.Text.Replace("FirstName", fName);
                }

            if (sName != "")
            {
                if (fTextBox.Text.IndexOf("SecondName") != -1)
                {
                    fTextBox.Text = fTextBox.Text.Replace("SecondName", sName);
                }
                else if (sTextBox.Text.IndexOf("SecondName") != -1)
                {
                    sTextBox.Text = sTextBox.Text.Replace("SecondName", sName);
                }
            }


            if (tName != "")
            {
                if (fTextBox.Text.IndexOf("ThirdName") != -1)
                {
                    fTextBox.Text = fTextBox.Text.Replace("ThirdName", tName);
                }
                else if (sTextBox.Text.IndexOf("ThirdName") == -1)
                {
                    sTextBox.Text = sTextBox.Text.Replace("ThirdName", tName);
                }
            }
        }
        public void ClearPhones(Grid grid, int col, int row, TextBox phone)
        {
            int numberColumn = col;
            int numberRow = row;

            for (; numberRow < 5; numberRow++)
            {
                var child = grid.
                            Children.
                            Cast<UIElement>().
                            Where(e1 => Grid.GetColumn(e1) == numberColumn && Grid.GetRow(e1) == numberRow).FirstOrDefault();

                grid.Children.Remove(child);
            }

            phone.Text = string.Empty;
        }  
        public void AddPhone(TextBox inputTextBox,
                             TextBox firstOutPutTextBox,
                             TextBox secondOutPutTextBox,
                             TextBox thirdOutPutTextBox,
                             List<string> phoneNumbersList)
        {
            string newPhoneNumber;

            if (inputTextBox.Text != String.Empty)
            {
                newPhoneNumber = inputTextBox.Text;
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["msb_notEnterNumber"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            phoneNumbersList.Add(newPhoneNumber);
            setNumberInTextBox(firstOutPutTextBox,secondOutPutTextBox,thirdOutPutTextBox,newPhoneNumber);
        }
        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;

                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);
                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;

                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        public void AddContactMan(
           TextBox inputFirstName,
           TextBox inputSecondName,
           TextBox inputThirdName,
           TextBox inputPhoneNumber,
           TextBox outputTSFName1,
           TextBox outputTSDName2,
           List<M_Users> contactManList,
           List<string> phoneNumbersList)
        {
            //set names
            string firstName = inputFirstName.Text;
            string secondName = inputSecondName.Text;
            string thirdName = inputThirdName.Text;
            inputFirstName.Text = String.Empty;
            inputSecondName.Text = String.Empty;
            inputThirdName.Text = String.Empty;
            //set phones
            string phoneNumber = inputPhoneNumber.Text;

            if(!String.IsNullOrEmpty(phoneNumber))
            {
                ///
            }

            string fullNames = firstName + " " + secondName + " " + thirdName; 
            M_Names contactManNames = new M_Names { NameFirst = firstName, NameLast = secondName, NameThird = thirdName };
            List<M_Names> contactManNamesList = new List<M_Names>();
            contactManNamesList.Add(contactManNames);
            List<M_Phones> contactManPhones = new List<M_Phones>();

            foreach (string item in phoneNumbersList)
            {
                contactManPhones.Add(new M_Phones { PhoneNumber = item });
            }

            M_Users contactMan = new M_Users
            {
                P_Names = contactManNamesList,
                P_Phones = contactManPhones
            };

            contactManList.Add(contactMan);
            setFullNameInTextBox(outputTSFName1,
                                 outputTSDName2, 
                                 firstName,
                                 secondName,
                                 thirdName);
        }
    }
}
