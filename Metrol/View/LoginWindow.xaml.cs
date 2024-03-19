using Metrol.ViewModel;
using System.Windows;

namespace Metrol.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            DataContext = new AuthViewModel();
        }
    }
}
