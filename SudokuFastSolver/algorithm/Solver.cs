using System.Data;
using System.Net.Http.Headers;

public class Solver
{
    private readonly Board board;
    private bool solved;
    public TimeTracker timeTracker = new TimeTracker();
    public Solver(Board board)
    {
        this.board = board;

        timeTracker.Start();
        solved = Solve();
        timeTracker.Stop();
        
    }


    // because it is a complex algorithm with recursion
    // so theres a lot of text but its quite simple
    private bool Solve()
    //The function Solve() is used to solve the sudoku board
    {

        //Find the row and col with the lest amount of options
        var (row, col ,count) = FindCellWithFewestOptions();

        //If the row is equal to the size of the board
        //and the column is equal to the size of the board,
        //return true

        if (row == -1 && col == -1)
            return true;


        




        //try to fill it with a number
        for (int num = 1; num <= board.Size; num++)
        {
            //check if the number is safe to put in the cell
            if (IsSafe(row, col, num))
            {
                if (count > 1)
                {
                    /*
                    int NakedPairRow = 0;
                    int NakedPairCol = 0;
                    (NakedPairRow, NakedPairCol) = FindNakedPair(row, col, num);
                     if (NakedPairRow != 0 && NakedPairCol != 0)
                    {
                        board.SetValue(row, col, num);
                        if (Solve())
                            return true;
                        board.SetValue(row, col, 0);
                        board.SetValue(NakedPairRow, NakedPairCol, num);
                        if (Solve())
                            return true;
                        board.SetValue(NakedPairRow, NakedPairCol, 0);
                        return false;
                    }
                    */

                }
                
                //if it is safe, put the number in the cell
                board.SetValue(row, col, num);

                if (Solve())
                    return true;
                //}
                //if it is not safe, backtrack
                board.SetValue(row, col, 0);
                
            }
        }
        //if no number is safe to put in the cell, return false
        //its unsolvable
        return false;
    }

    
    private bool IsSafe(int row, int col, int num)
    //The function IsSafe() is used to check if a number can be placed in a cell
    {
        return !(board.rows[row].Contains[num - 1] ||
            board.cols[col].Contains[num - 1] ||
            board.boxes[row / board.settings.BoxSize * board.settings.BoxSize + col / board.settings.BoxSize].Contains[num - 1]);
    }

    private (int row, int col) FindNakedPair(int row, int col, int num)
    {
        int boxStartRow = (row / board.settings.BoxSize) * board.settings.BoxSize; // Calculate the starting row of the box
        int boxStartCol = (col / board.settings.BoxSize) * board.settings.BoxSize; // Calculate the starting column of the box
        int count = 0; // Count of cells that can accept 'num'
        int returnRow = 0; 
        int returnCol = 0; 

        // Iterate through the 3x3 box
        for (int r = boxStartRow; r < boxStartRow + board.settings.BoxSize; r++)
        {
            for (int c = boxStartCol; c < boxStartCol + board.settings.BoxSize; c++)
            {
                //Console.WriteLine($"r = {r} , c = {c}");
                // Skip the current cell if it's already filled
                if (board.GetValue(r, c) != 0)
                    continue;
                if (r == row && c == col)
                    continue;

                // Check if 'num' is a valid option for this cell
                if (IsSafe(r, c, num))
                {
                    count++;
                    returnRow = r;
                    returnCol = c;
                    // If count exceeds 2, early exit since it's not a naked pair
                    if (count > 1)
                        return (0, 0);
                }
            }
        }

        // Return true if 'num' can only be placed in exactly two cells in the box
        // and the current cell is one of them
        return (returnRow, returnCol);
    }
    private int CountValidNumbers(int row, int col)
    {
        int count = 0;
        for (int num = 1; num <= board.Size; num++)
        {
            if (IsSafe(row, col, num))
                count++;
        }
        return count;
    }
    private (int row, int col , int count) FindCellWithFewestOptions()
    {
        int minOptions = int.MaxValue;
        int bestRow = -1, bestCol = -1;

        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.IsEmpty(row, col))
                {
                    int options = CountValidNumbers(row, col);
                    if (options < minOptions)
                    {
                        minOptions = options;
                        bestRow = row;
                        bestCol = col;

                        // Exit early if a cell with one option is found
                        if (minOptions == 1)
                            return (bestRow, bestCol , 1);

                    }
                }
            }
        }

        return (bestRow, bestCol , minOptions);
    }

    public void PrintSolution()
    {

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