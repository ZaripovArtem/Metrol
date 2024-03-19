using Metrol.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Metrol.ViewModel
{
    /// <summary>
    /// Модель представления аутентификации.
    /// </summary>
    class AuthViewModel : ViewModelBase
    {
        /// <summary>
        /// Минимальная длина логина.
        /// </summary>
        private const int MinUsernameLength = 3;

        /// <summary>
        /// Минимальная длина пароля.
        /// </summary>
        private const int MinPasswordLength = 3;

        /// <summary>
        /// Логин.
        /// </summary>
        private string _username;

        /// <summary>
        /// Пароль.
        /// </summary>
        private string _password;

        /// <summary>
        /// Логин.
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Проверка валидности логина.
        /// </summary>
        /// <param name="param">Передаваемые параметры.</param>
        /// <returns>True - если подходит под критерии, false - если нет.</returns>
        private bool IsValidUserName(object param)
        {
            if (string.IsNullOrEmpty(Username)
                || Username.Length < MinUsernameLength) 
            { 
                return false;
            }

            return true;
        }

        /// <summary>
        /// Команда регистрации.
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        /// <summary>
        /// Команда авторизации.
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        /// <summary>
        /// Инициализация.
        /// </summary>
        public AuthViewModel()
        {
            _context.Database.EnsureCreated();

            LoginCommand = new RelayCommand(Login, IsValidUserName);
            RegisterCommand = new RelayCommand(Register, IsValidUserName);
        }

        /// <summary>
        /// Регистрация.
        /// </summary>
        /// <param name="param">Передаваемые параметры.</param>
        private void Register(object param)
        {
            var isValid = Validation();

            if (isValid == false)
            {
                return;
            }

            var passwordSaltTextBlock = Application.Current.FindResource("PasswordSalt") as TextBlock;
            string passwordSalt = passwordSaltTextBlock.Text;

            var checkUser = _context.Users.FirstOrDefault(u => u.Username == Username);

            if (checkUser == null)
            {
                try
                {
                    _context.Users.Add(new User
                    {
                        Username = Username,
                        PasswordHash = PasswordEncoder.GenerateSaltedHash(Password, passwordSalt)
                    });

                    _context.SaveChanges();

                    MessageBox.Show("Пользователь зарегистрирован.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка. {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"Пользователь с логином {Username} уже существует.");
            }
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="param">Передаваемые параметры.</param>
        private void Login(object param)
        {
            var isValid = Validation();

            if (isValid == false)
            {
                return;
            }

            var passwordSaltTextBlock = Application.Current.FindResource("PasswordSalt") as TextBlock;
            string passwordSalt = passwordSaltTextBlock.Text;

            var user = _context.Users.FirstOrDefault(u => u.Username == Username);

            if (user != null)
            {
                try
                {
                    if (user.PasswordHash == PasswordEncoder.GenerateSaltedHash(Password, passwordSalt)) 
                    {
                        MessageBox.Show("Пользователь успешно вошел в систему.");
                    }
                    else
                    {
                        MessageBox.Show("Неверно введен пароль.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка. {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"Пользователь с логином {Username} не найден.");
            }
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        /// <returns>True - если корректно, false - нет.</returns>
        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < MinUsernameLength)
            {
                MessageBox.Show("Некорректное имя пользователя.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < MinPasswordLength)
            {
                MessageBox.Show($"Минимальная длина пароля: {MinPasswordLength}.");
                return false;
            }

            return true;
        }
    }
}
