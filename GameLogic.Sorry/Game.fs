namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Game =
    let Create (gameParams: GameParams): Game = 
        let players = (Players.ListFromColors gameParams.ColorsInPlay)
        { 
            Params = gameParams
            Players = players
            State = GameState.Create players gameParams.PawnCount
        }

    let NextStep (game: Game) (moves: Move list option) =
        let nextState = 
            match game.State.Step with
            | PlayerUp _ -> GameState.DrawCard game.State
            | CardDrawn _ -> GameState.ApplyMoves game.Players game.Params.PawnCount moves game.State
            | ShuffleNeeded _ -> GameState.ShuffleDeck game.State
            | _ -> raise (System.InvalidOperationException("The game is over."))
        { game with State = nextState }