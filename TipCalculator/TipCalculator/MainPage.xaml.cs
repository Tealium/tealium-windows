using System.Collections.Generic;
using System.Linq;
using TealiumUWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TipCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Tip tip;
        public MainPage()
        {
            InitializeComponent();
            tip = new Tip();
        }

        /*
         *        Radio button click listener that is calling track function of Tealium library
         */

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            PerformCalculation();
            var selectedRadio = myStackPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);

            /*
            *        Papulating Optional Data Dictionary
            */

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Service-Feedback", selectedRadio.Tag.ToString());

            string name = "Service_Selector_radio_btn";

            /*
            *        Calling track function of Tealium library
            *        Params : 
            *           name - Manadatory parameter containing event name
            *           data - Optional Dictionary of data __Key,Value__ form
            *           CallBackFunction - Optional callback function reference 
            */

            //    Tealium.SetSessionId("123456789876");
            //    Tealium.SetvisitorId("1234567890653");

            //    Tealium.Track(name);
            //    Tealium.Track(name, data);
            //    Tealium.Track(name, CallBackFunction);

            name = name.Trim(' ') != "" ? name : "event name";
            Tealium.Track(name , data, CallBackFunction);
        }


        /*
        *        CallBackFunction 
        */

        public void CallBackFunction(bool c)
        {
            int a, b;
            a = 1;
            b = a;
        }

        /*
        *        Text Changed Event Handler
        */

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformCalculation();
        }

        private void PerformCalculation()
        {
            var selectedRadio = myStackPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            tip.CalculateTip(billAmountTextBox.Text, double.Parse(selectedRadio.Tag.ToString()));
            amounttotip.Text = tip.tipAmount;
            totalBillAmount.Text = tip.totalAmount;
        }
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            billAmountTextBox.Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            billAmountTextBox.Text = tip.billAmount;
        }
    }
}
