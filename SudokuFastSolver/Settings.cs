public struct Settings
{
    public int GridSize { get; init; }
    public bool ShowEmptySquare { get; init; }
    public string EmptyCell { get; init; }
    public bool ShowGridLines { get; init; }
    public int ExpectedLength { get; init; }
    // The max size of the grid
    public readonly int MaxGridSize = 25;
    public Settings(int gridSize, bool showEmptySquare, string emptyCell, bool showGridLines) 
    {
        GridSize = gridSize;
        ExpectedLength = GridSize * GridSize;
        ShowEmptySquare = showEmptySquare;
        EmptyCell = emptyCell;
        ShowGridLines = showGridLines;
        this.Validate();
    }






    private void Validate() 
    //the function Validate() is used to check if the settings are valid
    {

        // Check if GridSize is positive
        if (GridSize <= 0)
        {
            throw new InvalidGridSizeException($"Grid size must be positive. Got: {GridSize}");
        }



        // Check if GridSize has an integer square root
        double squareRoot = Math.Sqrt(GridSize);

        if (squareRoot % 1 != 0)
        {
            throw new InvalidGridSizeException(
                $"Grid size must have an integer square root. {GridSize} has square root of {squareRoot}");
        }



        // Check if GridSize is within reasonable bounds (optional)
        if (GridSize > MaxGridSize)
        {
            throw new InvalidGridSizeException(
                $"Grid size {GridSize} is too large. Maximum supported size is 25");
        }


        // Validate EmptyCell
        if (string.IsNullOrEmpty(EmptyCell))
        {
            throw new InvalidGridSizeException("EmptyCell cannot be null or empty");
        }
    }

    public int BoxSize => (int)Math.Sqrt(GridSize);
}