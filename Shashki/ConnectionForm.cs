using System;
using System.Windows.Forms;

namespace Shashki
{
    /// <summary>
    /// Форма установление сетевого соединения
    /// </summary>
    public partial class ConnectionForm : Form
    {
        /// <summary>
        /// Ссылка на форму игрового поля
        /// </summary>
        private MainForm mainForm;

        /// <summary>
        /// Ссылка на объект, осуществляющий обмен данными по сети
        /// </summary>
        private ICommunicator communicator;

        /// <summary>
        /// Флаг установки соединения
        /// </summary>
        bool connected = false;

        /// <summary>
        /// Конструктор со ссылкой на форму игрового поля
        /// </summary>
        /// <param name="mainForm">Форма с игровым полем</param>
        public ConnectionForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку присоединения к созданной игре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void JoinGameBtn_Click(object sender, EventArgs e)
        {
            setUiEnable(false);
            string ip = normalizeIP(IPmaskedTextBox.Text);
            Client client = new Client();
            InfoLabel.Text = "Подключаемся";
            client.OnConnected += OnConnectedClient;
            client.OnConnectionError += OnConnectionError;
            await client.ConnectAsync(ip);
            setUiEnable(true);
        }

        /// <summary>
        /// Обработчик события установки соединения для клиента, создаёт игру соответствующего цвета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="color">Цвет игрока</param>
        private void OnConnectedClient(object sender, Color color)
        {
            var client = (Client)sender;
            mainForm.Communicator = client;
            mainForm.Game = new Game(color);
            mainForm.gameStarted = true;
            client.OnConnected -= OnConnectedClient;
            Close();
        }

        /// <summary>
        /// Обработчик события ошибки при попытке соединения для клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnConnectionError(object sender, string message)
        {
            InfoLabel.Text = "Неправильный адрес";
            MessageBox.Show(message);
        }

        /// <summary>
        /// Обработчик нажатия кнопки создания игровой партии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartOwnBtn_Click(object sender, EventArgs e)
        {
            setUiEnable(false);
            Random rnd = new Random();
            Server server = new Server();
            communicator = server;
            server.OnConnected += OnConnectedServer;
            server.AdminRequired += ShowAdminRequiredMessage;
            InfoLabel.Text = "Ждём оппонента";
            await server.StartAsync(rnd.Next(0, 2) < 1 ? Color.White : Color.Black);
            setUiEnable(true);
        }

        /// <summary>
        /// Обработчик события соединения для сервера, создаёт игру соответствующего цвета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="color">Цвет оппонента</param>
        private void OnConnectedServer(object sender, Color color)
        {
            var server = (Server)sender;
            mainForm.Communicator = server;
            mainForm.Game = new Game(color==Color.White ? Color.Black : Color.White);
            mainForm.gameStarted = true;
            server.OnConnected -= OnConnectedServer;
            connected = true;
            Close();
        }

        /// <summary>
        /// Обработчик ввода IP-адреса с валидацией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPmaskedTextBox_TextChanged(object sender, EventArgs e)
        {
            InfoLabel.Text = "";
            string ipText = IPmaskedTextBox.Text;
            JoinGameBtn.Enabled = isValidIP(ipText) ? true : false;
        }

        /// <summary>
        /// Валидация IP-адреса
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool isValidIP(string ip)
        {
            ip = normalizeIP(ip);

            string[] parts = ip.Split('.');

            if (parts.Length != 4) return false;

            foreach (string part in parts)
            {
                int int_part;
                if (string.IsNullOrEmpty(part) || !int.TryParse(part, out int_part)) return false;
                if (int_part > 255) return false;
            }

            return true;
        }

        /// <summary>
        /// Удаление пробелов из строкового представления IP-адреса
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private string normalizeIP(string ip)
        {
            while (ip.Contains(" ")) ip = ip.Replace(" ", "");
            return ip;
        }

        /// <summary>
        /// Установка возможности взаимодействия с интерфейсом для пользователя
        /// </summary>
        /// <param name="enable">Возможность взаимодействия с интерфейсом</param>
        private void setUiEnable(bool enable)
        {
            StartOwnBtn.Enabled = enable;
            JoinGameBtn.Enabled = enable;
            IPmaskedTextBox.Enabled = enable;
        }

        /// <summary>
        /// Закрытие сервера вместе с закрытием формы при отсутствии соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConnectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!connected && communicator is Server) ((Server)communicator).StopAsync();
        }

        /// <summary>
        /// Обработчик ошибки "Отказано в доступе" для сервера - показывает окно,
        /// сообщающее о необходимости прав администратора для создания сервера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAdminRequiredMessage(object sender, EventArgs e)
        {
            InfoLabel.Text = "";
            string message =
                "Не удалось запустить сервер игры.\n\n" +
                "Для создания игры (хостинга) требуется запустить приложение " +
                "с правами администратора.\n\n" +
                "Для подключения к чужой игре запуск от администратора НЕ требуется.";

            MessageBox.Show(
                message,
                "Требуется администратор для хостинга",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
