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
    public class HelperUser
    {
        private int NumberLabelAddContactMan = 1;
        private int NumberLabelAddAddPhone = 1;


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

                grid.
                Children.
                Remove(child);
            }

            phone.Text = string.Empty;
        }

        public void AddPhone(Grid grid, TextBox item, int col, int row, List<string> phoneNumbersList)
        {

            string phoneForAdd = item.Text;

            int numberColumn = col;
            int numberRow = row;

            for (; numberRow < row + 2; numberRow++)
            {
                if (grid.
                    Children.
                    Cast<UIElement>().
                    Where(e1 => Grid.GetColumn(e1) == numberColumn && Grid.GetRow(e1) == numberRow).Count() == 0)
                {
                    Label labelUserPhone = new Label();
                    labelUserPhone.Content = phoneForAdd;

                    labelUserPhone.Name = "L_ContactPhone" + NumberLabelAddAddPhone.ToString();
                    NumberLabelAddAddPhone++;

                    Grid.SetColumn(labelUserPhone, numberColumn);
                    Grid.SetRow(labelUserPhone, numberRow);

                    grid.Children.Add(labelUserPhone);

                    numberRow = row + 2;

                    item.Text = string.Empty;

                    if (!string.IsNullOrEmpty(phoneForAdd))
                    {
                        phoneNumbersList.Add(phoneForAdd);
                    }
                }
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

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
                    if (foundChild != null) break;
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
            Grid grid,
            TextBox fn,
            TextBox sn,
            TextBox tn,
            TextBox phone,
            int row,
            List<M_Users> contactManList,
            List<string> phoneNumbersList)
        {
            int numberRow = row;
            phoneNumbersList.Add(phone.Text);

            for (; numberRow < row + 2; numberRow++)
            {
                for (; numberRow < row + 2; numberRow++)
                {
                    if (grid.
                        Children.
                        Cast<UIElement>().
                        Where(e1 => Grid.GetRow(e1) == numberRow).Count() == 0)
                    {
                        Label labelUserPhone = new Label();

                        labelUserPhone.Content = fn.Text + " " + sn.Text + " " + tn.Text + " " + string.Join(",", phoneNumbersList.ToArray());

                        labelUserPhone.Name = "L_Contact" + NumberLabelAddContactMan.ToString();
                        NumberLabelAddContactMan++;

                        Grid.SetColumn(labelUserPhone, 0);
                        Grid.SetRow(labelUserPhone, numberRow);

                        grid.Children.Add(labelUserPhone);

                        numberRow = row + 2;

                        fn.Text = string.Empty;
                        sn.Text = string.Empty;
                        tn.Text = string.Empty;

                        M_Names contactManNames = new M_Names { NameFirst = fn.Text, NameLast = sn.Text, NameThird = tn.Text };
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
                    }

                }
            }
            NumberLabelAddAddPhone = 3;
        }
    }
}
