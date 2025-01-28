public class Cell
{
    public int Value { get; set; }
    public List<int> Possibilities { get; set; } = new List<int>();
    public int Row { get; set; }
    public int Col { get; set; }
}