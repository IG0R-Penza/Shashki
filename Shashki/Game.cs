using System;
using System.Drawing;

namespace Shashki
{
    /// <summary>
    /// Класс игры
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public CellState[,] Board { get; set; }

        /// <summary>
        /// Цвет игрока
        /// </summary>
        public Color Player {  get; }
        
        /// <summary>
        /// Статус хода (может походить, должен побить, должен продолжить бить, проиграл и не может ни походить, ни побить)
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// Координаты шашки, которой следует продолжить серию взятий при возможности
        /// </summary>
        private Point TakingChecker { get; set; }

        /// <summary>
        /// Создаёт игру с начальным состоянием поля для игрока заданного цвета
        /// </summary>
        /// <param name="player">Цвет игрока</param>
        public Game(Color player) {
            Player = player;
            Status = GameStatus.AbleToMove;
            TakingChecker = new Point(-1, -1);
            Board = new CellState[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Board[row, col] = new CellState();
                    if ((row + col) % 2 != 0)
                    {
                        if (row < 3)
                        {
                            Board[row, col].Color = Color.Black;
                            Board[row, col].Type = Type.Man;
                        }
                        else if (row > 4)
                        {
                            Board[row, col].Color = Color.White;
                            Board[row, col].Type = Type.Man;
                        }
                    }
                }
            }
            if (Player == Color.Black)
            {
                InvertBoard();
                Status = GameStatus.Turn;
            }
        }

        /// <summary>
        /// Переворачивает доску
        /// </summary>
        public void InvertBoard()
        {
            CellState[,] board_copy = Board;
            Board = new CellState[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[7-i, 7-j] = board_copy[i,j];
                }
            }
        }

        /// <summary>
        /// Проверяет возможность перемещения выбранной шашки в выбранную клетку
        /// </summary>
        /// <param name="current_col">Текущий столбец доски</param>
        /// <param name="current_row">Текущий ряд доски</param>
        /// <param name="dest_col">Столбец клетки назначения</param>
        /// <param name="dest_row">Ряд клетки назначения</param>
        /// <returns>Возможность хода</returns>
        public bool isAbleToMoveTo(int current_col, int current_row, int dest_col, int dest_row)
        {
            if (current_col == dest_col && current_row == dest_row) return false;
            if (dest_col < 0 || dest_row < 0 || dest_col > 7 || dest_row > 7) return false;
            if (Board[current_row, current_col].Color != Player) return false;
            if (Board[current_row, current_col].Type == Type.Man && Board[dest_row, dest_col].Type == Type.None)
            {
                return (dest_col == current_col + 1 || dest_col == current_col - 1) && dest_row == current_row - 1;
            }
            if (Board[current_row, current_col].Type == Type.King)
            {
                if (Math.Abs(dest_col-current_col)==Math.Abs(dest_row-current_row))
                {
                    int col_step = Math.Abs(dest_col - current_col) / (dest_col - current_col);
                    int row_step = Math.Abs(dest_row - current_row) / (dest_row - current_row);
                    for (int i = 1; i <= Math.Abs(dest_col - current_col); i++)
                    {
                        if (Board[current_row+i*row_step, current_col+i*col_step].Type != Type.None) return false;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет возможность хода для выбранной шашки вообще
        /// </summary>
        /// <param name="col">Столбец выбранной шашки</param>
        /// <param name="row">Ряд выбранной шашки</param>
        /// <returns>Возможность хода</returns>
        public bool isAbleToMove(int col, int row) {
            return isAbleToMoveTo(col, row, col+1, row+1) || isAbleToMoveTo(col, row, col + 1, row - 1) || isAbleToMoveTo(col, row, col - 1, row + 1) || isAbleToMoveTo(col, row, col - 1, row - 1);
        }

        /// <summary>
        /// Проверяет возможность взятия шашки соперника с последующим перемещением выбранной шашки в выбранную клетку
        /// </summary>
        /// <param name="current_col">Текущий столбец доски</param>
        /// <param name="current_row">Текущий ряд доски</param>
        /// <param name="dest_col">Столбец клетки назначения</param>
        /// <param name="dest_row">Ряд клетки назначения</param>
        /// <returns>Возможность взятия</returns>
        public bool isAbleToTake(int current_col, int current_row, int dest_col, int dest_row)
        {
            if (current_col == dest_col && current_row == dest_row) return false;
            if (dest_col < 0 || dest_row < 0 || dest_col > 7 || dest_row > 7) return false;
            if (Board[current_row, current_col].Color != Player) return false;
            if (Board[current_row, current_col].Type == Type.Man)
            {
                if ((dest_col != current_col + 2 && dest_col != current_col - 2) || (dest_row != current_row - 2 && dest_row != current_row + 2)) return false;
            }
            if (Math.Abs(dest_col - current_col) == Math.Abs(dest_row - current_row) && Board[dest_row, dest_col].Type == Type.None)
            {
                int col_step = Math.Abs(dest_col - current_col) / (dest_col - current_col);
                int row_step = Math.Abs(dest_row - current_row) / (dest_row - current_row);

                Color enemyColor = Player == Color.White ? Color.Black : Color.White;
                short enemyCount = 0;
                for (int i = 1; i <= Math.Abs(dest_col - current_col); i++)
                {
                    CellState cell = Board[current_row + i * row_step, current_col + i * col_step];
                    if (cell.Color == enemyColor) enemyCount++;
                    if (cell.Color == Player || enemyCount > 1) return false;
                }

                return enemyCount == 1;
            }
            return false;
        }

        /// <summary>
        /// Проверяет возможность взятия шашки соперника для выбранной шашки вообще
        /// </summary>
        /// <param name="col">Столбец выбранной шашки</param>
        /// <param name="row">Ряд выбранной шашки</param>
        /// <returns>Возможность взятия</returns>
        public bool isAbleToTake(int current_col, int current_row)
        {
            if (Board[current_row, current_col].Color != Player) return false;
            if (Board[current_row, current_col].Type == Type.Man)
            {
                return isAbleToTake(current_col, current_row, current_col + 2, current_row + 2) || isAbleToTake(current_col, current_row, current_col + 2, current_row - 2) ||
                    isAbleToTake(current_col, current_row, current_col - 2, current_row + 2) || isAbleToTake(current_col, current_row, current_col - 2, current_row - 2);
            }
            if (Board[current_row, current_col].Type == Type.King)
            {
                for (int i = 2; i < 7; i++)
                {
                    if (isAbleToTake(current_col, current_row, current_col + i, current_row + i) || isAbleToTake(current_col, current_row, current_col + i, current_row - i) ||
                    isAbleToTake(current_col, current_row, current_col - i, current_row + i) || isAbleToTake(current_col, current_row, current_col - i, current_row - i)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Обновляет статус хода
        /// </summary>
        public void updateStatus()
        {
            bool hasMovable = false;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    
                    if ((row + col) % 2 != 0)
                    {
                        if (isAbleToTake(col, row))
                        {
                            Status = GameStatus.HaveToTake;
                            return;
                        }
                        if (isAbleToMove(col, row))
                        {
                            Status = GameStatus.AbleToMove;
                            hasMovable = true;
                        }
                    }
                }
            }
            if (hasMovable) Status = GameStatus.AbleToMove;
            else Status = GameStatus.Lose;
        }

        /// <summary>
        /// Перемещает выбранную шашку в выбранную клетку
        /// </summary>
        /// <param name="current_col">Текущий столбец доски</param>
        /// <param name="current_row">Текущий ряд доски</param>
        /// <param name="dest_col">Столбец клетки назначения</param>
        /// <param name="dest_row">Ряд клетки назначения</param>
        /// <returns>Удалось ли походить</returns>
        public bool Move(int current_col, int current_row, int dest_col, int dest_row)
        {
            if (Status == GameStatus.AbleToMove && isAbleToMoveTo(current_col, current_row, dest_col, dest_row))
            {
                CellState current = Board[current_row, current_col];
                Board[dest_row, dest_col].Color = current.Color;
                Board[dest_row, dest_col].Type = current.Type;
                current.Color = Color.None;
                current.Type = Type.None;
                Status = GameStatus.Turn;
                if (dest_row == 0) Board[dest_row, dest_col].Type = Type.King;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Перемещает выбранную шашку в выбранную клетку, осуществляя взятие шашки соперника
        /// </summary>
        /// <param name="current_col">Текущий столбец доски</param>
        /// <param name="current_row">Текущий ряд доски</param>
        /// <param name="dest_col">Столбец клетки назначения</param>
        /// <param name="dest_row">Ряд клетки назначения</param>
        /// <returns>Удалось ли взять шашку соперника</returns>
        public bool Take(int current_col, int current_row, int dest_col, int dest_row)
        {
            if ((Status == GameStatus.HaveToTake || Status == GameStatus.ContinueToTake) && isAbleToTake(current_col, current_row, dest_col, dest_row))
            {
                if (Status == GameStatus.ContinueToTake && (current_col != TakingChecker.X ||  current_row != TakingChecker.Y)) { return false; }

                CellState current = Board[current_row, current_col];
                Board[dest_row, dest_col].Color = current.Color;
                Board[dest_row, dest_col].Type = current.Type;
                current.Color = Color.None;
                current.Type = Type.None;

                int col_step = Math.Abs(dest_col - current_col) / (dest_col - current_col);
                int row_step = Math.Abs(dest_row - current_row) / (dest_row - current_row);
                Color enemyColor = Player == Color.White ? Color.Black : Color.White;
                for (int i = 1; i <= Math.Abs(dest_col - current_col); i++)
                {
                    CellState cell = Board[current_row + i * row_step, current_col + i * col_step];
                    if (cell.Color == enemyColor)
                    {
                        cell.Color = Color.None;
                        cell.Type = Type.None;
                    }
                }

                if (dest_row == 0) Board[dest_row, dest_col].Type = Type.King;

                if (isAbleToTake(dest_col, dest_row))
                {
                    Status = GameStatus.ContinueToTake;
                    TakingChecker = new Point (dest_col, dest_row);
                }
                else
                {
                    Status = GameStatus.Turn;
                    TakingChecker = new Point(-1, -1);
                }
                return true;
            }
            else return false;
        }
    }

    /// <summary>
    /// Тип фигуры в клетке (пустая клетка, пешка, дамка)
    /// </summary>
    public enum Type { None, Man, King }

    /// <summary>
    /// Цвет фигуры в клетке (пустая клетка, белая шашка, чёрная шашка)
    /// </summary>
    public enum Color { None, White, Black }

    /// <summary>
    /// Статус хода (может ходить, должен взять шашку соперника, должен продолжить взятие шашек, проиграл и не может ни ходить, ни бить)
    /// </summary>
    public enum GameStatus { AbleToMove, HaveToTake, ContinueToTake, Turn, Lose }

    /// <summary>
    /// Класс клетки игрового поля
    /// </summary>
    public class CellState
    {
        /// <summary>
        /// Тип фигуры в клетке
        /// </summary>
        public Type Type { get; set; } = Type.None;

        /// <summary>
        /// Цвет фигуры в клетке
        /// </summary>
        public Color Color { get; set; } = Color.None;
    }

}
