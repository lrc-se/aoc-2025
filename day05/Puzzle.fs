module Puzzle

type Range = int64 * int64
type Input = { Ranges: Range array; Ingredients: int64 array }

let private createRange (line: string) =
    let parts = line.Split('-')
    int64 parts[0], int64 parts[1]

let private isInRange value range = value >= fst range && value <= snd range

let private count condition items =
    items
    |> Array.sumBy (fun item -> if condition item then 1L else 0)

let private countUnique ranges =
    let rec loop count maxValue curRanges =
        match curRanges with
        | [] -> count
        | range :: nextRanges ->
            let curMax = snd range
            if curMax <= maxValue then
                loop count maxValue nextRanges
            else
                let curCount =
                    if isInRange maxValue range then
                        curMax - maxValue
                    else
                        curMax - fst range + 1L

                loop (count + curCount) curMax nextRanges

    loop 0 0L (ranges |> Array.sort |> Array.toList)

let parseInput (rawInput: string) =
    let sections = rawInput.Split("\n\n") |> Array.map _.Split('\n')
    { Ranges = sections[0] |> Array.map createRange
      Ingredients = sections[1] |> Array.map int64 }

let runPartOne (input: Input) =
    input.Ingredients
    |> count (fun ingredient -> input.Ranges |> Array.exists (isInRange ingredient))

let runPartTwo (input: Input) = input.Ranges |> countUnique
