#  Game Of Life Test

## Overview

Runs ten simulations of a "game of life" based on input

## Usage

There are three input options, all of which use Life 1.06 format. For example:

```text
#Life 1.06
0 1
1 1
1 -1
1 2
```

1. Manual input
   1. Put in one line of alive cell coordinates at a time, separating x and y with a space
2. File input
   1. Place a file in the TestCases folder. You can then run it by typing in the file name, i.e. `overflow.life`
3. Auto-input
   1. Runs a constant input baked into the code:
```text
#Life 1.06
0 1
1 2
2 0
2 1
2 2
-2000000000000 -2000000000000
-2000000000001 -2000000000001
-2000000000000 -2000000000001
```

