namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Player =
    let Create (number: int) (color: Color) = { Player.Number = number; Player.Color = color }

