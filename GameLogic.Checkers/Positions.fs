namespace GameLogic.Checkers

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Positions = 
    open Types

    let Initial = 
        let darkPositions = [1..Pieces.InitialCount] |> List.map (fun number -> Position.Initial Color.Dark ({ Location.Number = number }))
        let lightPositions = [1..Pieces.InitialCount] |> List.map (fun number -> Position.Initial Color.Light ({ Location.Number = Board.SquareCount - number + 1 }))
        darkPositions @ lightPositions

    let IsEmpty (location: Location) (positions: Position list) =
        let exists = positions |> List.exists(fun p -> p.Location = location)
        not exists

    let HasColor (location: Location) (color: Color) (positions: Position list) =
        positions |> List.exists(fun p -> p.Location = location && p.Color = color)
        

    let ValidRegularMoves (position: Position) (positions: Position list)  =
        let locations = 
            position.Directions 
            |> List.map (fun d -> Location.NextMoveLocations position.Location d)
            |> List.collect (fun list -> list)
        let availbleLocations = locations |> List.filter (fun l -> IsEmpty l positions) 
        let moves = availbleLocations |> List.map (fun l -> { RegularMove.From = position.Location; RegularMove.To = l })
        moves

    let ValidJumpMoves (position: Position) (positions: Position list) =
        let locations = 
            position.Directions 
            |> List.map (fun d -> Location.NextJumpLocations position.Location d)
            |> List.collect (fun list -> list)
        let availbleLocations = locations |> List.filter (fun l -> IsEmpty l positions)
        let moves = availbleLocations |> List.map (fun l -> { JumpMove.From = position.Location; JumpMove.To = l; JumpMove.Jumped = (Location.JumpedLocation position.Location l) })
        let result = moves |> List.filter (fun m -> HasColor m.Jumped position.Color.Other positions)
        result

    let ValidCascadingJumpMoves (position: Position) (positions: Position list) =
        let nextValidJumpMoves (moves: JumpMove list) =
            let lastMove = moves.[moves.Length-1]
            let nextPosition = { position with Location = lastMove.To }
            ValidJumpMoves nextPosition positions
            
        let rec aux (moves: JumpMove list list) =
            let currentLength = moves |> List.map (fun m -> m.Length) |> List.sum
            let next = moves |> List.map (fun m -> m @ nextValidJumpMoves m)
            let nextLength = moves |> List.map (fun m -> m.Length) |> List.sum
            if nextLength = currentLength then next else aux next

        let initial = ValidJumpMoves position positions |> List.map (fun m -> [m])
        aux initial

    let OfColor (color: Color) (positions: Position list) = positions |> List.filter (fun p -> p.Color = color)
    
    let ValidJumpMovesForColor (color: Color) (positions: Position list) =
        let positionsOfColor = positions |> OfColor color
        let moves = 
            positionsOfColor 
            |> List.map (fun p -> ValidCascadingJumpMoves p positions)
            |> List.collect (fun list -> list)
        moves

    let ValidRegularMovesForColor (color: Color) (positions: Position list) =
        let positionsOfColor = positions |> OfColor color
        let moves = 
            positionsOfColor 
            |> List.map (fun p -> ValidRegularMoves p positions)
            |> List.collect (fun list -> list)
        moves

    let ValidMovesForColor (color: Color) (positions: Position list) =
        let validJumpMoves = positions |> ValidJumpMovesForColor color
        let validRegularMoves = positions |> ValidRegularMovesForColor color
        if not validJumpMoves.IsEmpty
            then validJumpMoves |> List.map (fun move -> JumpMoves(move))
            else validRegularMoves |> List.map (fun move -> RegularMove(move))
    
    let PositionAt (location: Location) (positions: Position list) =
        positions |> List.find(fun p -> p.Location = location)
    
    let PieceAt (position: Position) =
        let row = position.Location.ToGrid.Row
        match position.Color, row with
        | Color.Dark, 7 -> Piece.King
        | Color.Light, 0 -> Piece.King
        | _ -> position.Piece

    let ApplyRegularMove (move: RegularMove) (positions: Position list) =
      let fromPosition = PositionAt move.From positions
      let toPosition = { fromPosition with Location = move.To }
      let toPosition = { toPosition with Piece = PieceAt toPosition }
      let result = 
        positions
        |> List.filter (fun p -> p.Location <> move.From)
        |> List.append [toPosition]
      result

    let ApplyJumpMoves (moves: JumpMove list) (positions: Position list) =
      let fromPosition = PositionAt moves.[0].From positions
      let toPosition = { fromPosition with Location = moves.[moves.Length-1].To }
      let toPosition = { toPosition with Piece = PieceAt toPosition }
      let jumpedLocations = moves |> List.map (fun m -> m.Jumped)
      let result = 
        positions
        |> List.filter (fun p -> not (List.contains p.Location jumpedLocations))
        |> List.filter (fun p -> p.Location <> fromPosition.Location)
        |> List.append [toPosition]
      result

    let ApplyMove (move: Move) (positions: Position list) =
        match move with 
        | RegularMove m -> ApplyRegularMove m positions
        | JumpMoves m -> ApplyJumpMoves m positions
