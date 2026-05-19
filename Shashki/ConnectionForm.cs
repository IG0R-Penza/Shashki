using System;
using System.Windows.Forms;

namespace Shashki
{
    public partial class ConnectionForm : Form
    {
        private MainForm mainForm;

        private ICommunicator communicator;

        bool connected = false;
        public ConnectionForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

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

        private void OnConnectedClient(object sender, Color color)
        {
            var client = (Client)sender;
            mainForm.Communicator = client;
            mainForm.Game = new Game(color);
            mainForm.gameStarted = true;
            client.OnConnected -= OnConnectedClient;
            Close();
        }

        private void OnConnectionError(object sender, string message)
        {
            InfoLabel.Text = "Неправильный адрес";
            MessageBox.Show(message);//УБРАТЬ????
        }

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

        private void IPmaskedTextBox_TextChanged(object sender, EventArgs e)
        {
            InfoLabel.Text = "";
            string ipText = IPmaskedTextBox.Text;
            JoinGameBtn.Enabled = isValidIP(ipText) ? true : false;
        }

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

        private string normalizeIP(string ip)
        {
            while (ip.Contains(" ")) ip = ip.Replace(" ", "");
            return ip;
        }

        private void setUiEnable(bool enable)
        {
            StartOwnBtn.Enabled = enable;
            JoinGameBtn.Enabled = enable;
            IPmaskedTextBox.Enabled = enable;
        }

        private async void ConnectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!connected && communicator is Server) ((Server)communicator).StopAsync();
        }

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
