namespace Shashki
{
    /// <summary>
    /// DTO для передачи состояния игры между игроками
    /// </summary>
    public class TurnDTO
    {
        /// <summary>
        /// Состояние игры (игрок походил, проиграл, сдаётся)
        /// </summary>
        public TurnStatus status;

        /// <summary>
        /// Игровое поле по результатам хода
        /// </summary>
        public CellState[,] board;

        public TurnDTO(TurnStatus status, CellState[,] board)
        {
            this.status = status;
            this.board = board;
        }
    }

    /// <summary>
    /// Состояние игры (игрок походил, проиграл, сдаётся)
    /// </summary>
    public enum TurnStatus
    {
        MOVE, LOSE, GIVEUP
    }
}
