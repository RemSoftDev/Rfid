using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Rfid.ViewModel
{
    class ViewModelContactMan : INotifyPropertyChanged
    {
        public ViewModelContactMan()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            AddContactMan = new RoutedUICommand("AddContactMan", "AddContactMan", typeof(ViewModelContactMan), inputs);
            AddPhone = new RoutedUICommand("AddPhone", "AddPhone", typeof(ViewModelContactMan), inputs);


            CommandBinding binding1 = new CommandBinding(AddContactMan);
            binding1.Executed += Binding_Executed;

            CommandBinding binding2 = new CommandBinding(AddPhone);
            binding2.Executed += Binding2_Executed; 

        }

        #region EventHandlers
        private void Binding2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void Binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("hey");
        }
        #endregion

        #region field
        private string _firstName { get; set; }
        private string _secondName { get; set; }
        private string _thirdName { get; set; }
        private string _phoneNumber1 { get; set; }
        private string _phoneNumber2 { get; set; }
        private string _phoneNumber3 { get; set; }
        #endregion 

        #region Property
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnChanged("FirstName");
            }
        }
        public string SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                _secondName = value;
                OnChanged("SecondName");
            }
        }
        public string ThirdName
        {
            get
            {
                return _thirdName;
            }
            set
            {
                _thirdName = value;
                OnChanged("ThirdName");
            }
        }

        public string PhoneNumber1
        {
            get
            {
                return _phoneNumber1;
            }
            set
            {
                _phoneNumber1 = value;
                OnChanged("PhoneNumber1");
            }
        }
        public string PhoneNumber2
        {
            get
            {
                return _phoneNumber2;
            }
            set
            {
                _phoneNumber2 = value;
                OnChanged("PhoneNumber2");
            }
        }
        public string PhoneNumber3
        {
            get
            {
                return _phoneNumber3;
            }
            set
            {
                _phoneNumber3 = value;
                OnChanged("PhoneNumber3");
            }
        }
        #endregion

        #region Comands
        //public ICommand AddContactMan { get; set; }
        //public ICommand AddPhone { get; set; }

        public static ICommand AddContactMan { get; set; }
        public static ICommand AddPhone { get; set; }

        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

