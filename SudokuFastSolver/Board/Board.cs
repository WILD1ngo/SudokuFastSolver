using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Threading.Channels;
using System.Threading;
using System.Text;


/// <summary>
/// Board class stores the board 
/// 
///  also checks if the board is valid
///  uses some huristics to check
/// </summary>
public class Board
{
    /// <summary>
    /// The size of one side of the Sudoku board
    /// like 4 , 9 , 16 , 25
    /// </summary>
    public int size;
    



    /// <summary>
    /// The square root of the board size 
    /// for example (3 for a 9x9 puzzle).
    /// </summary>
    public int sqrtSize;



    /// <summary>
    /// One-dimensional array representing the Sudoku board.
    /// Values are stored row by row,
    /// For a 9x9 board, size of 81
    /// </summary>
    public int[] board;



    /// <summary>
    /// List of Cell objects representing empty positions on the board.
    /// Each Cell contains row, column, and box information.
    /// Used to track remaining cells to be filled during solving.
    /// 
    /// very importent for changing the order of cells for sorting then finding where
    /// to change staff
    /// </summary>
    public List<Cell> emptyCells;


    /// <summary>
    /// Bitmask array for each row, where each bit represents a possible value.
    /// For example, in a 9x9 puzzle,
    /// if rows[0] = 0b111111111, it means
    /// all numbers 1-9 are still possible in that row.
    /// When a number is placed, its corresponding bit is cleared.
    /// 
    /// 00000000
    /// 
    /// also its a uint (unsigned int because all 32 bit needed)
    /// 
    /// <summary>
    public uint[] rows;




    /// <summary>
    /// Same for colums
    /// </summary>
    public uint[] cols;


    /// <summary>
    /// Same for boxes 
    /// box definetion is 3x3 (in a 9x9 board)
    /// </summary>
    public uint[] boxes;


    /// <summary>
    // Bitmask array storing the column positions for each box.
    /// Used to quickly determine which columns intersect with a particular box.
    /// For a 9x9 puzzle, boxes 0,3,6 have columns 0-2, boxes 1,4,7 have columns 3-5, etc.
    /// </summary>
    public uint[] boxColumnMasks;


    /// <summary>
    /// Array of BoxInfo containing the boundary information for each box.
    /// Each BoxInfo stores the starting and ending row/column indices for that box.
    /// Used for efficient iteration
    /// 
    /// over cells within a specific box
    /// 
    /// 
    /// </summary>
    public BoxInfo[] boxInfos;


    /// <summary>
    /// Initializes a new instance of the Board class with pre-allocated space for empty cells.
    /// 
    /// 
    /// </summary>
    public Board()
    {
        // Pre-allocate for 9x9 board.
        emptyCells = new List<Cell>(81);
    }





    /// <summary>
    /// Loads a Sudoku puzzle from a string input. The input string can contain whitespace,
    /// which is ignored. Supports puzzle sizes of 4x4, 9x9, 16x16, and 25x25.
    ///
    /// Throws ArgumentException if the input length is invalid or contains invalid characters.
    /// 
    /// 
    /// probebly not the most generic code i have ever seen 
    /// TODO : change this garbage code
    /// </summary>
    /// <param name="input">String representation of the Sudoku puzzle</param>
    public void LoadPuzzle(string input)
    {
        int inputLength = 0;
        for (int i = 0; i < input.Length; ++i)
        {
            if (!char.IsWhiteSpace(input[i]))
                inputLength++;
        }

        size = (int)Math.Sqrt(inputLength);
        if (size * size != inputLength || !(size == 4 || size == 9 || size == 16 || size == 25))
            throw new ArgumentException($"{size * size} is an Invalid input length ");

        board = new int[inputLength];
        int boardIndex = 0;
        for (int i = 0; i < input.Length; ++i)
        {
            char c = input[i];
            if (!char.IsWhiteSpace(c))
            {
                int value = c - '0';
                if (value < 0 || value > size)
                    throw new ArgumentException($"Invalid character '{c}' at position {i}");
                board[boardIndex++] = value;
            }
        }
        Init();
    }




