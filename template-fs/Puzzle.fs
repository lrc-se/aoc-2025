module Puzzle

let parseInput (rawInput: string) = rawInput.Split('\n') |> Array.map int

let runPartOne (input: int[]) = input |> Array.sum

let runPartTwo (input: int[]) = input |> Array.reduce (*)
