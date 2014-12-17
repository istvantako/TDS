using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace TDSFormApp
{
    public class TDSFormTextbox : TextBox
    {
        public string type
        {
            get;
            set;
        }
        private string validatedText;

        public void Validate()
        {
            switch (type.ToLower())
            {
                case "bit":
                    ParseBit();
                    break;
                case "tinyint":
                    ParseTinyint();
                    break;
                case "smallint":
                    ParseSmallint();
                    break;
                case "int":
                    ParseInt();
                    break;
                case "bigint":
                    ParseBigint();
                    break;
                case "decimal":
                    ParseDecimal();
                    break;
                case "numeric":
                    ParseDecimal();
                    break;
                case "smallmoney":
                    ParseSmallmoney();
                    break;
            }
        }
        private void ParseBit()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, "^[01tTfF]$|false|true"))
            {
                if (Regex.IsMatch(input, "[tT]"))
                {
                    validatedText = "";
                    this.Text = "true";
                }
                else
                {
                    if (Regex.IsMatch(input, "[fF]"))
                    {
                        validatedText = "";
                        this.Text = "false";
                    }
                    else
                    {
                        validatedText = input;
                    }
                }
            }
            else
            {
                this.Text = validatedText;
            }
            
        }
        private void ParseTinyint()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^\d{1,3}?$"))
                
            {
                int number = int.Parse(input);
                if ((0 <= number) && (number <= 255))
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }
        }
        private void ParseSmallint()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^-?\d{1,5}$"))
            {
                int number = int.Parse(input);
                if (-32768 <= number && number <= 32767)
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }
        }
        private void ParseInt()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^-?\d{0,10}$"))
            {
                int outp;
                if (int.TryParse(input, out outp))
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }
        }
        private void ParseBigint()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^-?\d{1,19}$"))
            {
                long outp;
                if (long.TryParse(input, out outp))
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }

        }
        private void ParseDecimal()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^-?(\d+(\.\d{1,18})?){1,30}$"))
            {
                decimal number;
                if (decimal.TryParse(input, out number))
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }
        }
        private void ParseSmallmoney()
        {
            string input = this.Text;
            if (Regex.IsMatch(input, @"^-?\d{1,6}\.\d(1,4}$"))
            {
                decimal number;
                number = decimal.Parse(input);
                if (-214748.3648m <= number && number <= 214748.3647m)
                {
                    validatedText = input;
                }
                else
                {
                    this.Text = validatedText;
                }
            }
            else
            {
                this.Text = validatedText;
            }
        }
    }
}
