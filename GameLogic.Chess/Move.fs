namespace GameLogic.Chess


type CastleMove =
    {
        RookFrom: Position
        RookTo: Position
        PassThroughLocations: Location list
    }
    

type PromotionMove =
    {
        To: Type
    }
    static member Make (piece:Type) = { To = piece }

type MoveExtension =
    | Castle of CastleMove
    | Promotion of PromotionMove
    
type CastleOrientation =
    {
        RookStart: Location
        RookEnd: Location
        KingEnd: Location
        MidSquares: Location list
    }

type Move =
    {
        From: Position
        To: Position
        Takes: Position option
        Extension: MoveExtension option
    }
    static member Make (from:Position) (to_:Position) (takes:Position option) = 
        { From = from; To = to_; Takes = takes; Extension = None }
    static member MakeCastle (king:Position) (castle:CastleOrientation) =
        let rookPosition (location:Location) = Position.Make((king.Color, Type.Rook), location)
        { 
            From = king 
            To = {king with Location = castle.KingEnd}
            Takes = None
            Extension = Some (Castle(   {
                                            RookFrom = rookPosition castle.RookStart; 
                                            RookTo = rookPosition castle.RookEnd; 
                                            PassThroughLocations = castle.MidSquares
                                        }))
        }
    member this.AddTakes (take:Position) =
        {this with Takes = Some take}
    member this.AddExtension (extension:MoveExtension) =
        { this with Extension = Some extension }
    member this.HasCastle =
        if this.Extension.IsSome then
            match this.Extension.Value with
            | Castle _ -> true
            | _ -> false
        else false
    member this.AsCastle =
        match this.Extension.Value with
        | Castle x -> x
        | _ -> raise (System.InvalidOperationException("Move is not Castle Move"))
    

type Orientation =
    {
        Forward: int
        Backward: int
        Left: int
        Right: int
        Straight: int
        FirstRank: int
        SecondRank: int
        EighthRank: int
        OpponentSecondRank: int
        LeftCastle: CastleOrientation
        RightCastle: CastleOrientation
    }

type LocationAndTakePosition =
    {
        Location: Location
        TakePosition: Position option
    }
    static member Make (location:Location) (takePosition:Position option) = { Location = location; TakePosition = takePosition }

