namespace GameLogic.Othello.Ai

module LookAheadStrategy =
    open GameLogic.Library
    open GameLogic.Othello.Types
    open GameLogic.Othello

    let Execute (lookahead: int) (state: GameState): Square option =
        let maxValue = 64

        let playerColor = state.Step.AsPlayerUp.Player.Color

        let rec playResult (lookahead': int) (state': GameState) (play: Square) =
            let resultState = GameState.ApplyPlay (Some play) state'
            let colorCount = Positions.ColorCount playerColor resultState.Positions
            if lookahead' = 1
                then colorCount
                else
                    match resultState.Step with
                    | PlayerUp playerUp ->
                        let lookahead'' = lookahead' - 1
                        let nextPlaysMax =
                            if not playerUp.PotentialPlays.IsEmpty
                                then 
                                    let nextPlaysColorCounts = playerUp.PotentialPlays |> Positions.Squares |> List.map (playResult lookahead'' resultState)
                                    (nextPlaysColorCounts |> List.sum) / nextPlaysColorCounts.Length
                                else
                                    colorCount
                        nextPlaysMax
                    | GameOver gameOver -> 
                        match gameOver.Winner with
                        | Some player when player.Color = playerColor -> maxValue
                        | _ -> 0

        let lookAheadPlayResult (play: Square) (state': GameState) =
            playResult lookahead state' play

        let selectedPlay (state': GameState) (plays: Square list) =
            let results = plays |> List.map (fun play -> lookAheadPlayResult play state', play)
            let groupedResults = results |> List.groupBy (fun result_play -> fst result_play) |> List.sortBy (fun group -> fst group) |> List.rev
            let bestResults = snd groupedResults.Head
            let bestResult = 
                match bestResults with
                | result::[] -> snd result
                | _ -> 
                    let result_square = bestResults |> List.randomize None |> List.head
                    snd result_square
            bestResult

        let potentialPlays = state.Step.AsPlayerUp.PotentialPlays
        let result = match potentialPlays with
                        | [] -> None
                        | play::[] -> Some play.Square
                        | _ -> Some (potentialPlays |> Positions.Squares |> selectedPlay state)
        result
