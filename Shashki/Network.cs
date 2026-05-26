using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shashki
{
    /// <summary>
    /// Интерфейс для сетевого взаимодействия
    /// </summary>
    public interface ICommunicator
    {
        /// <summary>
        /// Асинхронный приём сообщений
        /// </summary>
        /// <returns></returns>
        Task ReceiveMessageAsync();

        /// <summary>
        /// Асинхронная отправка сообщения
        /// </summary>
        /// <param name="turn">Состояние игры по результатам хода</param>
        /// <returns></returns>
        Task SendMessageAsync(TurnDTO turn);

        /// <summary>
        /// Событие приёма сообщения
        /// </summary>
        event EventHandler<TurnDTO> OnMessageReceived;

        /// <summary>
        /// Событие установки соединения
        /// </summary>
        event EventHandler<Color> OnConnected;
    }

    /// <summary>
    /// Вебсокет сервер
    /// </summary>
    public class Server : ICommunicator
    {
        /// <summary>
        /// Прослушиватель http для апгрейда до WebSocket
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// Обслуживает WebSocket соединение
        /// </summary>
        private WebSocket webSocket;

        public event EventHandler<TurnDTO> OnMessageReceived;
        public event EventHandler<Color> OnConnected;

        /// <summary>
        /// Событие ошибки "Отказано в доступе", возникающей при необходимости прав Администратора
        /// </summary>
        public event EventHandler AdminRequired;

        /// <summary>
        /// Асинхронный запуск сервера
        /// </summary>
        /// <param name="color">Цвет игрока-хоста</param>
        /// <returns>Цвет оппонента</returns>
        public async Task StartAsync(Color color)
        {
            try
            {
                string IP_address = "127.0.0.1";
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IP_address = ip.ToString();
                    }
                }
                httpListener = new HttpListener();
                httpListener.Prefixes.Add($"http://{IP_address}:8080/");
                httpListener.Start();

                while (httpListener.IsListening)
                {
                    try
                    {
                        var context = await httpListener.GetContextAsync();
                        if (context.Request.IsWebSocketRequest)
                        {
                            var webSocketContext = await context.AcceptWebSocketAsync(null);
                            webSocket = webSocketContext.WebSocket;
                            OnConnected?.Invoke(this, color);
                            string json = JsonConvert.SerializeObject(color);
                            byte[] bytes = Encoding.UTF8.GetBytes(json);

                            if (webSocket?.State == WebSocketState.Open)
                            {
                                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                            }

                            await ReceiveMessageAsync();
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                            context.Response.Close();
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }

                }
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 5) // 5 = Access Denied
            {
                AdminRequired?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task ReceiveMessageAsync()
        {
            if (webSocket == null || webSocket.State != WebSocketState.Open) { return; }
            var buffer = new byte[1024*4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


            while (webSocket != null && webSocket.State == WebSocketState.Open && !result.CloseStatus.HasValue)
            {
                try
                {
                    TurnDTO Message = JsonConvert.DeserializeObject<TurnDTO>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    OnMessageReceived?.Invoke(this, Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обработки сообщения: {ex.Message}");
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                await Task.Delay(500);
                webSocket?.Dispose();
            }
        }

        public async Task SendMessageAsync(TurnDTO message)
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            if (webSocket?.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        /// <summary>
        /// Асинхронная остановка сервера
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                await Task.Delay(500);
                webSocket?.Dispose();
            }
            if (httpListener!=null && httpListener.IsListening)
            {
                httpListener.Stop();
                httpListener.Close();
            }
        }
    }

    /// <summary>
    /// Вебсокет клиент
    /// </summary>
    public class Client : ICommunicator
    {
        /// <summary>
        /// Обсуживает WebSocket на стороне клиента
        /// </summary>
        private ClientWebSocket webSocket;

        public event EventHandler<TurnDTO> OnMessageReceived;

        /// <summary>
        /// Событие ошибки соединения
        /// </summary>
        public event EventHandler<string> OnConnectionError;

        /// <summary>
        /// Событие подключения
        /// </summary>
        public event EventHandler<Color> OnConnected;

        /// <summary>
        /// Асинхронное соединение с сервером по IP-адресу
        /// </summary>
        /// <param name="ip">IP-адрес сервер</param>
        /// <returns></returns>
        public async Task ConnectAsync(string ip)
        {
            string url = "ws://" + ip + ":8080/";
            webSocket = new ClientWebSocket();
            try
            {
                await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);

                var buffer = new byte[1024*4];
                while (webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                        }
                        Color color = JsonConvert.DeserializeObject<Color>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        OnConnected?.Invoke(this, color);
                        //await Task.Run(async () => await ReceiveMessageAsync()); помогайка???
                        await Task.Run(() => ReceiveMessageAsync());
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обработки сообщения: {ex.Message}");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                OnConnectionError?.Invoke(this, $"Ошибка подключения: {ex.Message}");
            }
        }

        public async Task ReceiveMessageAsync()
        {
            var buffer = new byte[1024*4];
            

            while (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                        break;
                    }

                    TurnDTO Message = JsonConvert.DeserializeObject<TurnDTO>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    OnMessageReceived?.Invoke(this, Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обработки сообщения: {ex.Message}");
                }
            }

            await webSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }

        public async Task SendMessageAsync(TurnDTO message)
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            if (webSocket?.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