    /// <summary>
    /// Initializes the board's internal state, including rows, columns, and boxes masks.
    /// Sets up box information and finds empty cells. 
    /// 
    /// Also applies initial solving heuristics.
    /// 
    /// this is very very very IMPORTENT!!!!
    /// removes unsolvable boards problems
    /// 
    /// 
    /// Called after loading a new puzzle.
    /// </summary>
    private void Init()
    {
        emptyCells.Clear();

        // Initialize rows, cols, and boxes.
        rows = new uint[size];
        cols = new uint[size];
        boxes = new uint[size];
        uint startBits = GetStartBits();
        for (int i = 0; i < size; i++)
        {
            rows[i] = startBits;
            cols[i] = startBits;
            boxes[i] = startBits;
        }

        sqrtSize = (int)Math.Sqrt(size);
        boxColumnMasks = new uint[size];
        boxInfos = new BoxInfo[size];

        for (int b = 0; b < size; b++)
        {
            int boxColGroup = b % sqrtSize;
            boxColumnMasks[b] = (((1u << sqrtSize) - 1)) << (boxColGroup * sqrtSize);

            int boxRow = b / sqrtSize;
            int boxCol = b % sqrtSize;
            boxInfos[b] = new BoxInfo
            {
                StartRow = boxRow * sqrtSize,
                EndRow = (boxRow + 1) * sqrtSize - 1,
                StartCol = boxCol * sqrtSize,
                EndCol = (boxCol + 1) * sqrtSize - 1
            };
        }

        FindEmptyCellsAndPossibleValuesForTheCell();
        // Apply initial heuristics.
        Heuristics.ApplyHeuristics(this);
    }







