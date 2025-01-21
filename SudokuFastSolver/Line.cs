

public struct Line
{
    public Boolean[] Contains;
    // Constructor that takes the size and values from the user
    public Line(int size)
    {
        Contains = new Boolean[size];  
    }

    // Method to set a value to true
    public void Set(int num)
    {
        if (num == 0)
            return;


        Contains[num - 1] = true;
    }
    public void Set(int num, int prev)
    {
        if (num == 0)
            Contains[prev - 1] = false;
        else
          Contains[num - 1] = true;
    }
    public void Print()
    {
        Console.WriteLine("Contains array values:");
        for (int i = 0; i < Contains.Length; i++)
        {
            Console.WriteLine($"Contains[{i}] = {Contains[i]}");
        }
    }
}
