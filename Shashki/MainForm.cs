using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shashki
{
    public partial class MainForm : Form
    {
        private PictureBox[,] cellReferences = new PictureBox[8, 8];

        private Game game;

        private Point SelectedCell = new Point(-1, -1);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (BoardTable == null) return;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Image image = (i+j)%2==0 ? Properties.Resources.WhiteEmpty : Properties.Resources.BlackEmpty;
                    var cell = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = System.Drawing.Color.Transparent,
                        Tag = new Point(j, i)
                        //Image = image
                    };
                    cell.Click += Cell_Click;
                    cellReferences[i, j] = cell;
                    BoardTable.Controls.Add(cell);
                }
            }

            game = new Game(Color.White);
            //game.Board[5, 2].Type = Type.King;//спавн тестовой дамки
            //game.Board[3, 4].Type = PieceType.King;
            //game.Board[3, 4].Color = PlayerColor.Black;
            //game.Board[2, 5].Type = Type.None;
            //game.Board[2, 5].Color = Color.None;
            //game.Board[3, 4].Type = Type.King;
            //game.Board[3, 4].Color = Color.Black;
            UpdateVisualBoard();

            
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var cellPosition = (Point)((PictureBox)sender).Tag;
            //MessageBox.Show(cellPosition.ToString() + game.Board[cellPosition.Y, cellPosition.X].Color.ToString() + game.Board[cellPosition.Y, cellPosition.X].Type.ToString());
            //MessageBox.Show(game.isAbleToTake(cellPosition.X, cellPosition.Y, cellPosition.X+3, cellPosition.Y-3).ToString());
            switch (game.Status) {
                case GameStatus.AbleToMove:
                    if (SelectedCell.X == -1 && game.isAbleToMove(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = "Ходите";
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && game.Move(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = game.Player == Color.White ? "Ход чёрных" : "Ход белых";
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        SelectedCell.X = -1;
                        SelectedCell.Y = -1;
                        UpdateVisualBoard();
                    }
                    else InfoLabel.Text = "Так походить не получится!";
                        break;
                case GameStatus.HaveToTake:
                    if (SelectedCell.X == -1 && game.isAbleToTake(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = "Бейте";
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && game.Take(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = "";
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        if (game.Status == GameStatus.ContinueToTake)
                        {
                            SelectedCell.X = cellPosition.X;
                            SelectedCell.Y = cellPosition.Y;
                            visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                            visualCell.BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            InfoLabel.Text = game.Player == Color.White ? "Ход чёрных" : "Ход белых";
                            SelectedCell.X = -1;
                            SelectedCell.Y = -1;
                        }
                        UpdateVisualBoard();
                    }
                    else InfoLabel.Text = "Так побить не получится!";
                    break;
                case GameStatus.ContinueToTake:
                    if (SelectedCell.X == -1 && game.isAbleToTake(cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = "Бейте";
                        SelectedCell = new Point(cellPosition.X, cellPosition.Y);
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else if (SelectedCell.X != -1 && game.Take(SelectedCell.X, SelectedCell.Y, cellPosition.X, cellPosition.Y))
                    {
                        InfoLabel.Text = "";
                        var visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                        visualCell.BorderStyle = BorderStyle.None;
                        if (game.Status == GameStatus.ContinueToTake)
                        {
                            SelectedCell.X = cellPosition.X;
                            SelectedCell.Y = cellPosition.Y;
                            visualCell = cellReferences[SelectedCell.Y, SelectedCell.X];
                            visualCell.BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            InfoLabel.Text = game.Player == Color.White ? "Ход чёрных" : "Ход белых";
                            SelectedCell.X = -1;
                            SelectedCell.Y = -1;
                        }
                        UpdateVisualBoard();
                    }
                    else InfoLabel.Text = "Так побить не получится!";
                    break;
                default:
                    break;
            }
        }

        private void UpdateVisualBoard()
        {

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = game.Board[col, row];
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            int minSize = Math.Min(this.Size.Width - 104, this.Size.Height - 120) / 8 * 8;
            BoardTable.Size = new Size(minSize, minSize);
            BoardTable.Location = new Point((this.ClientSize.Width - minSize) / 2, 20);
        }
    }
}
