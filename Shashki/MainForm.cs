using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shashki
{
    /// <summary>
    /// Главная форма, содержит игровое поле
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Ссылки на PictureBox, помещённые в TableLayoutPanel.
        /// Обращение к элементу через сам TableLayoutPanel вызывает nullReferenceException в mono
        /// </summary>
        private PictureBox[,] cellReferences = new PictureBox[8, 8];

        /// <summary>
        /// Объект для сетевого взаимодействия
        /// </summary>
        public ICommunicator Communicator { get; set; }
        
        /// <summary>
        /// Игра
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// Координаты выбранной клетки
        /// </summary>
        private Point SelectedCell = new Point(-1, -1);

        /// <summary>
        /// Флаг начала игры
        /// </summary>
        public bool gameStarted { get; set; }

        private InfoPrinter infoPrinter;

        public MainForm()
        {
            InitializeComponent();
            gameStarted = false;
            infoPrinter = new InfoPrinter(PrintInfo);
        }

        /// <summary>
        /// Вывод информации о ходе игры
        /// </summary>
        /// <param name="info"></param>
        private void PrintInfo(string info)
        {
            InfoLabel.Text = info;
        }

        /// <summary>
        /// Обработчик загрузки формы, заполняет доску TableLayoutPanel клетками PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (BoardTable == null) return;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = System.Drawing.Color.Transparent,
                        Tag = new Point(j, i)
                    };
                    cell.Click += Cell_Click;
                    cellReferences[i, j] = cell;
                    BoardTable.Controls.Add(cell);
                }
            }

            Game = new Game(Color.White);
            UpdateVisualBoard();

            BoardTable.Enabled = false;

            HelloForm helloform = new HelloForm();
            helloform.ShowDialog();
        }

        /// <summary>
        /// Обработчик клика по клетке игрового поля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cell_Click(object sender, EventArgs e)
        {
            var cellPosition = (Point)((PictureBox)sender).Tag;
            switch (Game.Status) {
                case GameStatus.AbleToMove:
                    if (SelectedCell.X == -1 && Game.isAbleToMove(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, "Ходите");
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && Game.Move(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, Game.Player == Color.White ? "Ход чёрных" : "Ход белых");
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        SelectedCell.X = -1;
                        SelectedCell.Y = -1;
                        UpdateVisualBoard();
                        setUiEnable(false);
                        await Task.Run(() => Communicator.SendMessageAsync(new TurnDTO(TurnStatus.MOVE, Game.Board)));
                    }
                    else InfoLabel.Invoke(infoPrinter, "Так походить не получится!");
                        break;
                case GameStatus.HaveToTake:
                    if (SelectedCell.X == -1 && Game.isAbleToTake(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, "Бейте");
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && Game.Take(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, "");
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        UpdateVisualBoard();
                        if (Game.Status == GameStatus.ContinueToTake)
                        {
                            SelectedCell.X = cellPosition.X;
                            SelectedCell.Y = cellPosition.Y;
                            visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                            visualCell.BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            InfoLabel.Invoke(infoPrinter, Game.Player == Color.White ? "Ход чёрных" : "Ход белых");
                            SelectedCell.X = -1;
                            SelectedCell.Y = -1;
                            await Task.Run(()=>Communicator.SendMessageAsync(new TurnDTO(TurnStatus.MOVE, Game.Board)));
                        }
                    }
                    else InfoLabel.Invoke(infoPrinter, "Так побить не получится!");
                    break;
                case GameStatus.ContinueToTake:
                    if (SelectedCell.X == -1 && Game.isAbleToTake(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, "Бейте");
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && Game.Take(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Invoke(infoPrinter, "Бейте");
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        UpdateVisualBoard();
                        if (Game.Status == GameStatus.ContinueToTake)
                        {
                            SelectedCell.X = cellPosition.X;
                            SelectedCell.Y = cellPosition.Y;
                            visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                            visualCell.BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            InfoLabel.Invoke(infoPrinter, Game.Player == Color.White ? "Ход чёрных" : "Ход белых");
                            SelectedCell.X = -1;
                            SelectedCell.Y = -1;
                            await Task.Run(() => Communicator.SendMessageAsync(new TurnDTO(TurnStatus.MOVE, Game.Board)));
                        }
                    }
                    else InfoLabel.Invoke(infoPrinter, "Так побить не получится!");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Переотрисовка игрового поля
        /// </summary>
        private void UpdateVisualBoard()
        {

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = Game.Board[col, row];
                    var visualCell = cellReferences[col, row];
                    if ((row + col) % 2 == 0)
                    {
                        visualCell.Image = Properties.Resources.WhiteEmpty;
                    }
                    else
                    {
                        switch (cell.Color)
                        {
                            case Color.White:
                                visualCell.Image = cell.Type == Type.King ? Properties.Resources.BlackWhiteKing : Properties.Resources.BlackWhiteMan;
                                break;
                            case Color.Black:
                                visualCell.Image = cell.Type == Type.King ? Properties.Resources.BlackBlackKing : Properties.Resources.BlackBlackMan;
                                break;
                            default:
                                visualCell.Image = Properties.Resources.BlackEmpty;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события получения сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private async void onMessageReceived(object sender, TurnDTO message)
        {
            Game.Board = message.board;
            Game.InvertBoard();
            UpdateVisualBoard();
            switch (message.status)
            {
                case TurnStatus.MOVE:
                    Game.updateStatus();
                    if (Game.Status == GameStatus.Lose)
                    {
                        
                        BoardTable.Enabled = false;
                        gameStarted = false;
                        GameControlBtn.Enabled = true;
                        GameControlBtn.Text = "Начать игру";
                        InfoLabel.Invoke(infoPrinter, "Вы проиграли!");
                        await Task.Run(()=>Communicator.SendMessageAsync(new TurnDTO(TurnStatus.LOSE, Game.Board)));
                        try
                        {
                            if (Communicator is Server) await ((Server)Communicator).StopAsync();
                        }
                        catch { }
                    }
                    else
                    {
                        InfoLabel.Invoke(infoPrinter, "Ваш ход");
                        setUiEnable(true);
                    }
                    break;
                case TurnStatus.LOSE:
                    
                    BoardTable.Enabled = false;
                    gameStarted = false;
                    GameControlBtn.Enabled = true;
                    GameControlBtn.Text = "Начать игру";
                    InfoLabel.Invoke(infoPrinter, "Вы победили!");
                    try
                    {
                        if (Communicator is Server) await ((Server)Communicator).StopAsync();
                    }
                    catch { }
                    break;
                case TurnStatus.GIVEUP:
                    
                    BoardTable.Enabled = false;
                    gameStarted = false;
                    GameControlBtn.Enabled = true;
                    GameControlBtn.Text = "Начать игру";
                    InfoLabel.Invoke(infoPrinter, "Соперник сдался!");
                    try
                    {
                        if (Communicator is Server) await ((Server)Communicator).StopAsync();
                    }
                    catch { }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Обработчик события изменения размеров формы, масштабирующий игровое поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            int minSize = Math.Min(this.Size.Width - 104, this.Size.Height - 120) / 8 * 8;
            BoardTable.Size = new Size(minSize, minSize);
            BoardTable.Location = new Point((this.ClientSize.Width - minSize) / 2, 20);
        }

        /// <summary>
        /// Обработчик нажатия кнопки управления игрой (начать игру, сдаться)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GameControlBtn_Click(object sender, EventArgs e)
        {
            if (!gameStarted)
            {
                ConnectionForm connectionForm = new ConnectionForm(this);
                connectionForm.ShowDialog();
                if (gameStarted)
                {
                    Communicator.OnMessageReceived += onMessageReceived;
                    UpdateVisualBoard();
                    GameControlBtn.Text = "Сдаться";
                    setUiEnable(Game.Player == Color.White);
                    InfoLabel.Invoke(infoPrinter, Game.Player == Color.White ? "Ваш ход" : "Ход белых");
                }
            }
            else
            {
                var dialogRes = MessageBox.Show(
                    "Вы уверены, что хотите сдаться?",
                    "Сдаться",
                     MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                if (dialogRes == DialogResult.Yes)
                {
                    BoardTable.Enabled = false;
                    gameStarted = false;
                    GameControlBtn.Enabled = true;
                    GameControlBtn.Text = "Начать игру";
                    InfoLabel.Invoke(infoPrinter, "Вы сдались!");
                    await Task.Run(() => Communicator.SendMessageAsync(new TurnDTO(TurnStatus.GIVEUP, Game.Board)));
                    try
                    {
                        if (Communicator is Server) await ((Server)Communicator).StopAsync();
                    }
                    catch { }
                }
            }
            
        }

        /// <summary>
        /// Установка возможности взаимодействия с интерфейсом
        /// </summary>
        /// <param name="enable">Возможность взаимодействия с интерфейсом</param>
        private void setUiEnable(bool enable)
        {
            BoardTable.Enabled=enable;
            GameControlBtn.Enabled=enable;
        }

        /// <summary>
        /// Завершение партии при закрытии формы
        /// </summary>
        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gameStarted)
            {
                e.Cancel = true;
                await Task.Run(() => Communicator.SendMessageAsync(new TurnDTO(TurnStatus.GIVEUP, Game.Board)));
                await Task.Delay(500);
                try
                {
                    if (Communicator is Server) await((Server)Communicator).StopAsync();
                }
                catch { }
                gameStarted = false;
                Close();
            }
        }
    }

    delegate void InfoPrinter(string info);
}
