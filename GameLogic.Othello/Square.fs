namespace GameLogic.Othello

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Square =
    open GameLogic.Library
    open Types 

    let NeighborY (y: int) (direction: int) =
        let index = List.findIndex ((=) y) Board.YRange
        Board.YRange |> List.elementOrNone (index + direction)

    let NeighborX (x: char) (direction: int) =
        let index = List.findIndex ((=) x) Board.XRange
        Board.XRange |> List.elementOrNone (index + direction)

    let Neighbor (direction: Direction * Direction) (square: Square) =
        let neighborX = 
            match fst direction with
            | Left -> NeighborX square.X -1
            | Right -> NeighborX square.X 1
            | _ -> Some square.X

        let neighborY = 
            match snd direction with
            | Up -> NeighborY square.Y -1
            | Down -> NeighborY square.Y 1
            | _ -> Some square.Y

        let result = match neighborX, neighborY with 
                        | Some nx, Some ny -> Some { X = nx; Y = ny }
                        | _ -> None
        result

    let IncX (x: char) = (NeighborX x 1).Value
    let DecX  (x: char) = (NeighborX x -1).Value
    let IncY (y: int) = (NeighborY y 1).Value
    let DecY (y: int) = (NeighborY y -1).Value
