module Puzzle

type Node = { Name: string; Left: string; Right: string }
type Input = { LeftRight: char[]; Nodes: Map<string, Node> }

let private createNode (definition: string) =
    let parts = definition.Split(" = ")
    let dirParts = parts[1].Split(", ")
    { Name = parts[0];
      Left = dirParts[0][1..];
      Right = dirParts[1][..dirParts.Length] }

let private getSteps startNode isLastNodeName input =
    let rec loop curNode steps =
        if isLastNodeName curNode.Name then
            steps
        else
            let nextNodeName =
                match input.LeftRight[steps % input.LeftRight.Length] with
                | 'L' -> curNode.Left
                | _ -> curNode.Right

            loop input.Nodes[nextNodeName] (steps + 1)

    loop startNode 0 |> int64

[<TailCall>]
let rec private gcd (a: int64) (b: int64) = if b = 0 then a else gcd b (a % b)

let private lcm a b = a * b / gcd a b

let parseInput (rawInput: string) =
    let rec loop lines nodes =
        match lines with
        | [] -> nodes
        | line :: nextLines ->
            let node = createNode line
            loop nextLines (nodes |> Map.add node.Name node)

    let sections = rawInput.Split("\n\n")
    { LeftRight = sections[0].ToCharArray();
      Nodes = loop (sections[1].Split('\n') |> Array.toList) Map.empty }

let runPartOne (input: Input) = input |> getSteps input.Nodes["AAA"] ((=) "ZZZ")

let runPartTwo (input: Input) =
    input.Nodes.Values
    |> Seq.where _.Name.EndsWith('A')
    |> Seq.map (fun node -> getSteps node _.EndsWith('Z') input)
    |> Seq.reduce lcm
