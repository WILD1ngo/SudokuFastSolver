using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Board;

public class Board
{
    

    public Cell[,] cells;
    // Contains all cells in the board
    // 2D array
    // 1 0 0 
    // 0 0 0
    // 0 0 0
    //
    public List<Cell>[] regions { get; set; } // Contains all regions (rows, columns, boxes)

    // In 9x9 grid:
    // regions[0] to regions[8]     -> represent rows
    // regions[9] to regions[17]    -> represent columns
    // regions[18] to regions[26]   -> represent 3x3 boxes




    public Settings settings;

    public Board(int[,] initialGrid, Settings settings)
    {
        this.settings = settings;
        cells = new Cell[settings.GridSize, settings.GridSize];
        regions = new List<Cell>[settings.GridSize * 3];  // Rows + Columns + Boxes

        InitializeCells();
        InitializeRegions();
        SetBoard(initialGrid);
    }


    /// <summary>
    /// Initializes all cells in the board with their coordinates.
    /// </summary>
    private void InitializeCells()
    {
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                cells[row, col] = new Cell { Row = row, Col = col };
            }
        }
    }

    /// <summary>
    /// Initializes all regions (rows, columns, and boxes) with references to the appropriate cells.
    /// </summary>
    private void InitializeRegions()
    {
        // Initialize arrays
        for (int i = 0; i < settings.GridSize * 3; i++)
        {
            regions[i] = new List<Cell>();
        }

        // Add rows
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                regions[row].Add(cells[row, col]);                           // Rows
                regions[col + settings.GridSize].Add(cells[row, col]);       // Columns

                // Calculate box index and add to boxes
                int boxRow = row / settings.BoxSize;
                int boxCol = col / settings.BoxSize;
                int boxIndex = boxRow * settings.BoxSize + boxCol + (settings.GridSize * 2);
                regions[boxIndex].Add(cells[row, col]);
            }
        }
    }

    /// <summary>
    /// Sets the board state from a 2D array and analyzes initial possibilities.
    /// </summary>
    public void SetBoard(int[,] grid)
    {
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                cells[row, col].Value = grid[row, col];
            }
        }
        AnalyzePossibilities();
    }

    /// <summary>
    /// Analyzes and updates all possibilities for empty cells based on current board state.
    /// </summary>
    public void AnalyzePossibilities()
    {
        // Reset possibilities for empty cells
        foreach (var cell in cells)
        {
            if (cell.Value == 0)
            {
                cell.Possibilities.Clear();
                cell.Possibilities.AddRange(Enumerable.Range(1, settings.GridSize));
            }
            else
            {
                cell.Possibilities.Clear();
            }
        }

        // Remove invalid possibilities based on regions
        foreach (var region in regions)
        {
            var usedValues = region.Where(c => c.Value != 0).Select(c => c.Value).ToList();
            foreach (var cell in region.Where(c => c.Value == 0))
            {
                cell.Possibilities.RemoveAll(p => usedValues.Contains(p));
            }
        }
    }

    /// <summary>
    /// Sets a value in a specific cell and updates affected cells' possibilities.
    /// </summary>
    public void SetValue(int row, int col, int value)
    {
        var cell = cells[row, col];
        int oldValue = cell.Value;
        cell.Value = value;

        if (value != 0)
        {
            cell.Possibilities.Clear();
            UpdateAffectedCells(row, col, value);
        }
        else
        {
            RestorePossibilities(row, col, oldValue);
            AnalyzePossibilities();
        }
    }

    /// <summary>
    /// Updates possibilities for all cells affected by a change at the specified position.
    /// </summary>
    /// <param name="row">Row index of the changed cell</param>
    /// <param name="col">Column index of the changed cell</param>
    /// <param name="value">New value that was set</param>
    private void UpdateAffectedCells(int row, int col, int value)
    {
        // Remove 'value' from possibilities in the same row, column, and box
        foreach (var cell in regions[row])
        {
            // Remove 'value' from possibilities
            cell.Possibilities.Remove(value);
        }
        foreach (var cell in regions[settings.GridSize + col])
        {
            // Remove 'value' from possibilities
            cell.Possibilities.Remove(value);
        }
        int boxRow = row / settings.BoxSize;
        int boxCol = col / settings.BoxSize;
        int boxIndex = boxRow * settings.BoxSize + boxCol + (settings.GridSize * 2);
        foreach (var cell in regions[boxIndex])
        {
            // Remove 'value' from possibilities
            cell.Possibilities.Remove(value);
        }

    }

    /// <summary>
    /// Restores possibilities for cells affected by removing a value.
    /// </summary>
    /// <param name="row">Row index of the changed cell</param>
    /// <param name="col">Column index of the changed cell</param>
    /// <param name="oldValue">Value that was removed</param>
    private void RestorePossibilities(int row, int col, int oldValue)
    {
        foreach (var region in regions.Where(r => r.Contains(cells[row, col])))
        {
            foreach (var cell in region.Where(c => c.Value == 0))
            {
                if (!cell.Possibilities.Contains(oldValue) && IsValuePossible(cell, oldValue))
                {
                    cell.Possibilities.Add(oldValue);
                    cell.Possibilities.Sort();
                }
            }
        }
    }
    /// <summary>
    /// Checks if a value can be legally placed in a cell.
    /// </summary>
    /// <param name="cell">The cell to check</param>
    /// <param name="value">The value to check</param>
    /// <returns>True if the value is legal in this cell</returns>
    private bool IsValuePossible(Cell cell, int value)
    {
        return !regions.Where(r => r.Contains(cell))
                      .Any(r => r.Any(c => c.Value == value));
    }

    /// <summary>
    /// Checks if the current board state is valid according to puzzle rules.
    /// </summary>
    /// <returns>True if the board is valid and complete</returns>
    public bool IsValid()
    {
        foreach (var region in regions)
        {
            var values = region.Select(c => c.Value).Where(v => v != 0);
            if (values.Count() != values.Distinct().Count())
                return false;
        }
        return true;
    }


    /// <summary>
    /// Creates a deep copy of the current board, including cell values and possibilities.
    /// </summary>
    /// <returns>A new Board instance with identical state</returns>
    public Board Clone()
    {
        // Extract current cell values into a grid
        int[,] grid = new int[settings.GridSize, settings.GridSize];
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                grid[row, col] = cells[row, col].Value;
            }
        }

        // Create new board with the extracted grid and same settings
        Board clonedBoard = new Board(grid, settings);

        // Deep copy possibilities for each cell
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                clonedBoard.cells[row, col].Possibilities = new List<int>(cells[row, col].Possibilities);
            }
        }

        return clonedBoard;
    }


    /// <summary>
    /// Prints the current board state with formatting based on settings.
    /// </summary>
    public void Print()
    {
        var line = settings.ShowGridLines ? new string('-', settings.GridSize * 3 + settings.BoxSize + 1) : "";

        for (int i = 0; i < settings.GridSize; i++)
        {
            if (settings.ShowGridLines)
            {
                if (i % settings.BoxSize == 0) Console.WriteLine(line);
                Console.Write("|");
            }

            for (int j = 0; j < settings.GridSize; j++)
            {
                if (cells[i, j].Value == 0)
                    Console.Write(settings.ShowEmptySquare ? $" {settings.EmptyCell} " : "   ");
                else
                    Console.Write($" {cells[i, j].Value} ");

                if (settings.ShowGridLines && (j + 1) % settings.BoxSize == 0)
                    Console.Write("|");
            }
            Console.WriteLine();
        }

        if (settings.ShowGridLines) Console.WriteLine(line);
    }

    /// <summary>
    /// Copies all cell values and possibilities from another board with identical settings
    /// </summary>
    /// <param name="source">Board to copy from</param>
    /// <exception cref="InvalidOperationException">Thrown if boards have different settings</exception>
    public void CopyFrom(Board source)
    {
        // Validate board compatibility
        if (this.settings.GridSize != source.settings.GridSize ||
            this.settings.BoxSize != source.settings.BoxSize)
        {
            throw new InvalidOperationException("Cannot copy from board with different settings");
        }

        // Copy cell values and possibilities
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                var sourceCell = source.cells[row, col];
                var targetCell = this.cells[row, col];

                // Direct value copy
                targetCell.Value = sourceCell.Value;

                // Deep copy possibilities
                targetCell.Possibilities.Clear();
                targetCell.Possibilities.AddRange(sourceCell.Possibilities);
            }
        }
    }
    /// <summary>
    /// Returns a string representation of the board.
    /// </summary>
    /// <returns>A string containing all cell values in row-major order</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                sb.Append(cells[row, col].Value);
            }
        }
        return sb.ToString();
    }
}