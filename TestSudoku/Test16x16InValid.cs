[TestClass]
public class Test16X16InValid
{
    [TestMethod]
    public void TestSolveInValidPuzzle1()
    {
        // Arrange
        string input = "00300008000<000@50000:0000?000300000000000000000000000010005600:000000500090000>00100000000000000000;0000@00040008000@0030000070;000000000000000000000;000?0003006000=00002040008000000000000000000050000900<0000020000000000000000<00000200060000;0000000000000";
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
        string input = ";0?0=>010690000000710000500:?0;4000000<0400070=005<3000800000000500@000:?80>10004<30>?8;00=20000>?8;270060000000000000900000000?0000?00000>0=000?3:0000>0026000000;>61029@0<00000100<0@00:40000800500:0?;>012600800?0;0000090<0@0;07000005<00?8:00003050:4080709";
        Board board = new Board();
        board.LoadPuzzle(input);
        Solver solver = new Solver(board);

        // Act
        bool solved = solver.Solve();


        // Assert

        Assert.IsFalse(solved);

    }


}