# The Function String #

the function string will be of a simple format as follows:

(cell being changed):(input)

for example, cell A1 is filled with a 23, the input string will read:

A1:23

likewise, if A1 is filled with =1+2:

A1:=1+2


# Reference Tables #

There will be two reference indexs. One that includes all of the cells that are referenced by other cells, and the other that contains all cells that reference other cells. Both of these lists will be kept as a two dimensional arraylist or vector data structure, the first dimension being the names of the cells and the second a list of references.

For example if A1 references B1, then:

Referenced Table

B1   |   A1

Referencing Table

A1   |   B1

If then A2 references B1:

Referenced Table

B1   |   A1 A2

Referencing Table

A1   |   B1

A2   |   B1