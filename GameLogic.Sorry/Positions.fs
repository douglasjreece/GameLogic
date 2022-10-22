namespace GameLogic.Sorry

exception PositionsError of string

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Positions =
    let Initial (players: Players) (pawnCount: int): Positions =
        let pawnNumbers = [1..pawnCount]
        let pawnsForColor (color: Color) = pawnNumbers |> List.map (Pawn.Create color)
        let locationForColor (color: Color) = Location.Create color Zone.Start
        let locationsForColor (color: Color) = pawnNumbers |> List.map (locationForColor color)
        let positionsForColor (color: Color) = 
            let pawns = pawnsForColor color
            let locations = locationsForColor color
            List.map2 Position.Create pawns locations
        let initial = players |> List.map (fun x -> x.Color) |> List.collect positionsForColor 
        initial

    let ForPawnColor (color: Color) (positions: Positions) =
        positions |> List.filter (fun position -> position.Pawn.Color = color)

    let ExcludePawnColor (color: Color) (positions: Positions) =
        positions |> List.filter (fun position -> position.Pawn.Color <> color)

    let InZone (zone: Zone) (positions: Positions) =
        positions |> List.filter (fun position -> position.Location.Zone = zone)

    let InStart (positions: Positions) =
        positions |> InZone Zone.Start

    let OnBoard (positions: Positions) =
        positions |> InZone Zone.Board

    let InHome (positions: Positions) =
        positions |> InZone Zone.Home

    let InPlay (positions: Positions) =
        positions |> List.filter (fun position -> position.Location.Zone = Zone.Board || position.Location.Zone = Zone.Safety)

    let Locations (positions: Positions) =
        positions |> List.map (fun position -> position.Location)

    let AtLocation (location: Location) (positions: Positions) =
        positions |> List.first (fun position -> position.Location = location)

    let ForPawn (pawn: Pawn) (positions: Positions) =
        positions |> List.first (fun position -> position.Pawn = pawn)

    let ValidForwardMoves (positions: Positions) (color: Color) (count: int) =
        let playerPositions = positions |> ForPawnColor color |> OnBoard
        let locations = playerPositions |> Locations
        let hasLocation = Location.HasLocation locations
        let positionLocationOptTups = playerPositions |> List.map (fun postition -> postition, Location.ForwardNOption postition.Pawn.Color postition.Pawn.Number count postition.Location)
        let positionLocationTups = positionLocationOptTups |> List.filter (fun position_locationOpt -> (snd position_locationOpt).IsSome) |> List.map (fun position_locationOpt -> fst position_locationOpt, (snd position_locationOpt).Value)
        let validPositionLocationTups = positionLocationTups |> List.filter (fun position_location -> not (hasLocation (snd position_location)))
        let moves = validPositionLocationTups |> List.map (fun position_location -> Move.Create (fst position_location).Pawn (fst position_location).Location (snd position_location))
        moves

    let ValidBackwardMoves (positions: Positions) (color: Color) (count: int) =
        let playerPositions = positions |> ForPawnColor color |> OnBoard
        let locations = playerPositions |> Locations
        let hasLocation = Location.HasLocation locations
        let positionLocationOptTups = playerPositions |> List.map (fun position -> position, Location.BackwardN count position.Location)
        let positionLocationTups = positionLocationOptTups |> List.filter (fun position_location -> not (hasLocation (snd position_location)))
        let moves = positionLocationTups |> List.map (fun position_location -> Move.Create (fst position_location).Pawn (fst position_location).Location (snd position_location))
        moves

    let ValidDirectionMoves (positions: Positions) (color: Color) (count: int) =
        match count with
        | c when c > 0 -> ValidForwardMoves positions color count
        | c when c < 0 -> ValidBackwardMoves positions color -count
        | _ -> raise (PositionsError("Direction move must not be 0."))

    let ValidStartMove (positions: Positions) (color: Color) =
        let colorPositions = positions |> ForPawnColor color
        let positionsInStart = colorPositions |> InStart
        match positionsInStart with
        | position::_ ->
            let locations = colorPositions |> Locations
            let move = Move.CreateStartExit position.Pawn
            let isOccupied = Location.HasLocation locations move.To
            match isOccupied with
            | false -> [move]
            | _ -> []
        | _ -> []

    let ApplyMove (move: Move) (positions: Positions) =
        let map (position: Position) =
            if position.Pawn = move.Pawn
                then { position with Location = move.To}
                else position
        let result = positions |> List.map map
        result

    let ApplyMoves (moves: Moves) (positions: Positions) =
        moves |> List.fold (fun acc x -> ApplyMove x acc) positions

    let PlacePawn (pawn: Pawn) (at: Location) (positions: Positions) =
        let handle (position: Position) =
            if position.Pawn = pawn
                then Position.Create pawn at
                else position
        positions |> List.map handle

    let SetPosition (position: Position) (positions: Positions) =
        let handle (position': Position) =
            if position'.Pawn = position.Pawn
                then position
                else position'
        positions |> List.map handle

    let SideEffectMoves (positions: Positions) (move: Move): Moves =
        let regularSideEffects (positions': Positions) (move': Move) = 
            let occupied = positions' |> List.filter (fun position -> position.Location = move'.To)
            match occupied with
            | h::_ -> [Move.CreateSideEffect h.Pawn h.Location (Location.Start h.Pawn.Color h.Pawn.Number)]
            | _ -> []

        let slideAreaSideEffects (positions': Positions) (move': Move) (slideArea: SlideArea) =
            let positionsInSlideArea = positions' |> OnBoard |> List.filter (fun x -> x.Location.Color = move'.To.Color && x.Location.Spot >= slideArea.StartSpot && x.Location.Spot <= slideArea.EndSpot)
            let toStart = positionsInSlideArea |> List.map (fun position -> Move.CreateSideEffect position.Pawn position.Location (Location.Start position.Pawn.Color position.Pawn.Number)) 
            let sideEffectMoves = toStart @ [Move.CreateSlide move'.Pawn move'.To (Location.Create move'.To.Color Zone.Board slideArea.EndSpot)]
            sideEffectMoves

        let sideEffects (positions': Positions) (move': Move) =
            let slideArea = Board.SlideAreaForSpot move'.To.Spot
            match slideArea with
            | Some(slideArea) when move'.Pawn.Color <> move'.To.Color -> slideAreaSideEffects positions' move' slideArea
            | _ -> regularSideEffects positions' move'

        let swapSlideEffects (positions': Positions) (move': Move) =         
            let swappingPosition = positions' |> AtLocation move'.To
            let swappingMove = Move.CreateSwap swappingPosition.Pawn swappingPosition.Location move'.From
            let transPositions = positions |> List.filter (fun position -> position.Location <> move'.To && position.Location <> swappingMove.To)
            let sourceEffects = sideEffects transPositions move'
            let swapEffects = swappingMove :: sideEffects transPositions swappingMove
            let result = sourceEffects @ swapEffects
            result

        match move.Type = MoveType.Swap with 
        | true -> swapSlideEffects positions move
        | _ -> sideEffects positions move
        
    let MoveAndSideEffects (positions: Positions) (move: Move): Moves = move :: (SideEffectMoves positions move)

    let MovesAndSideEffects (positions: Positions) (moves: Move list): MovesList = moves |> List.map (fun x -> MoveAndSideEffects positions x)

