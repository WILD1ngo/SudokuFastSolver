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

    public bool Solve() => Solve(0, 0);
    // I try to explain the algoritem as simple as possible
    // because it is a complex algorithm with recursion
    // so theres a lot of text but its quite simple
    private bool Solve(int row, int col)
    //The function Solve() is used to solve the sudoku board
    {
        //running form the top left corner of the board
        //to the bottom right corner of the board
        //
        // 1 0 0 0 0 
        // 0 0 0 0 0
        // 0 0 0 0 0
        // 0 0 0 0 2
        // 
        // 1 is the first row and 2 is the last row

        //If the row is equal to the size of the board
        //and the column is equal to the size of the board,
        //return true
        if (row == board.Size - 1 && col == board.Size)
            return true;


        //Check if it got to the end of the column
        //THEN move to the next row
        //and git to the start of the column
        if (col == board.Size)
        {
            row++;
            col = 0;
        }

        //If the cell is not empty
        //move to the next cell
        if (!board.IsEmpty(row, col))
            return Solve(row, col + 1);

        //If the cell is empty
        //try to fill it with a number
        for (int num = 1; num <= board.Size; num++)
        {
            //check if the number is safe to put in the cell
            if (IsSafe(row, col, num))
            {
                //if it is safe, put the number in the cell
                board.SetValue(row, col, num);
                //move to the next cell
                if (Solve(row, col + 1))
                    return true;
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