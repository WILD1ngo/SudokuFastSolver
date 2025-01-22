using Xunit;

public class BoardTests
{
    [Fact]
    public void CreateBoard_CreatesBoardWithCorrectSettings()
    {
        // Arrange
        var settings = new Settings(
            gridSize: 9,
            showEmptySquare: false,
            emptyCell: "0",
            showGridLines: true
        );

        // Initialize the board with a puzzle string
        var board = new Board(settings, false, "300090002020104000000300700603500080870000014010007605002001000000905020900030006");

        // Initialize the solver with the board
        var solver = new Solver(board);

        // Add debugging output
        Console.WriteLine("Solver solved: " + solver.solved);

        // Assert: Verify that the puzzle is solved
        Assert.Equal<bool>(solver.solved, true);    
    }
}
