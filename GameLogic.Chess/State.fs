namespace GameLogic.Chess

type State =
    {
        Positions: Position list
        Score: Move list
        PlayerTurn: Color
        PossibleMoves: Move list
    }
    member this.Winner = 
        match this.PossibleMoves with
        | [] -> Some this.PlayerTurn.Other
        | _ -> None

module State =
    let Initial =
        {
            Positions = Positions.initial
            Score = []
            PlayerTurn = Color.White
            PossibleMoves = Moves.MovesForPlayer Color.White [] Positions.initial
        }

    let ApplyMove (move:Move) (state:State) =
        let nextPositions = Moves.Apply move state.Positions
        let nextPlayer = state.PlayerTurn.Other
        let nextScore = state.Score @ [move]
        {
            Positions = nextPositions
            Score = nextScore
            PlayerTurn = nextPlayer
            PossibleMoves = Moves.MovesForPlayer nextPlayer nextScore nextPositions 
        }

    

