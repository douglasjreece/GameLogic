namespace GameLogic.Othello


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Squares =
    open GameLogic.Library
    open Types

    let All =
        Board.XRange |> List.cartesian Board.YRange |> List.map (fun row_column -> { Y = fst row_column; X = snd row_column })

    let Sort (squares: Square list) = squares |> List.sortBy (fun square -> square.Y, square.X)
