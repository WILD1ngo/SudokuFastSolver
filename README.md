# Sudoku Solver Omega
## by yoav Mateless

A high-performance Sudoku solver implementation in C# that solves puzzles of sizes 4x4, 9x9, 16x16, and 25x25 using constraint propagation and backtracking search with  heuristics.

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




Installation
**Clone repository**:
   ```bash
   git clone https://github.com/yourusername/sudoku-solver.git
   open solution file via visual studio 2022
   ```


## Performance 
``` table 2x5
Puzzle Size   | 	Avg. Solve Time
------------------------------------
4x4           | 	<1ms
9x9	          |     <10ms
16x16         | 	<100ms
25x25	      |     <200ms
```

**HOW THE SOLVER WORKS?**

TODO : enter explanation

**GUI**

has pretty nice gui like vim

TODO : add some photos




