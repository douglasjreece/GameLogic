namespace GameLogic.Othello

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module GameState =
    open Types

    let Initial =
        {
            Positions = Positions.InitialSetup
            Step = PlayerUp
                    {
                        Player = { Color = Color.Black }
                        PotentialPlays = Positions.PotentialPlays Positions.InitialSetup Color.Black
                    }
        }

    let ApplyPlay (position: Position option) (state: GameState) =
        let player = state.Step.AsPlayerUp.Player
        let nextPositions = 
            match position with
            | Some (position') -> 
                let affectedSquares = Directions.All |> List.collect (fun direction -> Positions.EnclosingSquares position' direction state.Positions)
                let result = position' :: state.Positions
                let result = result |> Positions.ToColor player.Color affectedSquares |> List.distinct |> List.sortBy (fun position -> position.Square.Y, position.Square.X)
                result
            | None -> state.Positions

        let nextPotentialPlays = Positions.PotentialPlays nextPositions player.Color.Next
        let nextPlayerCanPlay = not nextPotentialPlays.IsEmpty
        let thisPotentialPlays = Positions.PotentialPlays nextPositions player.Color
        let thisPlayerCanPlay = not thisPotentialPlays.IsEmpty
        let gameOver = not (nextPlayerCanPlay || thisPlayerCanPlay)
 
        let result = 
            match gameOver with
            | false -> 
                let nextColor = if nextPlayerCanPlay then player.Color.Next else player.Color
                {
                    Positions = nextPositions
                    Step = PlayerUp (PlayerStep.Of (Player.Of nextColor) nextPotentialPlays)
                }
            | true ->
                let blackCount = Positions.ColorCount Color.Black nextPositions
                let whiteCount = Positions.ColorCount Color.White nextPositions
                let winner = 
                    match blackCount, whiteCount with
                    | (b,w) when b > w -> Some (Player.Of Color.Black)
                    | (b,w) when b < w -> Some (Player.Of Color.White)
                    | _ -> None
                GameState.Of nextPositions (Step.GameOver (GameOverStep.Of winner))
        result


