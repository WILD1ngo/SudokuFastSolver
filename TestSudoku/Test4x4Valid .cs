[TestClass]
public class Test4x4Valid
{
    [TestMethod]
    public void TestSolveValidPuzzle1()
    {
        // Arrange
        string input = "0321003004002100";
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
        string input = "1400000032104020";
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

        string input = "0010000402400421";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle4()
    {

        string input = "0300210000011043";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle5()
    {

        string input = "0102203040001020";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle6()
    {

        string input = "0132030100001020";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle7()
    {

        string input = "1400020000434020";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }





    [TestMethod]
    public void TestSolveValidPuzzle8()
    {

        string input = "0000430204300104";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle9()
    {

        string input = "2030102400000042";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }

    [TestMethod]
    public void TestSolveValidPuzzle10()
    {

        string input = "0014010004010042";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle11()
    {

        string input = "0010200002300321";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle12()
    {

        string input = "0213002004020300";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle13()
    {

        string input = "0203300000410402";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle14()
    {

        string input = "0243430000120000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    

    [TestMethod]
    public void TestSolveValidPuzzle15()
    {

        string input = "0004030014230001";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }
}