using BCrypt.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SecureApplicationDevelopment
{
    public partial class MainWindow : Window
    {
        // A dictionary to store users and their hashed passwords.
        // In a real application, this would be stored in a secure database.
        private Dictionary<string, string> users = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            // Register text changed event handlers
            UsernameTextBox.TextChanged += TextChanged;
            PasswordBox.PasswordChanged += TextChanged;

            // Initialize the state of the buttons
            UpdateButtonState();
        }

        // Event handler for when the text in the input fields changes
        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonState();
        }

        // Update the state of the buttons based on the input fields
        private void UpdateButtonState()
        {
            bool inputsNotEmpty = !string.IsNullOrEmpty(UsernameTextBox.Text) && !string.IsNullOrEmpty(PasswordBox.Password);

            Registenbtn.IsEnabled = inputsNotEmpty;
            Loginbtn.IsEnabled = inputsNotEmpty && users.ContainsKey(UsernameTextBox.Text);
        }

        // Event handler for when the text in the input fields changes
        private void TextChanged(object sender, RoutedEventArgs e)
        {
            UpdateButtonState();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Input validation
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                InfoTextBlock.Text = "Username must be at least 3 characters long.";
                return;
            }

            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                InfoTextBlock.Text = "Password must be at least 8 characters long.";
                return;
            }

            // Check if the user already exists
            if (users.ContainsKey(username))
            {
                InfoTextBlock.Text = "Username is taken. Please choose another one.";
                return;
            }

            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Store the hashed password
            users[username] = hashedPassword;

            InfoTextBlock.Text = "User registered successfully.";

            // Clear inputs
            UsernameTextBox.Clear();
            PasswordBox.Clear();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Input validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                InfoTextBlock.Text = "Please enter a username and password.";
                return;
            }

            // Check if the user exists
            if (!users.ContainsKey(username))
            {
                InfoTextBlock.Text = "No account found with that username.";
                return;
            }

            // Get the stored hashed password for the user
            string storedHashedPassword = users[username];

            // Verify the password
            bool verified = BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);

            if (verified)
            {
                InfoTextBlock.Text = "Login successful.";

                // Clear inputs
                UsernameTextBox.Clear();
                PasswordBox.Clear();
            }
            else
            {
                InfoTextBlock.Text = "Invalid password.";
            }
        }
    }
}
