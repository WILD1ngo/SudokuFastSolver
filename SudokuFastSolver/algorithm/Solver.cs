
using System.Numerics;

public class Solver
{

    // Stores reference to the Board being solved
    private readonly Board board;



    /// <summary>
    /// Constractor for the solver saves the board pointer it gets
    /// </summary>
    public Solver(Board board)
    {
        this.board = board;
    }




    /// <summary>
    /// 
    /// Entery point to the solver 
    /// 
    /// 
    /// Validate the board 
    /// This acticvates the hidden set 
    /// (Hidden sets explaintion in the board file but its like hidden singles just for every combinations of numbers)
    /// and removes a lot of hard unsolvable boards
    /// 
    /// works only at the start of the algorithm
    /// 
    /// then activate the main algorithem solver
    /// 
    /// </summary>
    public bool Solve()
    {
        // Validate the puzzle first.
        if (!board.ValidateInitialPuzzle())
            return false;

        // Pre-check: ensure every empty cell has at least one candidate.
        foreach (Cell cell in board.emptyCells)
        {
            uint possible = board.rows[cell.Row] & board.cols[cell.Col] & board.boxes[cell.Box];
            if (possible == 0)
                return false;
        }
        return Algorithm();
    }






    /// <summary>
    /// Core solving algorithm
    ///
    /// 
    /// 
    /// 
    /// Recursive backtracking algorithm for solving Sudoku puzzles.
    /// Uses multiple techniques:
    /// 1. State management for backtracking
    /// 2. Heuristic-based solving
    ///  2.1 Naked single (explaintion for this in the huristics file) 
    ///  2.2 Hidden single (same for this)
    /// 3. Most constrained variable selection
    /// 4. Least constraining value ordering
    ///
    /// 
    /// 
    /// 
    /// So how does the algorithm works? 
    /// I added nice phases inside the code 
    /// welcome to follow 
    private bool Algorithm()
    {


        // Phase 1: Save current state and apply heuristics
        BoardState state = board.SaveState();
        if (!Heuristics.ApplyHeuristics(board))
        {
            board.RestoreState(state);
            return false;
        }



        // Phase 2: Check if solved
        if (board.emptyCells.Count == 0)
            return true;




        // Phase 3: Select most constrained cell
        MoveCellWithLowestPossibilitiesToFront(0);
        Cell cell = board.emptyCells[0];
        board.emptyCells.RemoveAt(0);




        // Phase 4: Get possible values for selected cell
        // this is a little bit messi because bitwise
        uint possibleCandidates = board.rows[cell.Row] &
                                 board.cols[cell.Col] &
                                 board.boxes[cell.Box];
        int candidateCount = BitOperations.PopCount(possibleCandidates);




        // Phase 5: Extract individual candidates

        Span<uint> candidates = stackalloc uint[candidateCount];
        // Uses stack allocation for temporary arrays realy 
        // makes this a faster
        // and we hate allocating array in the heap even if c# forces us
        // and its a local var
        //
        // aka : its probbley better to write this in cpp and not this garbage c#

        int candidateIndex = 0;
        while (possibleCandidates != 0)
        {
            uint candidateBit = possibleCandidates & (uint)-(int)possibleCandidates;
            candidates[candidateIndex++] = candidateBit;
            possibleCandidates &= ~candidateBit;
        }

        // Phase 6: Order candidates optimally
        OrderCandidatesByLeastConstraining(cell.Row, cell.Col, cell.Box,
                                         candidates, candidateCount);

        // Phase 7: Try each candidate recursively
        for (int i = 0; i < candidateCount; i++)
        {
            uint candidateBit = candidates[i];
            BoardState candidateState = board.SaveState();

            // Apply the candidate
            board.rows[cell.Row] ^= candidateBit;
            board.cols[cell.Col] ^= candidateBit;
            board.boxes[cell.Box] ^= candidateBit;
            board.board[cell.Row * board.size + cell.Col] =
                BitOperations.TrailingZeroCount(candidateBit) + 1;

            // Recurse
            if (Algorithm())
                return true;

            // Backtrack if needed
            board.RestoreState(candidateState);
        }

        return false;
    }








