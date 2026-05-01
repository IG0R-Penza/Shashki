using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shashki
{
    public partial class Form1 : Form
    {
        private PictureBox[,] cellReferences = new PictureBox[8, 8];

        public enum PieceType { None, Man, King }
        public enum PlayerColor { None, White, Black }

        public class CellState
        {
            public PieceType Type { get; set; } = PieceType.None;
            public PlayerColor Color { get; set; } = PlayerColor.None;
        }
        private CellState[,] board;// = new CellState[8, 8];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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
                        BackColor = Color.Transparent,
                        Tag = new Point(j, i)
                        //Image = image
                    };
                    cell.Click += Cell_Click;
                    cellReferences[i, j] = cell;
                    BoardTable.Controls.Add(cell);
                }
            }


            board = new CellState[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new CellState();
                    if ((i+j)%2 != 0)
                    {
                        if (i < 3)
                        {
                            board[i, j].Color = PlayerColor.Black;
                            board[i, j].Type = PieceType.Man;
                        }
                        else if (i > 4)
                        {
                            board[i, j].Color = PlayerColor.White;
                            board[i, j].Type = PieceType.Man;
                        }
                    }
                }
            }
            UpdateVisualBoard();

            
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var cellPosition = (Point)((PictureBox)sender).Tag;
            MessageBox.Show(cellPosition.ToString() + board[cellPosition.Y, cellPosition.X].Color.ToString() + board[cellPosition.Y, cellPosition.X].Type.ToString());
        }

        private void UpdateVisualBoard()
        {

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = board[row, col];
                    //var visualCell = (PictureBox)BoardTable.GetControlFromPosition(col, row);
                    var visualCell = cellReferences[row, col];
                    if ((row + col) % 2 == 0)
                    {
                        visualCell.Image = Properties.Resources.WhiteEmpty;
                    }
                    else
                    {
                        switch (cell.Color)
                        {
                            case PlayerColor.White:
                                visualCell.Image = cell.Type == PieceType.King ? Properties.Resources.BlackWhiteKing : Properties.Resources.BlackWhiteMan;
                                break;
                            case PlayerColor.Black:
                                visualCell.Image = cell.Type == PieceType.King ? Properties.Resources.BlackBlackKing : Properties.Resources.BlackBlackMan;
                                break;
                            default:
                                visualCell.Image = Properties.Resources.BlackEmpty;
                                break;
                        }
                    }
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int minSize = Math.Min(this.ClientSize.Width-BoardTable.Location.X-200, this.ClientSize.Height-BoardTable.Location.Y-60)/8*8;
            BoardTable.Size = new Size(minSize, minSize);
        }
    }
}
