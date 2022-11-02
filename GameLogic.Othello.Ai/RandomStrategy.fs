namespace GameLogic.Othello.Ai

module RandomStrategy =
    open GameLogic.Library
    open GameLogic.Othello.Types

    let Execute (seed: int option) (state: GameState): Square option =
        let potentialPlays = state.Step.AsPlayerUp.PotentialPlays
        match potentialPlays with
        | [] -> None
        | play::[] -> Some play.Square
        | _ -> Some (potentialPlays |> List.randomize seed |> List.head).Square
