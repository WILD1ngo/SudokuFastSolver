using System;
using System.Collections.Generic;
using System.Linq;
using static Board;

public class Solver
{
    private readonly Board board;
    private bool solved;
    public TimeTracker timeTracker = new TimeTracker();
    public int deathCount = 0;



    /// <summary>
    /// Initializes a new solver instance and starts solving the puzzle.
    /// </summary>
    public Solver(Board board)
    {
        this.board = board;
        timeTracker.Start();
        solved = Solve();
        timeTracker.Stop();
    }






    /// <summary>
    /// Main solving method that applies rules before attempting backtracking.
    /// </summary>
    private bool Solve()
    {
        // Try rule-based solving 
        ApplyRules();

        deathCount++;

        var (row, col, possibilities) = FindCellWithMinPossibilities();


        // Puzzle solved condition (no empty cells)
        if (possibilities == -1) return true;

        // Contradiction found (empty cell with no possibilities)
        if (possibilities == 0) return false;

        
        //Console.WriteLine($"Guessing at {row}, {col}");
         

        foreach (var guess in board.cells[row, col].Possibilities.ToList())
        {
            board.SetValue(row, col, guess);
            //Console.WriteLine($"Guessing at {row}, {col} with {guess}");
            //board.Print();
            

            
            if (Solve())
            {
                // Commit successful solution
                return true;
            }
            board.SetValue(row, col, 0);
            //board.Print();
        }

        // All possibilities exhausted
        return false; // No valid solutions in this branch
    }


    /// <summary>
    /// Applies solving rules and returns the number of solutions found.
    /// </summary>
    private void ApplyRules()
    {
        while (true)
        {
            // Try the simpler rules first
            if (ApplySingleRule())
                continue;
            if (ApplyNakedRule())
                continue;
            break;
        }

    }
    /// <summary>
    /// Applies the single candidate rule to find cells with only one possibility.
    /// </summary>
    private bool ApplySingleRule()
    {
        bool match = false;
        for (int row = 0; row < board.settings.GridSize; row++)
        {
            for (int col = 0; col< board.settings.GridSize; col++)
            {
                
                if (board.cells[row, col].Value == 0 && board.cells[row,col].Possibilities.Count == 1) 
                {
                    match = true;
                    int value = board.cells[row, col].Possibilities[0];  // Get the actual value 
                    board.SetValue(row, col, value);
                }
            }
        }
        return match;
    }

    /// <summary>
    /// Applies the naked pairs/triples rule to find and eliminate candidates.
    /// </summary>
    private bool ApplyNakedRule()
    {
        bool match = false;
        for (int i = 0; i < board.settings.GridSize * 3; i++)  // 9 rows + 9 columns + 9 blocks
        {
            if (ApplyNakedRuleToRegion(board.regions[i]))
            {
                match = true;
            }
        }
        return match;
    }

    /// <summary>
    /// Applies naked tuple rules to a specific region (row, column, or block).
    /// </summary>
    private bool ApplyNakedRuleToRegion(List<Cell> region)
    {
        bool match = false;
        var emptyCells = new List<int>();

        // Find empty cells in the region
        for (int i = 0; i < board.settings.GridSize; i++)
        {
            if (region[i].Value == 0)
            {
                emptyCells.Add(i);
            }
        }

        // Check for naked 2 - 8 pairs
        for (int r = 2; r <= board.settings.GridSize - 1; r++)
        {
            var combinations = FindCombinations(emptyCells, r, 0);
            foreach (var combo in combinations)
            {
                var numbers = new bool[board.settings.GridSize];

                // Collect all candidate values for the combination
                foreach (int cellIndex in combo)
                {
                    foreach (int value in board.cells[region[cellIndex].Row, region[cellIndex].Col].Possibilities)
                    {
                        if (value > 0)  // Skip the first element (current value)
                            numbers[value - 1] = true;
                    }
                }

                // Count how many numbers are used
                int count = numbers.Count(x => x);

                if (count <= r)
                {
                    // Found a naked tuple, remove these values from other cells
                    for (int i = 0; i < board.settings.GridSize; i++)
                    {
                        if (!combo.Contains(i) && region[i].Value == 0)
                        {
                            bool changed = false;
                            for (int value = 1; value <= board.settings.GridSize; value++)
                            {
                                if (numbers[value - 1])
                                {
                                    if (board.cells[region[i].Row, region[i].Col].Possibilities.Remove(value))
                                        changed = true;
                                }
                            }
                            if (changed)
                                match = true;
                        }
                    }
                }
            }
        }
        return match;
    }

    /// <summary>
    /// Recursive method to find all possible combinations.
    /// </summary>
    private List<List<int>> FindCombinations(List<int> numbers, int r, int startIndex)
    {
        if (r == 0)
        {
            return new List<List<int>> { new List<int>() };
        }

        if (startIndex >= numbers.Count)
        {
            return new List<List<int>>();
        }

        var combinations = new List<List<int>>();

        // Include current number
        var withCurrent = FindCombinations(numbers, r - 1, startIndex + 1);
        foreach (var combo in withCurrent)
        {
            combo.Add(numbers[startIndex]);
            combinations.Add(combo);
        }

        // Exclude current number
        var withoutCurrent = FindCombinations(numbers, r, startIndex + 1);
        combinations.AddRange(withoutCurrent);

        return combinations;
    }


    private (int row, int col, int possibilities) FindCellWithMinPossibilities()
    {
        int minRow = -1, minCol = -1;
        int minCount = int.MaxValue;
        bool solved = true;

        for (int row = 0; row < board.settings.GridSize; row++)
        {
            for (int col = 0; col < board.settings.GridSize; col++)
            {
                Cell cell = board.cells[row, col];

                if (cell.Value != 0) continue;
                solved = false;
                int currentCount = cell.Possibilities.Count;

                if (currentCount < minCount)
                {
                    minCount = currentCount;
                    minRow = row;
                    minCol = col;
                }
            }
        }
        if (solved) return (-1, -1, -1);
        return (minRow, minCol, minCount);
    }

    /// <summary>
    /// Prints the solution and solving statistics.
    /// </summary>
    public void PrintSolution()
    {
        Console.WriteLine($"Backtrack steps: {deathCount}");
        timeTracker.PrintTime();
        if (solved)
        {
            Console.WriteLine("\nSolved Board:");
            board.Print();
        }
        else
        {

            Console.WriteLine("\nNo solution exists.");
            
        }
    }
}