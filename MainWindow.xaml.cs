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
using System.Diagnostics;
using System.Globalization;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private const string V = "nie dziel przez zero";
        decimal _results = 0;
        String _operation = "";
        bool _isTyping = false;
        bool _zero = false;
        bool _isEqualized = false;
        decimal _eqval = 0;
        bool _justStarting = true;

        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            AllowsTransparency = true;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_isEqualized) Clear_btn_Click(sender, e);
            _isTyping = true;
            if (display.Text == "0") display.Text = "";

            Button num = (Button)sender;
            String numContent = num.Content.ToString() + "";
            String content = numContent.ToString(CultureInfo.CurrentCulture);
            if (content == ".")
            {
                if (display.Text != "")
                {
                    if (!display.Text.Contains('.')) display.Text = display.Text + content;
                }
                else
                {
                    display.Text = "0" + content;
                }
            }
            else
            {
                if (display.Text.Length < 20) display.Text = display.Text + content;
            }
        }

        private void mathButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isEqualized) _isEqualized = false;
                Button mathbtn = (Button)sender;
                String mathbtnContent = mathbtn.Content.ToString() + "";
                if (_justStarting)
                {
                    _results = decimal.Parse(display.Text);
                    display.Text = "0";
                    _operation = mathbtnContent.ToString(CultureInfo.CurrentCulture);
                    _isTyping = false;
                    _justStarting = false;
                }

                Trace.WriteLine(_results);
                if (!_justStarting)
                {
                    if (_isTyping)
                    {
                        switch (_operation)
                        {
                            case "+":
                                _results += decimal.Parse(display.Text);
                                break;
                            case "-":
                                _results -= decimal.Parse(display.Text);
                                break;
                            case "*":
                                _results *= decimal.Parse(display.Text);
                                break;
                            case "/":
                                if (decimal.Parse(display.Text) == 0)
                                {
                                    _zero = true;
                                    break;
                                }
                                else
                                {
                                    _results /= decimal.Parse(display.Text);
                                    break;
                                }
                        }

                        _operation = mathbtnContent.ToString(CultureInfo.CurrentCulture);
                        display.Text = "0";
                        _isTyping = false;
                    }
                    else
                    {
                        _operation = mathbtnContent.ToString(CultureInfo.CurrentCulture);
                        display.Text = "0";
                    }
                }

                if (!_zero)
                    label.Text = System.Convert.ToString(_results.ToString(CultureInfo.CurrentCulture)) + " " +
                                 _operation;
                if (_zero) nieDzielPrzezZero();
            }
            catch
            {
                liczbaPozaZakresem();
            }
        }

        private void equals_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_operation == "") return;
                if (!_isEqualized)
                {
                    _eqval = decimal.Parse(display.Text);
                    switch (_operation)
                    {
                        case "+":
                            display.Text =
                                (_results + decimal.Parse(display.Text)).ToString(CultureInfo.CurrentCulture);
                            break;
                        case "-":
                            display.Text =
                                (_results - decimal.Parse(display.Text)).ToString(CultureInfo.CurrentCulture);
                            break;
                        case "*":
                            display.Text =
                                (_results * decimal.Parse(display.Text)).ToString(CultureInfo.CurrentCulture);
                            break;
                        case "/":
                            if (decimal.Parse(display.Text) == 0)
                            {
                                _zero = true;
                                break;
                            }
                            else
                            {
                                display.Text =
                                    (_results / decimal.Parse(display.Text)).ToString(CultureInfo.CurrentCulture);
                                break;
                            }
                    }

                    _isEqualized = true;
                }
                else
                {
                    display.Text = _operation switch
                    {
                        "+" => (_results + _eqval).ToString(CultureInfo.CurrentCulture),
                        "-" => (_results - _eqval).ToString(CultureInfo.CurrentCulture),
                        "*" => (_results * _eqval).ToString(CultureInfo.CurrentCulture),
                        "/" => (_results / _eqval).ToString(CultureInfo.CurrentCulture),
                        _ => display.Text
                    };
                }

                _isTyping = false;
                label.Text = System.Convert.ToString(_results.ToString(CultureInfo.CurrentCulture)) + " " + _operation +
                             " " + _eqval + " =";
                _results = decimal.Parse(display.Text);
                if (_zero) nieDzielPrzezZero();
            }
            catch
            {
                liczbaPozaZakresem();
            }
        }

        private void PosNeg_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_isEqualized) Clear_btn_Click(sender, e);
            else
            {
                if (display.Text != "0")
                    display.Text = (decimal.Parse(display.Text) * (-1)).ToString(CultureInfo.CurrentCulture);
            }
        }

        private void ClearEntry_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_isEqualized) Clear_btn_Click(sender, e);
            else display.Text = "0";
        }

        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            display.Text = "0";
            label.Text = "";
            _results = 0;
            _operation = "";
            _isEqualized = false;
            _justStarting = true;
            _zero = false;
        }

        private void Deletion_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_isEqualized) Clear_btn_Click(sender, e);
            else
            {
                if (display.Text.Length > 0)
                {
                    display.Text = display.Text.Substring(0, display.Text.Length - 1);
                }

                if (display.Text == "")
                {
                    display.Text = "0";
                }
            }
        }

        private void nieDzielPrzezZero()
        {
            display.Text = "0";
            label.Text = "NIE DZIEL PRZEZ ZERO";
            _results = 0;
            _operation = "";
            _isEqualized = false;
            _zero = false;
        }

        private void liczbaPozaZakresem()
        {
            nieDzielPrzezZero();
            label.Text = "To big number";
        }

        private void Minimize_Button(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}