namespace GameLogic.Othello

module Types =
    type Direction =
        | Left
        | Right
        | Up
        | Down
        | Neither
        static member All = [Left; Right; Up; Down; Neither]

    type Square = 
            {
                X: char
                Y: int
            }
            static member At (x: char) (y: int) = { X = x; Y = y }

    type Squares = Square list

    type Color = 
            | Black
            | White
            member this.Next =
                match this with
                | Black -> White
                | White -> Black

    type Player =
        {
            Color: Color
        }
        static member Of (color: Color) = { Color = color }

    type Position =
        {
            Square: Square
            Color: Color
        }
        static member Of (square: Square) (color: Color) = { Square = square; Color = color }

    type Positions = Position list

    type PlayerStep =
        {
            Player: Player
            PotentialPlays: Positions
        }
        static member Of (player: Player) (potentialPlays: Positions) = { Player = player; PotentialPlays = potentialPlays }

    type GameOverStep =
        {
            Winner: Player option
        }
        static member Of (winner: Player option) = { Winner = winner }

    type Step =
        | PlayerUp of PlayerStep
        | GameOver of GameOverStep
        member this.AsPlayerUp = 
            match this with
            | PlayerUp step -> step
            | _ -> failwith "Step is not PlayerUp"
        member this.AsGameOver =
            match this with
            | GameOver step -> step
            | _ -> failwith "Step is not GameOver"

    type GameState =
        {   
            Positions: Positions
            Step: Step
        }
        static member Of (positions: Positions) (step: Step) = { Positions = positions; Step = step }

    type Game = 
        {
            State: GameState
        }