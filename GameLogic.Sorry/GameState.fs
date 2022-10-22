namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module GameState =
    let Create (players: Players) (pawnCount: int): GameState = 
        { 
            Deck = Cards.Shuffled None
            Step = PlayerUp players.Head
            Positions = Positions.Initial players pawnCount
        }

    let DrawCard (state: GameState): GameState =
        let cardDrawn = state.Deck.Head
        let nextDeck = state.Deck.Tail
        let player = state.Step.AsPlayerUp
        let nextStep = CardDrawn (CardDrawnState.Create player cardDrawn state.Positions)
        { state with Deck = nextDeck; Step = nextStep }
    
    let ApplyMoves (players: Players) (pawnCount: int) (moves: Move list option) (state: GameState): GameState =
        let card = state.Step.AsCardDrawn.Card
        let player = state.Step.AsCardDrawn.Player
        let nextPositions = 
            match moves with
            | Some (moves')-> Positions.ApplyMoves moves' state.Positions
            | _ -> state.Positions
        let pawnsInHome = nextPositions |> Positions.ForPawnColor player.Color |> Positions.InHome
        let nextStep = 
            let nextPlayer = 
                match card with
                | Number number when number = 2 -> player
                | _ -> player |> Players.Next players
            match pawnsInHome.Length with
            | length when length = pawnCount -> GameEnd
            | _ ->
                match state.Deck.Length with
                | 0 -> ShuffleNeeded nextPlayer
                | _ -> PlayerUp nextPlayer
        { Deck = state.Deck; Positions = nextPositions; Step = nextStep}

    let ShuffleDeck (state: GameState): GameState = 
        { state with Deck = Cards.Shuffled None; Step = PlayerUp state.Step.Player  }
