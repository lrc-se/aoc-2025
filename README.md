Advent of Code 2025
===================

[Solutions](#puzzles) for the 2025 edition of [Advent of Code](https://adventofcode.com/).

Gonna be more bleeding-edge .NET, methinks. And some asm, because it's asm.


Templates
---------

The repo includes a couple of base templates in C# and F# for .NET 10, neither with any external dependencies, plus a special one integrating x64-asm with C# ([see below](#x64-assembly)). As usual the idea is to reuse common infrastructure code between solutions, only modifying functions in the puzzle file and adding more files when necessary. The setup has been further simplified from [last year's](https://github.com/lrc-se/aoc-2024) common code, with no Docker integration, but the run script remains.

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

### x64 assembly

For this year's added spice I got it into my head to investigate how one can interface between x64 assembly code and C# in the context of AoC, and it actually turned out to be surprisingly straightforward. To keep things from growing too complex I decided on a hybrid solution where input loading and parsing is done in managed code and only the actual algorithm is then offloaded to native code. The asm template is very simple and does the same thing as the other two templates, namely adds (part 1) and multiplies (part 2) the numbers parsed from the input.

The run script for the asm template includes build steps for assembling with NASM and linking with GoLink. Do note that these two tools need to be in the current path, and that this build chain is specific for the `win64` platform and needs to be adjusted for other platforms. The actual assembly code also uses the calling convention for `win64`, so any other implementation will have to update its register use to conform to a different calling convention. (So why a *bash* script under `win64` in the first place, then? Well, I tend to use Git Bash for all repo-related things, and AoC is no exception.)


Exercises
---------

The repo also includes a few exercises, one for each target language, consisting of puzzles from previous years for which I had existing solutions to start from:

### 2024: Day 2 (x64-asm)

Heavy use of registers for performance, together with some other optimization tricks. It could certainly be optimized further, especially in part 2, but I think it works well as a POC as it is; it's already extremely fast and further work would make very little practical difference seeing as how this is an early-day problem where execution times are bound to be very short anyway.

The same platform note as [above](#x64-assembly) applies, i.e. the implementation is specific to `win64`, the calling convention for which passes the arguments in `RCX` and `RDX` and does not consider `RSI` and `RDI` volatile.

### 2023: Day 8 (F#)

Fully functional and immutable, using built-in language features for brevity as much as I've been able.

### 2020: Day 23 (C#)

This is basically the direct opposite approach to the above, utilizing mutability as much as possible for performance, plus some extra optimizations.


Puzzles
-------

### Day 1 (C#)

Modular arithmetic right at the start? OK then!

### Day 2 (C#)

Just brute-forcing this for now, since the iteration count is rather low anyway. Some basic early bailout checks, though.

### Day 3 (C#)

Using a moving window for max values, with a simplified version in part 1.

### Day 4 (C#)

Using a hash set of coordinates to keep track of the rolls, as usual.

### Day 5 (F#)

Overlaps! After a couple of false starts in part 2 I settled on a linear traversal of ascending ranges, which is actually faster than part 1.

### Day 6 (C#)

I was a bit too trigger-happy with the input parsing in part 1, so I had to backtrack to a more generic approach for part 2. LINQ-heavy, with some GC-friendly performance enhancements. Oh, and the right-to-left direction doesn't actually matter.
