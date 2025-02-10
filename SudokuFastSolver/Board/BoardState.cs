
/// <summary>
/// struct to save the state of the board
/// 
/// btw its stack alloc (:
/// </summary>
public struct BoardState
{
    public int[] board;
    public uint[] rows;
    public uint[] cols;
    public uint[] boxes;
    public List<Cell> emptyCells;
}