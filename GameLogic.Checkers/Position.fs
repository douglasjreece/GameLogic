namespace GameLogic.Checkers

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Position =
    open Types

    let Initial (color: Color) (location: Location): Position =
        {
            Color = color
            Piece = Piece.Man
            Location = location
        }

