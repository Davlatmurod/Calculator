using System;
using System.Collections.Generic;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Double num1 = 0;
        Double num2 = 0;
        String sign = "";
        int cnt_comma_num1 = 0;
        int cnt_comma_num2 = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = BitmapFrame.Create(Application.GetResourceStream(new Uri("icons/icons8-calc-3.png",
UriKind.RelativeOrAbsolute)).Stream);

            clear.IsEnabled = false;
            add.IsEnabled = false;
            subtract.IsEnabled = false;
            mult.IsEnabled = false;
            div.IsEnabled = false;
        }

        /*If clicked new number*/
        public void add_new_number(int add_new_number)
        {

            string text = calc_label.Content.ToString();
            if (sign == "")
            {
                num1 = num1 * 10 + add_new_number;
                
                /*Not possible 0 add again 0(for example 00)*/
                if (num1 == 0 && add_new_number == 0 && text.Length > 0 && text[text.Length -1] == '0') return;

                /*Not possible 0 add new number (for example 08)*/
                if (num1 == add_new_number && text.Length > 0 && cnt_comma_num1 == 0) 
                    calc_label.Content = text.Substring(0, text.Length - 1);

                /*cnt values after (,)*/
                if (cnt_comma_num1 > 0) cnt_comma_num1++;

                clear.IsEnabled = true;
                add.IsEnabled = true;
                subtract.IsEnabled = true;
                mult.IsEnabled = true;
                div.IsEnabled = true;
            }
            else
            {
                num2 = num2 * 10 + add_new_number;

                /*Not possible 0 add again 0(for example 00)*/
                if (num2 == 0 && add_new_number == 0 && text.Length > 0 && text[text.Length - 1] == '0') return;

                /*Not possible 0 add new number (for example 08)*/
                if (num2 == add_new_number && text.Length > 0 && text[text.Length - 1] == '0' && cnt_comma_num2 == 0) 
                    calc_label.Content = text.Substring(0, text.Length - 1);

                /*cnt values after (,)*/
                if (cnt_comma_num2 > 0) cnt_comma_num2++;

                /*Possible for calculation*/
                calc.IsEnabled = true;
            }
            calc_label.Content += add_new_number.ToString();
        }
        public void sign_click(String s)
        {
            sign = s;
            string text = calc_label.Content.ToString();
            
            /*Check impossible format (12,+)*/
            if (text.Length > 0 && text[text.Length - 1] == ',')
                calc_label.Content = text.Substring(0, text.Length - 1);

            calc_label.Content += s; 

            comma.IsEnabled = true;

            /*User can only one operation*/
            add.IsEnabled = false;
            subtract.IsEnabled = false;
            mult.IsEnabled = false;
            div.IsEnabled = false;
        }

        public void clicked_comma()
        {
            string text = calc_label.Content.ToString();
            if (sign == "") cnt_comma_num1++;
            else cnt_comma_num2 ++;

            if (text == "") calc_label.Content += "0";
            if (text.Length > 0 && (text[text.Length - 1] == '+' || text[text.Length - 1] == '-' || text[text.Length - 1] == '*' || text[text.Length - 1] == '/')) calc_label.Content += "0";
            calc_label.Content += ",";

            comma.IsEnabled = false;
        }

        public void clear_last()
        {
            string text = calc_label.Content.ToString();
            if (sign == "")
            {
                num1 = (Convert.ToInt64(num1) / 10);
                if(cnt_comma_num1 > 0) cnt_comma_num1--;
            }
            else
            {
                if (text.Length > 0 && (text[text.Length - 1] == '+' || text[text.Length - 1] == '-' || text[text.Length - 1] == '*' || text[text.Length - 1] == '/'))
                {
                    sign = "";

                    /*Impossible for calculation*/
                    calc.IsEnabled = false;

                    /*User can only one operation*/
                    add.IsEnabled = true;
                    subtract.IsEnabled = true;
                    mult.IsEnabled = true;
                    div.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show(num2.ToString() + " " + cnt_comma_num2.ToString());
                    num2 = (Convert.ToInt64(num2) / 10);
                    MessageBox.Show(num2.ToString() + " "+ cnt_comma_num2.ToString());
                    if (cnt_comma_num2 > 0) cnt_comma_num2--;
                    if (cnt_comma_num2 == 0) comma.IsEnabled = true; 
                }
            }
            if (text.Length > 0)
            {
                calc_label.Content = text.Substring(0, text.Length - 1);
            }
            if (calc_label.Content.ToString().Length == 0) clear.IsEnabled = false;
            else clear.IsEnabled = true;
        }

        public void calculation()
        {
            try
            {
                /*Calculated , for absolute value most one*/
                if (cnt_comma_num1 > 0) cnt_comma_num1--;
                if (cnt_comma_num2 > 0) cnt_comma_num2--;

                /*If have comma in values recalculate*/
                num1 = num1 / Math.Pow(10, cnt_comma_num1);
                num2 = num2 / Math.Pow(10, cnt_comma_num2);

                if (sign == "+") calc_label.Content = (num1 + num2).ToString();
                else if (sign == "-") calc_label.Content = (num1 - num2).ToString();
                else if (sign == "*") calc_label.Content = (num1 * num2).ToString();
                else if (sign == "/") calc_label.Content = (num1 * 1.0 / num2).ToString();

                sign = "";
                num2 = 0;

                /*Save result to num1*/
                num1 = Convert.ToDouble(calc_label.Content.ToString());
                cnt_comma_num1 = 0;
                cnt_comma_num2 = 0;
                while (num1 != Convert.ToInt64(num1))
                {
                    num1 *= 10;
                    cnt_comma_num1++;
                }
                if (cnt_comma_num1 > 0) cnt_comma_num1++;
                

                /*Impossible for calculation*/
                calc.IsEnabled = false;

                /*Can possible put (,)*/
                if (cnt_comma_num1 == 0) comma.IsEnabled = true;
                else comma.IsEnabled = false;

                /*User can only one operation*/
                add.IsEnabled = true;
                subtract.IsEnabled = true;
                mult.IsEnabled = true;
                div.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(1);
        }

        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(2);
        }

        private void btn_3_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(3);
        }

        private void btn_4_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(4);
        }

        private void btn_5_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(5);
        }

        private void btn_6_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(6);
        }

        private void btn_7_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(7);
        }

        private void btn_8_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(8);
        }

        private void btn_9_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(9);
        }

        private void btn_0_Click(object sender, RoutedEventArgs e)
        {
            add_new_number(0);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            sign_click("+");
        }

        private void subtract_Click(object sender, RoutedEventArgs e)
        {
            sign_click("-");
        }

        private void mult_Click(object sender, RoutedEventArgs e)
        {
            sign_click("*");
        }

        private void div_Click(object sender, RoutedEventArgs e)
        {
            sign_click("/");
        }

        private void calc_Click(object sender, RoutedEventArgs e)
        {
            calculation();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            clear_last();
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {
            num1 = 0;
            num2 = 0;
            calc_label.Content = "".ToString();
        }

        private void comma_Click(object sender, RoutedEventArgs e)
        {
            clicked_comma();
        }
    }
}