[TestClass]
public class Test9x9Invalid
{


    [TestMethod]
    public void TestSolveInValidPuzzle1()
    {
        // Arrange
        string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }




    [TestMethod]
    public void TestSolveInValidPuzzle2()
    {
        // Arrange
        string input = "100006709050000000009000008000090030000010000900600801002700000700800040800060107";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }


   
    [TestMethod]
    public void TestSolveInValidPuzzle3()
    {
        // Arrange
        string input = "100050209007100000060000000200000000000501002000020390300090001500010003000800040";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }



    [TestMethod]
    public void TestSolveInValidPuzzle4()
    {
        // Arrange
        string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }



    [TestMethod]
    public void TestSolveInValidPuzzle5()
    {
        // Arrange
        string input = "003000000400080037008000100040060073000900010000002000004070068600004000700000500";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }



    [TestMethod]
    public void TestSolveInValidPuzzle6()
    {
        // Arrange
        string input = "100006080000700000090050000000560030300000000000003801500001060000020400802005010";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }
    [TestMethod]
    public void TestSolveInValidPuzzle7()
    {
        // Arrange
        string input = "023000009400000100090030040200910004000007800900040002300090001060000000000500000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }
}