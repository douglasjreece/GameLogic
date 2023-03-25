namespace GameLogic.Checkers

module GameState =
    open Types

    let Initial =
        {
            Positions = Positions.Initial
            PlayerUp = Color.Dark
            AvailableMoves = Positions.ValidMovesForColor Color.Dark Positions.Initial
            WinnerColor = None
        }

    let ApplyMove (move: Move) (state: GameState) =
        let nextPositions = Positions.ApplyMove move state.Positions
        let nextColor = state.PlayerUp.Other
        let nextMoves = Positions.ValidMovesForColor nextColor nextPositions
        let nextWinnerColor = if nextMoves.IsEmpty then Some state.PlayerUp else None
        {
            Positions = nextPositions
            PlayerUp =nextColor
            AvailableMoves = nextMoves
            WinnerColor = nextWinnerColor
        }