open System
open Puzzle

let runner part =
    match part with
    | "1" | "part1" -> Ok(runPartOne)
    | "2" | "part2" -> Ok(runPartTwo)
    | unknown -> Error(unknown)

let args = Environment.GetCommandLineArgs()
match runner args[1] with
| Ok(runPuzzle) ->
    printfn $"Result: {(
        IO.File.ReadAllText(args[2]).TrimEnd()
        |> parseInput
        |> runPuzzle
    )}"
| Error(part) -> printfn $"Unknown part: '{part}'"
