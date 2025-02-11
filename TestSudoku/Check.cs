
class Check
{


    /// <summary>
    /// test if a solved sudoku board is solved and fits the prev sudoku
    ///  
    /// 
    /// </summary>
    /// <param name="prevBoard"></param>
    /// <param name="solvedBoard"></param>
    /// <returns></returns>
    public static bool vaild(string prevBoard, Board solvedBoard)
    {
        
        // Checks if the length is equal
        if (prevBoard.Length != solvedBoard.board.Length)
        {
            return false;
        }


        // Checks if the boards are the same just solved
        if (!hasTheSameCharsAsStart(prevBoard, solvedBoard))
        {
            return false;
        }

        // Checks if this is a valid board
        return hasAllCharsInEveryArea(solvedBoard);
    }



    /// <summary>
    /// Checks if this has the same chars as the start
    /// </summary>
    /// <param name="prevBoard"></param>
    /// <param name="solvedBoard"></param>
    /// <returns></returns>
    public static bool hasTheSameCharsAsStart(string prevBoard , Board solvedBoard)
    {
        for (int i = 0; i < prevBoard.Length; i++)
        {
            if (prevBoard[i] == '0')
                continue;

            if (prevBoard[i] != solvedBoard.board[i] + '0')
            {
                return false;
            }

        }
        return true;

    }



    /// <summary>
    /// Checks if this is a soved sudoku 
    /// 
    /// by checking every box and and row and col
    /// to see if every value exsit and only once
    /// </summary>
    /// <param name="solvedBoard"></param>
    /// <returns></returns>
    public static bool hasAllCharsInEveryArea(Board solvedBoard)
    {
        int size = solvedBoard.size;
        int sqrtSize = solvedBoard.sqrtSize;
        
        uint fullMask = (1u << size) - 1;// Example: For 9x9, fullMask = 0x1FF (binary: 111111111)

        // Check all rows
        for (int row = 0; row < size; row++)
        {
            uint rowMask = 0;
            for (int col = 0; col < size; col++)
            {
                int value = solvedBoard.board[row * size + col];
                // Validate value range (1-size)
                if (value < 1 || value > size)
                    return false;

                uint bit = 1u << (value - 1);  // Convert number to bit position

                // Check for duplicates
                if ((rowMask & bit) != 0)
                    return false;
                rowMask |= bit;// Mark number as seen
            }
            // Verify all bits are lit
            if (rowMask != fullMask)
                return false;
        }

        // Check all columns
        for (int col = 0; col < size; col++)
        {
            uint colMask = 0;
            for (int row = 0; row < size; row++)
            {
                int value = solvedBoard.board[row * size + col];
                if (value < 1 || value > size)
                    return false;
                uint bit = 1u << (value - 1);
                if ((colMask & bit) != 0)
                    return false;
                colMask |= bit;
            }
            if (colMask != fullMask)
                return false;
        }

        // Check all box
        for (int BoxRow = 0; BoxRow < sqrtSize; BoxRow++)
        {
            for (int BoxCol = 0; BoxCol < sqrtSize; BoxCol++)
            {
                uint subgridMask = 0;
                for (int r = 0; r < sqrtSize; r++)
                {
                    for (int c = 0; c < sqrtSize; c++)
                    {
                        int actualRow = BoxRow * sqrtSize + r;
                        int actualCol = BoxCol * sqrtSize + c;
                        int value = solvedBoard.board[actualRow * size + actualCol];
                        if (value < 1 || value > size)
                            return false;
                        uint bit = 1u << (value - 1);
                        if ((subgridMask & bit) != 0)
                            return false;
                        subgridMask |= bit;
                    }
                }
                if (subgridMask != fullMask)
                    return false;
            }
        }

        return true;
    }


}
