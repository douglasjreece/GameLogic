namespace GameLogic.Checkers

module Types =
    type Color = 
        | Dark
        | Light
        member this.Other =
            match this with
            | Dark -> Light
            | Light -> Dark

    type Piece =
        | Man
        | King

    type Direction = 
        | Increment
        | Decrement
        member this.AsInt =
            match this with
            | Increment -> 1
            | Decrement -> -1
        member this.Other =
            match this with
            | Increment -> Decrement
            | Decrement -> Increment

    type Location = 
        {
            Number: int
        }
        member this.ToGrid: Grid =
            let row = (this.Number - 1) / 4
            let colofs = if row % 2 = 1 then 0 else 1
            { 
                Grid.Row = row; 
                Grid.Column = ((this.Number - 1) % 4) * 2 + colofs  
            }
        static member At (number: int) = { Number = number }
    and Grid = 
        {
            Row: int
            Column: int
        }
        member this.Next (dRow: int) (dCol: int) =
            {
                Row = this.Row + dRow
                Column = this.Column + dCol
            }
        member this.IsValid = 
            this.Row >= 0 && 
            this.Row <= 7 && 
            this.Column >= 0 && 
            this.Column <= 7 &&
            this.Row % 2 <> this.Column % 2

        
        member this.ToLocation: Location =
            {
                Location.Number = (this.Column + 1) / 2 + (this.Row * 4) + (this.Row % 2)
            }

    type Position =
        {
            Piece: Piece
            Color: Color
            Location: Location
        }
        member this.Directions =
            match this.Piece, this.Color with
            | Piece.Man, Color.Dark -> [Direction.Increment]
            | Piece.Man, Color.Light -> [Direction.Decrement]
            | _-> [Direction.Increment; Direction.Decrement]
            

    type RegularMove =
        {
            From: Location
            To: Location
        }

    type JumpMove =
        {
            From: Location
            To: Location
            Jumped: Location
        }

    type Move =
        | RegularMove of RegularMove
        | JumpMoves of JumpMove list
        member this.AsRegularMove = 
            match this with
            | RegularMove move -> Some move
            | _ -> None
        member this.AsJumpMoves = 
            match this with
            | JumpMoves moves -> Some moves
            | _ -> None

    type GameState =
        {
            Positions: Position list
            PlayerUp: Color
            AvailableMoves: Move list
            WinnerColor: Color option
        }
