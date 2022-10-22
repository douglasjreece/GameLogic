namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Move =
    let Create (pawn: Pawn) (from: Location) (to_: Location) = { Pawn = pawn; From = from; To = to_; Type = MoveType.Regular }

    let CreateSwap (pawn: Pawn) (from: Location) (to_: Location) = { Pawn = pawn; From = from; To = to_; Type = MoveType.Swap }

    let CreateSlide (pawn: Pawn) (from: Location) (to_: Location) = { Pawn = pawn; From = from; To = to_; Type = MoveType.Slide }

    let CreateSideEffect (pawn: Pawn) (from: Location) (to_: Location) = { Pawn = pawn; From = from; To = to_; Type = MoveType.SideEffect }

    let CreateStartExit (pawn: Pawn) = 
        let location1 = Location.Start pawn.Color pawn.Number
        let location2 = Location.ExitStart pawn.Color
        Create pawn location1 location2

    let NextDirection (count: int) (move: Move) =
        { move with To = Location.DirectionN move.Pawn.Color move.Pawn.Number count move.To}

    let Sort (moves: Move list) = moves |> List.sortBy (fun move -> move.Pawn.Color.AsOrdinal, move.Type.AsOrdinal, move.From.Spot)