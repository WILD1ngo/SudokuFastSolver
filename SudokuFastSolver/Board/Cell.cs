/// <summary>
/// Represents a cell position in the Sudoku grid. its a readonly struct for performance.
/// </summary>
public readonly struct Cell : IEquatable<Cell>
{
    // Using auto-properties as they are optimized by the compiler for readonly structs
    public int Row { get; }
    public int Col { get; }
    public int Box { get; }

    public Cell(int row, int col, int box)
    {
        Row = row;
        Col = col;
        Box = box;
    }

    // IEquatable<T> for better commpreing
    public bool Equals(Cell other) =>
        Row == other.Row && Col == other.Col && Box == other.Box;

    public override bool Equals(object obj) =>
        obj is Cell other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(Row, Col, Box);
}