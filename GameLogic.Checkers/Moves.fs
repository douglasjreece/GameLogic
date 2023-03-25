namespace GameLogic.Checkers

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Moves =
    open Types

    let RegularMoves (moves: Move list) =
        moves |> List.map (fun m -> m.AsRegularMove) |> List.filter (fun m -> m.IsSome) |> List.map (fun m -> m.Value)

    let JumpMoves (moves: Move list) =
        moves |> List.map (fun m -> m.AsJumpMoves) |> List.filter (fun m -> m.IsSome) |> List.map (fun m -> m.Value)