    /// <summary>
    /// Select cell with fewest possibilities
    /// 
    /// This is pretty simple 
    /// It just goes in loop for all cells 
    /// And picks the one with the fewest bits lit
    /// 
    /// </summary>
    private void MoveCellWithLowestPossibilitiesToFront(int idx)
    {
        int bestIdx = idx;
        uint possible = board.rows[board.emptyCells[bestIdx].Row] & board.cols[board.emptyCells[bestIdx].Col] & board.boxes[board.emptyCells[bestIdx].Box];
        int bestCount = BitOperations.PopCount(possible);
        for (int i = idx + 1; i < board.emptyCells.Count; i++)
        {
            possible = board.rows[board.emptyCells[i].Row] & board.cols[board.emptyCells[i].Col] & board.boxes[board.emptyCells[i].Box];
            int currentCount = BitOperations.PopCount(possible);
            if (currentCount < bestCount)
            {
                bestIdx = i;
                bestCount = currentCount;
                if (bestCount == 0)
                    break;
            }
        }
        if (bestIdx != idx)
        {
            Cell temp = board.emptyCells[idx];
            board.emptyCells[idx] = board.emptyCells[bestIdx];
            board.emptyCells[bestIdx] = temp;
        }
    }










    /// <summary>
    /// The function OrderCandidatesByLeastConstraining sorts possible values for a 
    /// Sudoku cell based on how much they 
    /// constrain other empty cells' possibilities.
    /// 
    /// What is constrain? this is a great question
    /// 
    /// For each candidate number that could go in the current cell
    /// it counts how many other empty cells would have their possibilities reduced if we used that number
    /// The fewer other cells affected, the better ("least constraining")
    /// 
    /// 
    /// so how it works:
    /// Constraint Counting:
    /// The function checks three types of constraints for each candidate:
    ///     Checks all empty cells in the same row
    ///     Similarly checks all empty cells in the same column
    ///     Checks all empty cells in the same 3x3 box
    /// Then
    /// Uses insertion sort to order candidates
    /// 
    /// 
    /// aka:
    /// its pretty simple but it took me 3 hours to make this work 
    /// I dont know why this took so long
    /// FUCK
    /// </summary>
    private void OrderCandidatesByLeastConstraining(int row, int col, int box, Span<uint> candidates, int count)
    {
        // Local array on the stack to be faster 
        Span<int> candidateConstraints = stackalloc int[count];
        BoxInfo info = board.boxInfos[box];

        for (int i = 0; i < count; i++)
        {
            uint candidate = candidates[i];
            int constraintCount = 0;
            // Evaluate row constraints.
            for (int c = 0; c < board.size; c++)
            {
                if (c == col)
                    continue;
                if (board.board[row * board.size + c] == 0)
                {
                    uint poss = board.rows[row] & board.cols[c] & board.boxes[(row / board.sqrtSize) * board.sqrtSize + (c / board.sqrtSize)];
                    if ((poss & candidate) != 0)
                        constraintCount++;
                }
            }
            // Evaluate column constraints.
            for (int r = 0; r < board.size; r++)
            {
                if (r == row)
                    continue;
                if (board.board[r * board.size + col] == 0)
                {
                    uint poss = board.rows[r] & board.cols[col] & board.boxes[(r / board.sqrtSize) * board.sqrtSize + (col / board.sqrtSize)];
                    if ((poss & candidate) != 0)
                        constraintCount++;
                }
            }
            // Evaluate box constraints.
            for (int r = info.StartRow; r <= info.EndRow; r++)
            {
                for (int c = info.StartCol; c <= info.EndCol; c++)
                {
                    if (r == row && c == col)
                        continue;
                    if (board.board[r * board.size + c] == 0)
                    {
                        uint poss = board.rows[r] & board.cols[c] & board.boxes[box];
                        if ((poss & candidate) != 0)
                            constraintCount++;
                    }
                }
            }
            candidateConstraints[i] = constraintCount;
        }



        // Insertion sort for least constraining candidates.
        for (int i = 1; i < count; i++)
        {
            uint keyCandidate = candidates[i];
            int keyConstraint = candidateConstraints[i];
            int j = i - 1;
            while (j >= 0 && candidateConstraints[j] > keyConstraint)
            {
                candidates[j + 1] = candidates[j];
                candidateConstraints[j + 1] = candidateConstraints[j];
                j--;
            }
            candidates[j + 1] = keyCandidate;
            candidateConstraints[j + 1] = keyConstraint;
        }
    }
}