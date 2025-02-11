[TestClass]
public class Test9x9Valid
{
    [TestMethod]
    public void TestSolveValidPuzzle1()
    {
        // Arrange
        string input = "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert
        
        Assert.IsTrue(Check.vaild(input,board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle2()
    {
        // Arrange
        string input = "000000000000003085001020000000507000004000100090000000500000073002010000000040009";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);


        // Act
        bool solved = solver.Solve();


        // Assert
        Assert.IsTrue(Check.vaild(input, board));

    }

    [TestMethod]
    public void TestSolveValidPuzzle3()
    {

        string input = "000006000059000008200008000045000000003000000006003054000325006000000000000000000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle4()
    {

        string input = "100000002000010030005003400002001004000080700400900000001005040800000500900060000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle5()
    {

        string input = "000400000006080100700002004030500000009060800000007050002000016000020008980000200";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle6()
    {

        string input = "100006080050100200009000100090010000004500000600002008040000060700001800000300070";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle7()
    {

        string input = "000000080006080100700003004047005000500340000000000005300004007000000900010900060";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }





    [TestMethod]
    public void TestSolveValidPuzzle8()
    {

        string input = "800000000095000000067000000000954286000712943000638517000476005000521030000893400";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle9()
    {

        string input = "800000000095000000067000000000472968000913245000856137000090716000608492000000583";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }

    [TestMethod]
    public void TestSolveValidPuzzle10()
    {

        string input = "800000000059000000076000000000832765000476198000951243000090516000604982000000437";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle11()
    {

        string input = "800000000059000000067000000000030265000406198000000437000753916000891742000624583";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle12()
    {

        string input = "800000000059000000067000000000756238000418796000923415000571060000892107000634000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle13()
    {

        string input = "800000000059000000067000000000728563000691487000534912000976030000412708000853000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle14()
    {

        string input = "800000000095000000076000000000974286000512943000638517000791060000423708000856000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    

    [TestMethod]
    public void TestSolveValidPuzzle15()
    {

        string input = "800000000095000000076000000000624798000593142000718536000006417000070983000800265";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }
}