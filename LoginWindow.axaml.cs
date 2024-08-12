using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace POSApp
{
    public partial class LoginWindow : Window
    {
        private TextBox _focusedTextBox;

        public LoginWindow()
        {
            InitializeComponent();
            CreateKeyboard();
            UsernameTextBox.GotFocus += TextBox_GotFocus;
            PasswordTextBox.GotFocus += TextBox_GotFocus;
        }

        private void CreateKeyboard()
        {
            var keys = new List<string>
            {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
             "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
             "A", "S", "D", "F", "G", "H", "J", "K", "L",
             "Z", "X", "C", "V", "B", "N", "M", "Backspace"
            };

            var keyboard = this.FindControl<Grid>("Keyboard");
            int row = 0, col = 0;

            foreach (var key in keys)
            {
                var button = new Button { Content = key, Classes = { "keyboard" } };
                button.Click += OnKeyPress;


                if (key == "Backspace")
                {
                    Grid.SetColumnSpan(button, 2);
                }

                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);

                keyboard.Children.Add(button);

                col++;
                if (col > 9)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private void OnKeyPress(object sender, RoutedEventArgs e)
        {
            if (_focusedTextBox == null) return;

            var key = ((Button)sender).Content.ToString();

            if (key == "Backspace")
            {
                if (_focusedTextBox.Text.Length > 0)
                {
                    _focusedTextBox.Text = _focusedTextBox.Text.Substring(0, _focusedTextBox.Text.Length - 1);
                }
            }
            else
            {
                _focusedTextBox.Text += key;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _focusedTextBox = (TextBox)sender;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Text;

            if (AuthenticateUser(username, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid credentials");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // TODO: Implement actual authentication logic
            return username == "admin" && password == "password";
        }
    }
}
