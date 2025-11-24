Advent of Code 2025
===================

Solutions for the 2025 edition of [Advent of Code](https://adventofcode.com/).

Gonna be more bleeding-edge .NET, methinks.


Templates
---------

The repo includes a couple of base templates in C# and F# for .NET 10, neither with any external dependencies. As usual the idea is to reuse common infrastructure code between solutions, only modifying functions in the puzzle file and adding more files when necessary. The setup has been further simplified from [last year's](https://github.com/lrc-se/aoc-2024) common code, with no Docker integration, but the run script remains.

Part selection has been moved from an environment variable to the first command line argument, which is recognized as follows:

- `1` or `part1`: only runs part one
- `2` or `part2`: only runs part two

Any other value will abort the execution. Consequently, input file selection has been shifted to the *second* command line argument.


### Run script

The templates also include a shell script *run.sh*, which will time the execution of the solution. It has the following syntax:

`run.sh part [mode] [testsuffix]`

- `part`: which part to run
- `mode`:
  - `test`: activates test mode
  - `rel`: builds the solution in release mode
  - `test-rel`: combines `test` and `rel`
- `testsuffix`: suffix to add to test input filename when in test mode

If the `mode` argument is omitted, the puzzle will be run in normal mode without release optimizations. Input is read from *input.txt* in normal mode, and from *input-testX.txt* in test mode where *X* is set to the value of `testsuffix`.

Puzzles
-------

TBA
