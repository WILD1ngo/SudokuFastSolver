# Sudoku Solver Omega (C#/.NET 8)
## by yoav Mateless

A high-performance Sudoku solver implementation in C# that solves puzzles of sizes 4x4, 9x9, 16x16, and 25x25 using backtracking search with  heuristics.


![alt text](https://sudoku-puzzles.net/wp-content/puzzles/butterfly-sudoku/easy/1.png)

## Features 

- Supports multiple grid sizes (4×4, 9×9, 16×16, 25×25)
- Clean console GUI app (like vim) 
- Enter sudoku via file
- Write solution to file
- Tests


## Solver Abilty
- Implements constraint propagation with:
  - **Naked Singles** elimination
  - **Hidden Singles** elimination
  - **Hidden sets** elinination
- Uses backtracking search with:
  - Least-constraining value ordering
  - Minimum-remaining-values heuristic
  - Forward checking via bitwise constraints
- Efficient bitmask-based candidate tracking




## Installation
   ```bash
   git clone https://github.com/yourusername/sudoku-solver.git
   cd sudoku-solver
   dotnet build
   dotnet run
   ```


## How To Use
**run the app:**
first you will be greated with  this pretty UI

**change the size:**
click tab to move the curser and select (using Enter) "change size" button
change to the size you want 

**load the board:**
you can insert the board in two ways

1. via the CLI just press the CLI and enter the string of chars for the board
2. from file **make sure that the file has the pazzle with the correct size in or it wont work** Enter the file path  make sure it show in the file at the left bottom 

**string:**
for example in 16 x 16 pazzles 
if i want to write 10 
i need to insert the tenth number after '1'
10 -> :
11 -> ;
as you can see in the follow ascii table
![](https://www.johndcook.com/ascii.png)

in the end:
you should get a string like this (16x16):
```0000000<0700000=30000>?00=05@6:0;>00719003?004<000000=20490000100?100000:0;00907@030065190700002=00>0<00000600508002003704000<>09000020;60100000?2<005030>009061000590002803000000>000000<0?3000000000100000:32000;0<300=00000?50603240070000@0001?0=0;@<:006870``` 

**Then Just Click on The solve button and wait for solution**
(note : if its a very hard 25x25 and you click two times on the enter you might get that the time is realy low because  you activate the solver again once its solved)
## How To Run The Tests

1.Open Test Explorer:
To open Test Explorer, choose Test > Test Explorer from the top menu bar (or press Ctrl + E, T).

Run your unit tests by clicking Run All (or press Ctrl + R, V).
![alt text](https://learn.microsoft.com/en-us/visualstudio/test/media/vs-2022/test-explorer-run-all.png?view=vs-2022)


After the tests have completed, a green check mark indicates that a test passed. A red "x" icon indicates that a test failed.

![](https://learn.microsoft.com/en-us/visualstudio/test/media/vs-2022/unit-test-passed.png?view=vs-2022)
## Performance 
| FPuzzle Size   |  Avg. Solve Time |
| ----------------- |----------------- |
| 4x4    |   	<1ms  |
| 9x9	| <10ms  |
| 16x16   |<100ms |
| 25x25	    |   <200ms  |

**How The Algorithm So Fast?**
Explantion in the following readme


**GUI**

has pretty nice gui like vim


## How i would improve the project further

1. Make 36x36 and 49x49 work (its pretty simple shouldnt take too long)
2. Add generator that creates sudokus 
3. improve the ui that player can play on the UI
4. clean the code more

**welcome to fork the project!!**






