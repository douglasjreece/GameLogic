namespace GameLogic.Sorry

type Color = 
    | Red 
    | Blue 
    | Yellow 
    | Green
    member this.AsOrdinal =
        match this with
        | Red -> 0
        | Blue -> 1
        | Yellow -> 2
        | Green -> 3
    member this.Next =
        match this with
        | Red -> Blue
        | Blue -> Yellow
        | Yellow -> Green
        | Green -> Red
    member this.Previous =
        match this with
        | Red -> Green
        | Blue -> Red
        | Yellow -> Blue
        | Green -> Yellow
    
type Colors = Color list

type Zone = 
    | Start 
    | Board 
    | Safety 
    | Home

type SlideArea = 
    {
        StartSpot: int
        EndSpot: int
    }

type Player =
    {
        Number: int
        Color: Color
    }

type Players = Player list

type Location = 
    { 
        Color: Color
        Zone: Zone
        Spot: int 
    }

type Locations = Location list

type Pawn = 
    {   
        Color: Color
        Number: int 
    }

type Position = 
    {   
        Pawn: Pawn
        Location: Location 
    }

type Positions = Position list

type MoveType =
    | Regular
    | Swap
    | Slide
    | SideEffect
    member this.AsOrdinal =
        match this with
        | Regular -> 0
        | Swap -> 1
        | Slide -> 2
        | SideEffect -> 3

type Move = 
    {
        Pawn: Pawn
        From: Location
        To: Location
        Type: MoveType
    }

type Moves = Move list

type MovesList = Moves list

type Card =
    | Sorry
    | Number of int

type Cards = Card list

type GameParams = 
    { 
        ColorsInPlay: Colors
        PawnCount: int 
    }
    member this.PlayerCount = this.ColorsInPlay.Length

type CardDrawnState = 
    { 
        Player: Player
        Card: Card
        PotentialMoves: Moves list
    }

type StepState =
    | PlayerUp of Player
    | CardDrawn of CardDrawnState
    | ShuffleNeeded of Player
    | GameEnd
    member this.AsPlayerUp =
        match this with
        | PlayerUp playerUp -> playerUp
        | _ -> raise (System.InvalidCastException("StageState is not PlayerUp"))
    member this.AsCardDrawn =
        match this with
        | CardDrawn cardDrawn -> cardDrawn
        | _ -> raise (System.InvalidCastException("StageState is not CardDrawn"))
    member this.Player =
        match this with
        | PlayerUp x -> x
        | CardDrawn x -> x.Player
        | ShuffleNeeded x -> x
        | _ -> raise (System.InvalidCastException("Player is not valid"))

type GameState = 
    {
        Deck: Cards
        Positions: Positions
        Step: StepState
    }

type Game = 
    {
        Params: GameParams
        Players: Players
        State: GameState
    }
