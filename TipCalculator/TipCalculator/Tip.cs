using System;

namespace TipCalculator
{
    public class Tip
    {
        public string billAmount { get; set; }
        public string tipAmount { get; set; }
        public string totalAmount { get; set; }

        public Tip()
        {
            this.billAmount = string.Empty;
            this.tipAmount = string.Empty;
            this.totalAmount = string.Empty;
        }
        public void CalculateTip(string originalAmount, double tipPersent)
        {
            double bill = 0.0;
            double tip = 0.0;
            double total = 0.0;
            if (double.TryParse(originalAmount.Replace('$', ' '), out bill))
            {
                tip = bill * tipPersent;
                total = bill + tip;
            }
            billAmount = string.Format("{0:C}", bill);
            tipAmount = string.Format("{0:C}", tip);
            totalAmount = string.Format("{0:C}", total);
        }
    }
}