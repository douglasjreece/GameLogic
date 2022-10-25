namespace GameLogic.Othello

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Game =
    open Types

    let Initial =
        {
            State = GameState.Initial
        }

    let NextStep (game: Game) (play: Position option) =
        let nextState = GameState.ApplyPlay play game.State
        { 
            State = nextState 
        }