using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Heuristics
{



    /// <summary>
    /// applies all solving heuristics repeatedly until no more changes occur
    /// 
    /// 
    /// pretty simple function 
    /// </summary>

    public static bool ApplyHeuristics(Board board)
    {
        bool changed;
        do
        {
            changed = ProcessNakedSingles(board) | ProcessHiddenSingles(board);

            // Check for any empty cell with no possibility.
            // If so return that the board is unsolvable

            // probebly theres a better way to do it 
            // TODO : change this in the future
            foreach (Cell cell in board.emptyCells)
            {
                uint possible = board.rows[cell.Row] & board.cols[cell.Col] & board.boxes[cell.Box];
                if (possible == 0)
                    return false;
            }
        } while (changed);
        return true;
    }




    /// <summary>
    /// Processes cells where only one number is possible (Naked Singles)
    /// 
    /// the number in a cell can appear only once in
    /// each block, each row, and each column. 
    /// For each cell we can create a list of possible candidates that can appear in a cell. \
    /// We can remove from this list of candidates any number that appears in the same block, row, or column.
    /// For some cells, the list of possible candidates may have only one single number.
    /// This is called a "Naked Single", 
    /// and this number should go into the cell
    /// 
    /// 
    /// 
    /// So what the algorithem does?
    /// 
    /// it runs on every cell and checks if the board has one single bit lit 
    /// because bitwise
    /// </summary>
    /// <return> if changed something <return>
    private static bool ProcessNakedSingles(Board board)
    {
        bool changed = false;
        for (int i = board.emptyCells.Count - 1; i >= 0; i--)
        {
            Cell cell = board.emptyCells[i];
            uint possible = board.rows[cell.Row] & board.cols[cell.Col] & board.boxes[cell.Box];
            if (BitOperations.PopCount(possible) == 1)
            {
                uint value = possible;
                int num = BitOperations.TrailingZeroCount(value) + 1;
                board.board[cell.Row * board.size + cell.Col] = num;
                board.rows[cell.Row] ^= value;
                board.cols[cell.Col] ^= value;
                board.boxes[cell.Box] ^= value;
                board.emptyCells.RemoveAt(i);
                changed = true;
            }
        }
        return changed;
    }



    /// <summary>
    /// For example in a 9x9:
    /// Select a 3x3 block and a digit. 
    /// Check how many cells in that block can contain the selected digit. 
    /// If there is only one such cell, you can fill that cell with the selected digit. 
    /// The same logic is acceptible for rows and columns.
    /// 
    /// 
    /// 
    /// How Hidden Single is different from Naked Single?
    /// 
    /// 
    /// Hidden Single is when there is only one possible cell in a row, column or block, where the digit can be placed.
    /// 
    /// Naked Single is when there is only one allowed digit in a cell
    /// 
    /// 
    /// 
    /// So what the algorithem does?
    /// 
    /// For each empty cell in the region:
    /// 1. Calculates combined constraints(row & col & box)
    /// 2. Checks if current number is possible
    /// 3. Counts how many cells can accept the number
    /// 4. Remembers the last valid cell
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    private static bool ProcessHiddenSingles(Board board)
    {
        bool changed = false;
        // Process rows.
        for (int row = 0; row < board.size; row++)
        {
            for (int num = 1; num <= board.size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((board.rows[row] & mask) == 0)
                    continue;

                int count = 0;
                Cell hiddenCell = new Cell(-1, -1, -1);
                for (int col = 0; col < board.size; col++)
                {
                    int index = row * board.size + col;
                    if (board.board[index] != 0)
                        continue;
                    int box = (row / board.sqrtSize) * board.sqrtSize + (col / board.sqrtSize);
                    uint possible = board.rows[row] & board.cols[col] & board.boxes[box];
                    if ((possible & mask) != 0)
                    {
                        count++;
                        hiddenCell = new Cell(row, col, box);
                    }
                }
                if (count == 1)
                {
                    int index = hiddenCell.Row * board.size + hiddenCell.Col;
                    if (board.board[index] == 0)
                    {
                        board.board[index] = num;
                        board.rows[hiddenCell.Row] ^= mask;
                        board.cols[hiddenCell.Col] ^= mask;
                        board.boxes[hiddenCell.Box] ^= mask;
                        RemoveEmptyCell(board.emptyCells, hiddenCell);
                        changed = true;
                    }
                }
            }
        }

        // Process columns.
        for (int col = 0; col < board.size; col++)
        {
            for (int num = 1; num <= board.size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((board.cols[col] & mask) == 0)
                    continue;

                int count = 0;
                Cell hiddenCell = new Cell(-1, -1, -1);
                for (int row = 0; row < board.size; row++)
                {
                    int index = row * board.size + col;
                    if (board.board[index] != 0)
                        continue;
                    int box = (row / board.sqrtSize) * board.sqrtSize + (col / board.sqrtSize);
                    uint possible = board.rows[row] & board.cols[col] & board.boxes[box];
                    if ((possible & mask) != 0)
                    {
                        count++;
                        hiddenCell = new Cell(row, col, box);
                    }
                }
                if (count == 1)
                {
                    int index = hiddenCell.Row * board.size + hiddenCell.Col;
                    if (board.board[index] == 0)
                    {
                        board.board[index] = num;
                        board.rows[hiddenCell.Row] ^= mask;
                        board.cols[hiddenCell.Col] ^= mask;
                        board.boxes[hiddenCell.Box] ^= mask;
                        RemoveEmptyCell(board.emptyCells, hiddenCell);
                        changed = true;
                    }
                }
            }
        }

        // Process boxes.
        for (int b = 0; b < board.size; b++)
        {
            BoxInfo boxInfo = board.boxInfos[b];
            for (int num = 1; num <= board.size; num++)
            {
                uint mask = 1u << (num - 1);
                if ((board.boxes[b] & mask) == 0)
                    continue;

                int count = 0;
                Cell hiddenCell = new Cell(-1, -1, -1);
                for (int r = boxInfo.StartRow; r <= boxInfo.EndRow; r++)
                {
                    for (int c = boxInfo.StartCol; c <= boxInfo.EndCol; c++)
                    {
                        int index = r * board.size + c;
                        if (board.board[index] != 0)
                            continue;
                        uint possible = board.rows[r] & board.cols[c] & board.boxes[b];
                        if ((possible & mask) != 0)
                        {
                            count++;
                            hiddenCell = new Cell(r, c, b);
                        }
                    }
                }
                if (count == 1)
                {
                    int index = hiddenCell.Row * board.size + hiddenCell.Col;
                    if (board.board[index] == 0)
                    {
                        board.board[index] = num;
                        board.rows[hiddenCell.Row] ^= mask;
                        board.cols[hiddenCell.Col] ^= mask;
                        board.boxes[hiddenCell.Box] ^= mask;
                        RemoveEmptyCell(board.emptyCells, hiddenCell);
                        changed = true;
                    }
                }
            }
        }
        return changed;
    }






    /// <summary>
    /// Method to remove a cell from the empty cells list
    /// 
    /// Mainly used as an helper methed
    /// </summary>
    private static void RemoveEmptyCell(List<Cell> emptyCells, Cell cell)
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
}