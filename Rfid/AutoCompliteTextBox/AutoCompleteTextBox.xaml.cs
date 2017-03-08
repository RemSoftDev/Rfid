using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid.AutoCompliteTextBox
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AutoCompleteTextBox : Canvas
    {

        private VisualCollection controls;
        private TextBox textBox;
        private ComboBox comboBox;
        private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;

        public event EventHandler ItemSelected;

        public string SelectedItem { get; private set; }
        
        public ComboBox ComboBox
        {
            get
            {
                return comboBox;
            }
            set
            {
                comboBox = value ;
            }
        }    
        public string Text
        {
            get { return textBox.Text; }
            set
            {
                insertText = true;
                textBox.Text = value;
            }
        }
        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }
        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }
        public AutoCompleteTextBox()
        {
            controls = new VisualCollection(this);
            InitializeComponent();
            autoCompletionList = new ObservableCollection<AutoCompleteEntry>();
            searchThreshold = 0;        // default threshold to 2 char
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            // set up the text box and the combo box
            comboBox = new ComboBox();
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
            textBox = new TextBox();
            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.GotFocus += TextBox_GotFocus;
            textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            comboBox.Margin = new Thickness(15, 0, 15, 0);
            controls.Add(comboBox);
            controls.Add(textBox);
        }

        private void TextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBox_TextChanged(null, null);
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox_TextChanged(null, null);
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
                SelectedItem = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
                textBox.Text = cbItem.Content.ToString();
                //invoke event
                if (ItemSelected != null)
                {
                    ItemSelected.Invoke(null, null);
                }
            }
        }
       

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;
            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            textBox.Arrange(new Rect(arrangeSize));
            comboBox.Arrange(new Rect(arrangeSize));
            return base.ArrangeOverride(arrangeSize);
        }
        protected override Visual GetVisualChild(int index)
        {
            return controls[index];
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return controls.Count;
            }
        }
        public void AddItem(AutoCompleteEntry entry)
        {
            autoCompletionList.Add(entry);
        }
        private void TextChanged()
        {
            try
            {
                string searchStr="";
                foreach(Char a in textBox.Text)
                {
                    if(a!=' ')
                    {
                        searchStr += a;
                    }
                }

                comboBox.Items.Clear();
                foreach (AutoCompleteEntry entry in autoCompletionList)
                {
                    foreach (string word in entry.KeywordStrings)
                    {
                        if (word.StartsWith(searchStr, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ComboBoxItem cbItem = new ComboBoxItem();
                            cbItem.Content = entry.ToString();
                            comboBox.Items.Add(cbItem);
                            break;
                        }
                    }
                }
                comboBox.IsDropDownOpen = comboBox.HasItems;
            }
            catch { }
        }
    }
}
