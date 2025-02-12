## Algorithm


**backtracking** 

## first : 
inital check and activation of heavy heuristics 
(in this level for every cell saved the allowed digits to enter for every 
this way you cab activate **hidden sets** (hidden subsets) and **naked sets** (naked subset)



**bitwise**
how the board is being saved
you save at the start for every cells the possible values for it 

example:
![](https://www.sudokuoftheday.com/image.svg?sg=4(15)(35)27(39)6(189)(58)798156234(16)2(356)84(39)(15)(19)7237468951849531726561792843(36)82(36)15479(169)7(56)(69)243(168)(58)W(1369)W(15)W4W(369)W8W7W(15)W(16)W2)
here you can see on the bottom left squre the possible values are 1 , 3 , 6 , 9
this is being saved as **byte 100100101**
because the first bit it lit means that you can insert one ... 

this is realy nice for the following heuristics but this has some problems

you to insert value you need to remove it from the row col and box

that why we are going to change the way we solve the sudoku mid solve
**Advantages**
all havey heuristics are possible in this way 




**Hidden Sets**
(probbebly could find better definition in the web i copyed this for a website that explains this)
All Hidden Subsets work the same way, the only thing that changes is the number of cells and candidates affected by the move. Take Hidden Pair: If you can find two cells within a house such as that two candidates appear nowhere outside those cells in that house, those two candidates must be placed in the two cells. All other candidates can therefore be eliminated.
![](https://hodoku.sourceforge.net/examples/h201.png)
![](https://hodoku.sourceforge.net/examples/h202.png)

Take a look at column 9 in the left example: The candidates 1 and 9 appear only in cells (r5c9) and (r7c9) (often abbreviated as r57c9) in that column (they appear elsewhere in row 5, row 7, block 6 and block 9, but that is not important here). One of those two cells has to be 1 and the other 9. We don't know yet which is which, but what we know is, that r5c9 can't possibly be 6.
 
 **Naked sets**
 (probbebly could find better definition in the web i copyed this for a website that explains this)
 works by looking for candidates that can be removed from other cells. Naked Pairs are when there are just two candidates being looked for, Naked Triple when there are three, and Naked Quads when there are four.


![](https://www.sudokuoftheday.com/image.svg?sg=4(15)(35)27(39)6(189)(58)798156234(16)2(356)84(39)(15)(19)7237468951849531726561792843(36)82(36)15479(169)7(56)(69)243(168)(58)W(1369)W(15)W4W(369)W8W7W(15)W(16)W2)


## Second :
we change the way we store the board 
and solve only with 
**Naked Singles** 
**Hidden Singles**
**Least-constraining value ordering**
**Minimum-remaining-values heuristic**


**how we store the board now?**
we only save the possible values for rows cols and box then to check if value is possible to insert 

## TODO : continue explantion
