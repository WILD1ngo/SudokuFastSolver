using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;

struct Tile
{

    private BitArray candidates;
    public Tile(int size)
    {
        candidates = new BitArray(size + 1, true);
    }

     public void SetCandidate(int number)
     {
        if (number > 0)
            candidates[number] = true;
     }

    public void RemoveCandidate(int number)
    {
        if (number > 0)
            candidates[number] = false;
    }

    public bool HasCandidate(int number)
    {
        return number > 0 && candidates[number];
    }

}

