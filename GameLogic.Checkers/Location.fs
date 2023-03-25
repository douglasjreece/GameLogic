namespace GameLogic.Checkers

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Location =
    open Types

    let NextMoveLocations (location: Location) (direction: Direction) =
        let grid = location.ToGrid
        let nextLeft = grid.Next direction.AsInt -1
        let nextRight = grid.Next direction.AsInt 1
        let result = 
            match nextLeft.IsValid, nextRight.IsValid with
            | true, true -> [nextLeft; nextRight]
            | true, false -> [nextLeft]
            | false, true -> [nextRight]
            | _ -> []
        result |> List.map (fun g -> g.ToLocation)

    let NextJumpLocations (location: Location) (direction: Direction) =
        let grid = location.ToGrid
        let nextLeft = grid.Next (direction.AsInt * 2) -2
        let nextRight = grid.Next (direction.AsInt * 2) 2
        let result = 
            match nextLeft.IsValid, nextRight.IsValid with
            | true, true -> [nextLeft; nextRight]
            | true, false -> [nextLeft]
            | false, true -> [nextRight]
            | _ -> []
        result |> List.map (fun g -> g.ToLocation)

    let JumpedLocation (jumpFrom: Location) (jumpTo: Location) =
        let fromGrid = jumpFrom.ToGrid
        let toGrid = jumpTo.ToGrid
        let jumpRow = if toGrid.Row > fromGrid.Row then fromGrid.Row + 1 else fromGrid.Row - 1
        let jumpColumn = if toGrid.Column > fromGrid.Column then fromGrid.Column + 1 else fromGrid.Column - 1
        let jumpedGrid = { Grid.Row = jumpRow; Grid.Column = jumpColumn }
        jumpedGrid.ToLocation