    /// <summary>
    /// Scans the board to identify empty cells and updates the possible values masks
    /// for rows, columns, and boxes based on filled cells. Empty cells are added to
    /// the emptyCells list, while filled cells update the corresponding bitmasks.
    /// </summary>
    private void FindEmptyCellsAndPossibleValuesForTheCell()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                int box = (row / sqrtSize) * sqrtSize + (col / sqrtSize);
                int index = row * size + col;
                int boardValue = board[index];
                if (boardValue == 0)
                {
                    emptyCells.Add(new Cell(row, col, box));
                }
                else
                {
                    uint value = 1u << (boardValue - 1);
                    // XOR to remove candidate (assumes puzzle has no duplicate digits).
                    rows[row] ^= value;
                    cols[col] ^= value;
                    boxes[box] ^= value;
                }
            }
        }
    }











    /// <summary>
    /// Removes a specified cell from the emptyCells list. Used when a cell is filled
    /// during the solving process.
    /// </summary>
    /// <param name="cell">The cell to remove from emptyCells</param>
    private void RemoveEmptyCell(Cell cell)
    {
        for (int i = emptyCells.Count - 1; i >= 0; i--)
        {
            if (emptyCells[i].Equals(cell))
            {
                emptyCells.RemoveAt(i);
                break;
            }
        }
    }


    /// <summary>
    /// Returns the initial bitmask for candidate values based on the puzzle size.
    /// For example, a 9x9 puzzle returns 0x1FF (binary: 111111111).
    /// to show the allowed values to enter
    /// Throws ArgumentException for unsupported Sudoku sizes.
    /// </summary>
    /// <returns>uint bitmask representing all possible values for the given size</returns>
    private uint GetStartBits()
    {
        return size switch
        {
            4 => 0xFu,
            9 => 0x1FFu,
            16 => 0xFFFFu,
            25 => 0x1FFFFFFu,
            _ => throw new ArgumentException("Unsupported Sudoku size")
        };
    }






    /// <summary>
    /// Creates a deep copy of the current board state, including the board array,
    /// rows, columns, boxes masks, and empty cells list. Used for backtracking
    /// during solving.
    /// 
    /// importent for board changes
    /// </summary>
    /// <returns>BoardState containing the current state</returns>
    public BoardState SaveState()
    {
        return new BoardState
        {
            board = (int[])board.Clone(),
            rows = (uint[])rows.Clone(),
            cols = (uint[])cols.Clone(),
            boxes = (uint[])boxes.Clone(),
            emptyCells = new List<Cell>(emptyCells)
        };
    }





    /// <summary>
    /// Restores the board to a previously saved state.
    /// </summary>
    /// <param name="state">BoardState to restore from</param>
    public void RestoreState(BoardState state)
    {
        board = state.board;
        rows = state.rows;
        cols = state.cols;
        boxes = state.boxes;
        emptyCells = state.emptyCells;
    }




    /// <summary>
    /// Prints the board
    /// </summary>
    public void Print()
    {
        if (board == null || board.Length != size * size)
        {
            Console.WriteLine("Nothing to print");
            return;
        }
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                int value = board[row * size + col];
                char c = value == 0 ? '.' : (char)('0' + value);
                Console.Write($"{c} ");
                if ((col + 1) % sqrtSize == 0 && col != size - 1)
                    Console.Write("| ");
            }
            Console.WriteLine();
            if ((row + 1) % sqrtSize == 0 && row != size - 1)
            {
                int lineLength = size * 2 + (sqrtSize - 1) * 3 - 1;
                Console.WriteLine(new string('-', lineLength));
            }
        }
    }




    /// <summary>
    ///  Returns a simple string representation of the board.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(size*size);
        for (int i = 0; i < board.Length; i++)
        {
            sb.Append((char)(board[i] + '0'));
        }
        return sb.ToString();
    }





    /// <summary>
    /// Validates the initial puzzle state to ensure it follows Sudoku rules.
    /// Checks for duplicate digits in rows, columns, and boxes.
    /// Also verifies that all candidate digits have valid positions in empty cells
    /// and validates hidden pairs/triples in all regions.
    /// 
    /// 
    /// 
    /// 
    /// realy importent for unsolveable pazzles to eliminate them at the start
    /// 
    /// 
    /// </summary>
    /// <returns>true if the puzzle is valid, false otherwise</returns>
    public bool ValidateInitialPuzzle()
    {
        // Check for duplicate digits in each row.
        for (int r = 0; r < size; r++)
        {
            int seen = 0;
            for (int c = 0; c < size; c++)
            {
                int val = board[r * size + c];
                if (val != 0)
                {
                    int mask = 1 << (val - 1);
                    if ((seen & mask) != 0)
                        return false;
                    seen |= mask;
                }
            }
        }

        // Check for duplicate digits in each column.
        for (int c = 0; c < size; c++)
        {
            int seen = 0;
            for (int r = 0; r < size; r++)
            {
                int val = board[r * size + c];
                if (val != 0)
                {
                    int mask = 1 << (val - 1);
                    if ((seen & mask) != 0)
                        return false;
                    seen |= mask;
                }
            }
        }

        // Check for duplicate digits in each box.
        for (int b = 0; b < size; b++)
        {
            BoxInfo info = boxInfos[b];
            int seen = 0;
            for (int r = info.StartRow; r <= info.EndRow; r++)
            {
                for (int c = info.StartCol; c <= info.EndCol; c++)
                {
                    int val = board[r * size + c];
                    if (val != 0)
                    {
                        int mask = 1 << (val - 1);
                        if ((seen & mask) != 0)
                            return false;
                        seen |= mask;
                    }
                }
            }
        }



        // Check that every candidate digit appears in at least one empty cell for each region.
        // Rows.
        for (int r = 0; r < size; r++)
        {
            uint rowCandidates = rows[r];
            for (int num = 1; num <= size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((rowCandidates & mask) != 0)
                {
                    bool found = false;
                    for (int c = 0; c < size; c++)
                    {
                        if (board[r * size + c] == 0)
                        {
                            int b = (r / sqrtSize) * sqrtSize + (c / sqrtSize);
                            uint poss = rows[r] & cols[c] & boxes[b];
                            if ((poss & mask) != 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                        return false;
                }
            }
        }

        // Columns.
        for (int c = 0; c < size; c++)
        {
            uint colCandidates = cols[c];
            for (int num = 1; num <= size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((colCandidates & mask) != 0)
                {
                    bool found = false;
                    for (int r = 0; r < size; r++)
                    {
                        if (board[r * size + c] == 0)
                        {
                            int b = (r / sqrtSize) * sqrtSize + (c / sqrtSize);
                            uint poss = rows[r] & cols[c] & boxes[b];
                            if ((poss & mask) != 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                        return false;
                }
            }
        }

        // Boxes.
        for (int b = 0; b < size; b++)
        {
            uint boxCandidates = boxes[b];
            BoxInfo info = boxInfos[b];
            for (int num = 1; num <= size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((boxCandidates & mask) != 0)
                {
                    bool found = false;
                    for (int r = info.StartRow; r <= info.EndRow; r++)
                    {
                        for (int c = info.StartCol; c <= info.EndCol; c++)
                        {
                            if (board[r * size + c] == 0)
                            {
                                uint poss = rows[r] & cols[c] & boxes[b];
                                if ((poss & mask) != 0)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found)
                            break;
                    }
                    if (!found)
                        return false;
                }
            }
        }

        // --- Additional hidden pair/triple check ---
        // Validate each region using hidden sets.
        // Rows.
        for (int r = 0; r < size; r++)
        {
            List<uint> cellCands = new List<uint>();
            for (int c = 0; c < size; c++)
            {
                if (board[r * size + c] == 0)
                {
                    int b = (r / sqrtSize) * sqrtSize + (c / sqrtSize);
                    uint cand = rows[r] & cols[c] & boxes[b];
                    cellCands.Add(cand);
                }
            }
            if (cellCands.Count > 1 && !ValidateHiddenSetsInRegion(cellCands, rows[r], size))
                return false;
        }
        // Columns.
        for (int c = 0; c < size; c++)
        {
            List<uint> cellCands = new List<uint>();
            for (int r = 0; r < size; r++)
            {
                if (board[r * size + c] == 0)
                {
                    int b = (r / sqrtSize) * sqrtSize + (c / sqrtSize);
                    uint cand = rows[r] & cols[c] & boxes[b];
                    cellCands.Add(cand);
                }
            }
            if (cellCands.Count > 1 && !ValidateHiddenSetsInRegion(cellCands, cols[c], size))
                return false;
        }
        // Boxes.
        for (int b = 0; b < size; b++)
        {
            List<uint> cellCands = new List<uint>();
            BoxInfo info = boxInfos[b];
            for (int r = info.StartRow; r <= info.EndRow; r++)
            {
                for (int c = info.StartCol; c <= info.EndCol; c++)
                {
                    if (board[r * size + c] == 0)
                    {
                        uint cand = rows[r] & cols[c] & boxes[b];
                        cellCands.Add(cand);
                    }
                }
            }
            if (cellCands.Count > 1 && !ValidateHiddenSetsInRegion(cellCands, boxes[b], size))
                return false;
        }


        // Returns True if every thing is ok this is a solveable pazzle

        return true;
    }






    /// <summary>
    /// Validates hidden sets (pairs and triples) within a given region (row, column, or box).
    /// Used as part of the puzzle validation process to ensure the puzzle is solvable.
    ///
    /// how it works ?
    /// 
    /// All Hidden Subsets work the same way, the only thing that changes is the number of cells and candidates affected by the move.
    /// Take Hidden Pair: If you can find two cells within a house such as that two candidates 
    /// appear nowhere outside those cells in that house, those two candidates must be placed in the two cells. 
    /// All other candidates can therefore be eliminated.
    /// 
    /// 
    /// This code it a bit disgusting
    /// so i will explain what is does in pseudo code
    /// 
    /// Look for Hidden Pairs
    ///    Find two numbers that appear only in two specific cells.
    ///    If found, remove all other numbers from those two cells.
    ///    
    ///    Look for Hidden Triples
    ///Find three numbers that appear only in three specific cells.
    ///    If found, remove all other numbers from those three cells.
    ///    Ensure Every Digit Still Has a Place
    ///    
    ///Check that every required number still exists in at least one cell.
    ///If any number is missing, return false (invalid Sudoku state).
    ///Repeat if Changes Were Made
    ///
    ///
    /// 
    /// </summary>
    /// <param name="cellCands">List of candidate masks for cells in the region</param>
    /// <param name="regionCand">Candidate mask for the entire region</param>
    /// <param name="sudokuSize">Size of the Sudoku puzzle</param>
    /// <returns>true if the region's hidden sets are valid, false otherwise</returns>
    private bool ValidateHiddenSetsInRegion(List<uint> cellCands, uint regionCand, int sudokuSize)
    {
        bool changed;
        do
        {
            changed = false;
            // Check for hidden pairs.
            for (int d1 = 1; d1 <= sudokuSize; d1++)
            {
                uint bit1 = 1u << (d1 - 1);
                if ((regionCand & bit1) == 0)
                    continue;
                for (int d2 = d1 + 1; d2 <= sudokuSize; d2++)
                {
                    uint bit2 = 1u << (d2 - 1);
                    if ((regionCand & bit2) == 0)
                        continue;
                    uint pairMask = bit1 | bit2;
                    List<int> indices = new List<int>();
                    for (int i = 0; i < cellCands.Count; i++)
                    {
                        if ((cellCands[i] & bit1) != 0 || (cellCands[i] & bit2) != 0)
                            indices.Add(i);
                    }
                    if (indices.Count == 2)
                    {
                        foreach (int i in indices)
                        {
                            uint newMask = cellCands[i] & pairMask;
                            if (newMask != cellCands[i])
                            {
                                cellCands[i] = newMask;
                                if (newMask == 0)
                                    return false;
                                changed = true;
                            }
                        }
                    }
                }
            }

            // Check for hidden triples.
            for (int d1 = 1; d1 <= sudokuSize; d1++)
            {
                uint bit1 = 1u << (d1 - 1);
                if ((regionCand & bit1) == 0)
                    continue;
                for (int d2 = d1 + 1; d2 <= sudokuSize; d2++)
                {
                    uint bit2 = 1u << (d2 - 1);
                    if ((regionCand & bit2) == 0)
                        continue;
                    for (int d3 = d2 + 1; d3 <= sudokuSize; d3++)
                    {
                        uint bit3 = 1u << (d3 - 1);
                        if ((regionCand & bit3) == 0)
                            continue;
                        uint tripleMask = bit1 | bit2 | bit3;
                        List<int> indices = new List<int>();
                        for (int i = 0; i < cellCands.Count; i++)
                        {
                            if ((cellCands[i] & tripleMask) != 0)
                                indices.Add(i);
                        }
                        if (indices.Count == 3)
                        {
                            foreach (int i in indices)
                            {
                                uint newMask = cellCands[i] & tripleMask;
                                if (newMask != cellCands[i])
                                {
                                    cellCands[i] = newMask;
                                    if (newMask == 0)
                                        return false;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
        } while (changed);

        // Finally, verify that each candidate digit appears in at least one cell.
        for (int d = 1; d <= sudokuSize; d++)
        {
            uint bit = 1u << (d - 1);
            if ((regionCand & bit) != 0)
            {
                bool found = false;
                foreach (uint mask in cellCands)
                {
                    if ((mask & bit) != 0)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }
        }
        return true;
    }
}



