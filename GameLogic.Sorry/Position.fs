namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Position =
    let Create (pawn: Pawn) (location: Location): Position = { Pawn= pawn; Location = location }

    let CreateStartExit (pawn: Pawn) = (Create pawn (Location.ExitStart pawn.Color))

    let IsZone (zone: Zone) (position: Position) = Location.IsZone zone position.Location

    let IsPawnColor (color: Color) (position: Position) = position.Pawn.Color = color

    let NotIsPawnColor (color: Color) (position: Position) = position.Pawn.Color <> color

    let Increment (count: int) (position: Position) = Create position.Pawn (Location.ForwardN position.Pawn.Color position.Pawn.Number count position.Location)

    let Decrement (count: int) (position: Position) = Create position.Pawn (Location.BackwardN count position.Location)

    let Location (position: Position) = position.Location

        