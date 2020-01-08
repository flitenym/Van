using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using Van.DataBase;
using Van.Helper;

namespace Van.ViewModel
{
    class ConnectionDataBaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region connString из app.config или просто введенный

        private string originalConnectionString = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString; 
        public string OriginalConnectionString
        {
            get { return originalConnectionString; }
            set
            {
                originalConnectionString = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OriginalConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Кастомный connString строится из вводимых данных

        public string CustomConnectionString
        {
            get { return $"{CustomServerAddress}{CustomDBName}{CustomLogin}{CustomPassword}Trusted_Connection = False; MultipleActiveResultSets = True; {CustomTimeOut}"; }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Используется из app.config или строится вручную

        private bool isCustom = true;
        public bool IsCustom
        {
            get { return isCustom; }
            set
            {
                isCustom = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCustom)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Сервер

        private string CustomServerAddress => ServerAddress == string.Empty ? string.Empty : $"Server = {ServerAddress}; ";

        private string serverAddress = string.Empty;
        public string ServerAddress
        {
            get { return serverAddress; }
            set
            {
                serverAddress = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ServerAddress)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Название БД

        private string CustomDBName => DBName == string.Empty ? string.Empty : $"Database = {DBName}; ";
        private string dBName = string.Empty;
        public string DBName
        {
            get { return dBName; }
            set
            {
                dBName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DBName)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Логин

        private string CustomLogin => Login == string.Empty ? string.Empty : $"User Id = {Login}; ";
        private string login = string.Empty;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Login)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Пароль

        private string CustomPassword => Password == string.Empty ? string.Empty : $"password = {Password}; ";
        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Password)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region ТаймАут

        private string CustomTimeOut => TimeOut == string.Empty ? string.Empty : $"Connect Timeout = {TimeOut}; ";
        private string timeOut = "10";
        public string TimeOut
        {
            get { return timeOut; }
            set
            {
                timeOut = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TimeOut)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomConnectionString)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Результирующий connection string, используется для проверки соединения

        public string ConnectionString
        {
            get { return IsCustom ? CustomConnectionString : OriginalConnectionString; }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }

        #endregion

        #region Команда для проверки подключения к БД

        private RelayCommand checkConnectionCommand;
        public RelayCommand CheckConnectionCommand
        {
            get
            {
                return checkConnectionCommand ??
                  (checkConnectionCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          CheckConnection()
                      );
                  }));
            }
        }

        public void CheckConnection()
        {
            DatabaseOperation.TryConnection(ConnectionString);
        }

        #endregion

    }
}
