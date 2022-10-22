namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Pawn =
    let Create (color: Color) (number: int): Pawn = { Color = color; Number = number }

