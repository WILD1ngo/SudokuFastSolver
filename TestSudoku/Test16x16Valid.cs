[TestClass]
public class Test16X16Valid
{
    [TestMethod]
    public void TestSolveValidPuzzle1()
    {
        // Arrange
        string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle2()
    {
        // Arrange
        string input = "<000509000000062000000005000?400000000=000000:000000<06070008000700000000050403050004000000000000800000000600?00000005>;04000000:;000000608000000000000000000000609>70000000004042000009000006>00020000@000008?0?0000:000@0000000001000000020000000010000=000000";
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

        string input = "030000;062<000@0001;:620@80>700400020@8900035=109>0030000;0=0000=000620080003?05:00008000030000<0@000450;<0000090?4500000900>@0004000<:10>600870000000060008040=0200070050?010<0@873050?0:000200;<060000300000=020>070?0000500:60000501000;020000001006;00098030";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle4()
    {

        string input = "=06100900200500@<0000100000000?0;0000@02=000340889>0000040@00<:00?0@000=15082;93069070@;3:000080020=100>000;00<5:000<020>@40?000000?0;18200000000405:0070<=@800?0080000<00700042900>25000?3007=00=23000000>:@00048:000>0005?;0360>57;0?0000000000000000000007020";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle5()
    {

        string input = "0>@03?;0040860070000:0>200<000?000620095:0=0000800=00060?300000<00?070<0@0500>4:=:;7000>1<30?006><30=04000000002080000:0000=005;000015049?;7800>0000>0000=02000070:000?0<>00000900060<0980000?;0?60<5;0009000080;04>00=?70:0000001200800;00@00900009000:30050<70";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle6()
    {

        string input = ";07805>0300:=0<@004007000<0?:002:0000000000437000050009?000000080400;8000>07930000>37<0000?=;0049?07005=03000080500=0?3024:8<0@008000000;0000@=500=000000800?0>000:090?0067000;<000<0:;0000>72407:090;1000500630=0<>?070:00008000000>0:00=0050000635@9<00;00>000";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



    [TestMethod]
    public void TestSolveValidPuzzle7()
    {

        string input = "0360000400020<;0<?00000@00100=00200>=78000@;0?:0=0006;9?3000000004250009000<0000600;>8:04070?0000=000<0005080000008<70?;9=0004>0:0020000;84756?000=000061>?00200>5000320609:000<00000:00<0000890?05900100000000;;<710?3>2400008040:0000=0900000>000=0000003><:0?";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }


    [TestMethod]
    public void TestSolveValidPuzzle8()
    {

        string input = "00;7030008140@0010:3008=0207004;0<020090?0000008?000:>00;060001000796?000<0:40=0=005<0000036000006@000:0105>00000000;=390000<0@090>0050004@0086:0?4000600782>;0002000800300170?05300?900>00<0100000<>00000030:;00030=000002090<00>0?060@00000000002;000<00?00070";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);
        bool solved = solver.Solve();
        Assert.IsTrue(Check.vaild(input, board));

    }



}