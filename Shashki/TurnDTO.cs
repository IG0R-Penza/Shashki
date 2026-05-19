namespace Shashki
{
    public class TurnDTO
    {
        public TurnStatus status;
        public CellState[,] board;

        public TurnDTO(TurnStatus status, CellState[,] board)
        {
            this.status = status;
            this.board = board;
        }
    }

    public enum TurnStatus
    {
        MOVE, LOSE, GIVEUP
    }
}