module Moves =
    open GameLogic.Library

    let Apply (move:Move) (positions:Position list): Position list =
        let KeepIfNotMove (position:Position) = position.Location <> move.From.Location && position.Location <> move.To.Location
        let positionsWithoutMove = positions |> List.filter KeepIfNotMove
        let newPositions = move.To :: positionsWithoutMove
        newPositions

    let LastMove (moves:Move list) = moves |> List.lastOrNone

    let Move' (from:Position, to_:Position) = Move.Make from to_ None
    let TakesMove' (from:Position, to_:Position, take:Position option) = Move.Make from to_ take
    let CastleMove' (king:Position, castle:CastleOrientation) = Move.MakeCastle king castle

    let InCheck (moves:Move list): bool =
        moves |> List.any (fun move -> move.Takes.IsSome && move.Takes.Value.Type = Type.King)

    let PositionsContainLocation (location:Location) (positions:Position list) = 
        (positions |> List.tryFind(fun position -> position.Location = location)).IsSome

    let MovesContainFromLocation (location:Location) (moves:Move list) =
        (moves |> List.tryFind(fun move -> move.From.Location = location)).IsSome

    let OrientationFor (color:Color): Orientation =
        if color = Color.White
            then 
                {
                    Forward = 1
                    Backward = -1
                    Left = -1
                    Right = 1
                    Straight = 0
                    FirstRank = 1
                    SecondRank = 2
                    EighthRank = 8
                    OpponentSecondRank = 7
                    LeftCastle = 
                        {
                            RookStart = ('a', 1)
                            RookEnd = ('d', 1)
                            KingEnd = ('c', 1)
                            MidSquares = [('b', 1);('c', 1);('d', 1)]
                        }
                    RightCastle = 
                        {
                            RookStart = ('h', 1)
                            RookEnd = ('f', 1)
                            KingEnd = ('g', 1)
                            MidSquares = [('f', 1);('g', 1)]
                        }
                }
            else
                {
                    Forward = -1
                    Backward = 1
                    Left = 1
                    Right = -1
                    Straight = 0
                    FirstRank = 8
                    SecondRank = 7
                    EighthRank = 1
                    OpponentSecondRank = 2
                    LeftCastle = 
                        {
                            RookStart = ('h', 8)
                            RookEnd = ('f', 1)
                            KingEnd = ('g', 1)
                            MidSquares = [('f', 1);('g', 1)]
                        }
                    RightCastle = 
                        {
                            RookStart = ('a', 1)
                            RookEnd = ('d', 1)
                            KingEnd = ('c', 1)
                            MidSquares = [('b', 1);('c', 1);('d', 1)]
                        }
                }

    let CheckForCastleTake (lastMove:Move option) (moves:Move list): Move list =
        if lastMove.IsNone || not lastMove.Value.HasCastle then moves
        else
            let castle = lastMove.Value.AsCastle
            let CheckForTakesKing (move:Move) =
                if move.Takes.IsSome then move
                else 
                if castle.PassThroughLocations |> List.contains (move.To.Location)
                    then move.AddTakes lastMove.Value.To // King's position
                    else move
            moves |> List.map CheckForTakesKing


    let PotentialMovesForPawn (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
        let opponentColor = position.Color.Other
        let orientation = OrientationFor position.Color
        let lastMove = LastMove score
        let forwardMove1 =
            let toLocation = Location.RelativeLocation 0 orientation.Forward position.Location
            let takePosition = if toLocation.IsSome then Positions.PositionAt toLocation.Value positions else None
            let toLocation = if takePosition.IsNone then toLocation else None
            let toPosition = if toLocation.IsSome then Some (position.SetLocation toLocation.Value) else None
            if toLocation.IsSome then Some (Move'(position, toPosition.Value)) else None
        let forwardMove2 = 
            let toLocation = if position.Rank = orientation.SecondRank then Location.RelativeLocation 0 (orientation.Forward * 2) position.Location else None
            let toPosition = if toLocation.IsSome then Positions.PositionAt toLocation.Value positions else None
            let toLocation = if toPosition.IsNone then toLocation else None
            let toPosition = if toLocation.IsSome then Some (position.SetLocation toLocation.Value) else None
            if toLocation.IsSome then Some (Move'(position, toPosition.Value)) else None
        let possibleEnPassant = 
            lastMove.IsSome && 
            lastMove.Value.To.Color = opponentColor && 
            lastMove.Value.To.Type = Type.Pawn &&
            lastMove.Value.From.Rank = 
                orientation.OpponentSecondRank &&
                abs lastMove.Value.To.Rank - lastMove.Value.From.Rank = 2
        let isEnPassantMove (direction:int) =
            if not possibleEnPassant then false
            else
                let toAdjacentColumn = Location.RelativeFile direction position.File
                toAdjacentColumn.IsSome && toAdjacentColumn.Value = lastMove.Value.To.File 
        let leftMove = 
            let toLocation = Location.RelativeLocation orientation.Left orientation.Forward position.Location
            let takePosition = if toLocation.IsSome then Positions.PositionAt toLocation.Value positions else None
            let takePosition = 
                if takePosition.IsSome then takePosition
                else
                    if isEnPassantMove orientation.Left
                        then Some lastMove.Value.To
                        else None
            let toLocation = if takePosition.IsSome && (takePosition.Value.Color = opponentColor) then toLocation else None
            let toPosition = if toLocation.IsSome then Some (position.SetLocation toLocation.Value) else None
            if toLocation.IsSome then Some (Move.Make position toPosition.Value takePosition) else None
        let rightMove = 
            let toLocation = Location.RelativeLocation orientation.Right orientation.Forward position.Location
            let takePosition = if toLocation.IsSome then Positions.PositionAt toLocation.Value positions else None
            let takePosition = 
                if takePosition.IsSome then takePosition
                else    
                    if isEnPassantMove orientation.Right
                        then Some lastMove.Value.To
                        else None
            let toLocation = if takePosition.IsSome && (takePosition.Value.Color = opponentColor) then toLocation else None
            let toPosition = if toLocation.IsSome then Some (position.SetLocation toLocation.Value) else None
            if toLocation.IsSome then Some (Move.Make position toPosition.Value takePosition) else None
        let moves = [forwardMove1;forwardMove2;leftMove;rightMove]
                    |> List.filter (fun move -> move.IsSome)
                    |> List.map (fun move -> move.Value)
        let moves = moves |> CheckForCastleTake lastMove
        let expandIfPromtion (move:Move): Move list =
            if move.To.Rank = orientation.EighthRank
                then Types.ForPromotion
                        |> List.map (fun piece -> move.AddExtension (Promotion(PromotionMove.Make piece)))
                else [move]
        let moves = moves |> List.collect expandIfPromtion
        moves

    let PotentialMoves (position:Position) (fileRankOffsets: (int * int) list) (score:Move list) (positions:Position list) : Move list =
        let PositionAt (location:Location) = Positions.PositionAt location positions
        let offsetLocations = 
            fileRankOffsets
            |> List.map (fun x -> Location.RelativeLocation (fst x) (snd x) position.Location)
            |> List.filterSome
        let ToLocationAndTakePosition (location:Location) = LocationAndTakePosition.Make location (PositionAt location)
        let locationAndTakePositions =
            offsetLocations
            |> List.map ToLocationAndTakePosition
        let IsValid (x:LocationAndTakePosition) = x.TakePosition.IsNone || x.TakePosition.Value.Color <> position.Color
        let locationAndTakePositions = 
            locationAndTakePositions 
            |> List.filter IsValid
        let MakeMove (x:LocationAndTakePosition) = TakesMove'(position, {position with Location = x.Location}, x.TakePosition)
        let moves = locationAndTakePositions |> List.map MakeMove
        let moves = moves |> CheckForCastleTake (LastMove score)
        moves

    let PotentialMovesForKnight (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
       let orientation = OrientationFor position.Color
       let offsets = 
            [
                (orientation.Left, orientation.Forward * 2)
                (orientation.Right, orientation.Forward * 2)
                (orientation.Right * 2, orientation.Forward)
                (orientation.Right * 2, orientation.Backward)
                (orientation.Right, orientation.Backward * 2)
                (orientation.Left, orientation.Backward * 2)
                (orientation.Left * 2, orientation.Backward)
                (orientation.Left * 2, orientation.Forward)
            ]
       PotentialMoves position offsets score positions

    let PotentialContigousMoves (position:Position) (fileRankDirections: (int * int) list) (score:Move list) (positions:Position list) : Move list =
        let PositionAt (location:Location) = Positions.PositionAt location positions
        let ToLocationAndTakePosition (location:Location) = LocationAndTakePosition.Make location (PositionAt location)
        let InsertOrIgnore (acc:LocationAndTakePosition list) (x:LocationAndTakePosition) =
            if (acc.IsEmpty || acc.Head.TakePosition.IsNone) then x :: acc else acc
        let TakePositionIsValid (x:LocationAndTakePosition) = x.TakePosition.IsNone || x.TakePosition.Value.Color = position.Color.Other

        let LocationAndTakePositionsInDirection (columnDirection:int, rowDirection:int) =
            let locations = Location.AdjacentLocations columnDirection rowDirection Location.maxSpaces position.Location
            let locationAndTakePositions = locations |> List.map ToLocationAndTakePosition
            let locationAndTakePositions = 
                locationAndTakePositions 
                |> List.fold InsertOrIgnore []
                |> List.filter TakePositionIsValid
                |> List.rev
            locationAndTakePositions
        let locationAndTakePositions = 
            fileRankDirections 
            |> List.map LocationAndTakePositionsInDirection 
            |> List.collectAll
        let MakeMove (x:LocationAndTakePosition) = TakesMove'(position, position.SetLocation(x.Location), x.TakePosition)
        let moves = locationAndTakePositions |> List.map MakeMove
        let moves = moves |> CheckForCastleTake (LastMove score)
        moves
                           
    let PotentialMovesForBishop (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
        let directions = OrientationFor position.Color
        let vectors = 
            [
                (directions.Left, directions.Forward)
                (directions.Right, directions.Forward)
                (directions.Right, directions.Backward)
                (directions.Left, directions.Backward)
            ]
        PotentialContigousMoves position vectors score positions

    let PotentialMovesForRook (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
        let orientation = OrientationFor position.Color
        let directions = 
            [
                (orientation.Straight, orientation.Forward)
                (orientation.Right, orientation.Straight)
                (orientation.Straight, orientation.Backward)
                (orientation.Left, orientation.Straight)
            ]
        PotentialContigousMoves position directions score positions

    let PotentialMovesForQueen (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
        let orientation = OrientationFor position.Color
        let directions = 
            [
                (orientation.Straight, orientation.Forward)
                (orientation.Right, orientation.Straight)
                (orientation.Straight, orientation.Backward)
                (orientation.Left, orientation.Straight)
                (orientation.Left, orientation.Forward)
                (orientation.Right, orientation.Forward)
                (orientation.Right, orientation.Backward)
                (orientation.Left, orientation.Backward)
            ]
        PotentialContigousMoves position directions score positions


    let PotentialMovesForKing (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list) : Move list =
        let orientation = OrientationFor position.Color
        let offsets = 
            [
                (orientation.Left, orientation.Forward)
                (orientation.Straight, orientation.Forward)
                (orientation.Right, orientation.Forward)
                (orientation.Right, orientation.Straight)
                (orientation.Right, orientation.Backward)
                (orientation.Straight, orientation.Backward)
                (orientation.Left, orientation.Backward)
                (orientation.Left, orientation.Straight)
            ]
        let regularMoves = PotentialMoves position offsets score positions
        
        let kingUnderAttack = underAttack |> PositionsContainLocation position.Location
        let kingCanCastle = not (score |> MovesContainFromLocation position.Location)
        let leftRookUnderAttack = underAttack |> PositionsContainLocation orientation.LeftCastle.RookStart
        let leftRookCanCastle = not (score |> MovesContainFromLocation orientation.LeftCastle.RookStart)
        let leftMidsquaresAreEmpty = Positions.AreEmpty orientation.LeftCastle.MidSquares positions
        let rightRookUnderAttack = underAttack |> PositionsContainLocation orientation.RightCastle.RookStart
        let rightRookCanCastle = not (score |> MovesContainFromLocation orientation.RightCastle.RookStart)
        let rightMidsquaresAreEmpty = Positions.AreEmpty orientation.RightCastle.MidSquares positions
        let leftCastleMove =
            if not kingUnderAttack && kingCanCastle && not leftRookUnderAttack && leftRookCanCastle && leftMidsquaresAreEmpty
                then Some (CastleMove'(position, orientation.LeftCastle))
                else None
        let rightCastleMove =
            if not kingUnderAttack && kingCanCastle && not rightRookUnderAttack && rightRookCanCastle && rightMidsquaresAreEmpty
                then Some (CastleMove'(position, orientation.RightCastle))
                else None
        let castleMoves = [leftCastleMove; rightCastleMove] |> List.filterSome
        let allMoves = regularMoves @ castleMoves
        allMoves
            
    let PotentialMovesFor = 
        [
            Type.Pawn, PotentialMovesForPawn
            Type.Knight, PotentialMovesForKnight
            Type.Bishop, PotentialMovesForBishop
            Type.Rook, PotentialMovesForRook
            Type.Queen, PotentialMovesForQueen
            Type.King, PotentialMovesForKing
        ] |> Map.ofList

    let PotentialMovesForPosition (position:Position) (score:Move list) (underAttack:Position list) (positions:Position list): Move list =
        PotentialMovesFor[position.Type] position score underAttack positions

    let PotentialMovesForColor (color:Color) (score:Move list) (underAttack:Position list) (positions:Position list): Move list =
        let PotentialMoves (position:Position) = PotentialMovesForPosition position score underAttack positions 
        let playerPositions = Positions.PositionsOfColor color positions 
        let potentialMoves = playerPositions |> List.collect PotentialMoves
        potentialMoves
        
    let PositionsUnderAttack (color:Color) (score:Move list) (positions:Position list) =
        let opponentPotentialMoves = PotentialMovesForColor color.Other score [] positions
        let underAttack = 
            opponentPotentialMoves 
            |> List.map (fun x -> x.Takes)
            |> List.filterSome
        underAttack

    let MovesForPlayer (color:Color) (score:Move list) (positions:Position list): Move list =
        let underAttack = PositionsUnderAttack color score positions 
        let potentialMoves = PotentialMovesForColor color score underAttack positions
        let MoveAllowsCheck (move:Move): bool =
            let nextPositions = Apply move positions
            let nextScore = score @ [move]
            let opponentUnderAttack = PositionsUnderAttack color.Other nextScore nextPositions
            let potentialOpponentMoves = PotentialMovesForColor color.Other nextScore opponentUnderAttack nextPositions
            InCheck potentialOpponentMoves
        let KingProtected (move:Move) = not (MoveAllowsCheck move)
        let potentialMoves = potentialMoves |> List.filter KingProtected
        potentialMoves