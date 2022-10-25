namespace GameLogic.Othello

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Positions =
    open Types

    let Empty: Position list = []

    let InitialSetup =
        [
            Position.Of (Square.At 'D' 4) Color.White
            Position.Of (Square.At 'E' 4) Color.Black
            Position.Of (Square.At 'D' 5) Color.Black
            Position.Of (Square.At 'E' 5) Color.White
        ]

    let PositionAt (square: Square) (positions: Positions) =
        positions |> List.findOrNone (fun position -> position.Square = square)

    let HasPositionAt (square: Square) (positions: Positions) =
        let found = PositionAt square positions
        found.IsSome

    let IsOpen (position: Position) (direction: Direction * Direction) (positions: Positions) =
        let location = Square.Neighbor direction position.Square
        match location with
        | Some (location') -> not (HasPositionAt location' positions)
        | None -> false

    let EnclosingSquares (position: Position) (direction: Direction * Direction) (positions: Positions) =
        let rec aux square acc =
            match square with
            | Some square' -> 
                let position' = PositionAt square' positions
                match position' with
                | Some position'' -> 
                    let acc' = (position''.Square :: acc)
                    if position''.Color = position.Color
                    then if acc' |> List.length > 1 then acc' else []
                    else aux (Square.Neighbor direction square') acc'
                        
                | None -> []
            | None -> []
        let result = aux (Square.Neighbor direction position.Square) []
        result
            
    let HasEnclosingSquares (position: Position) (direction: Direction * Direction) (positions: Positions) =
        (EnclosingSquares position direction positions |> List.length) > 0

    let ToColor (color: Color) (squares: Squares) (positions: Positions) = 
        let positionColor (position: Position) =
            if List.contains position.Square squares 
            then color
            else position.Color
        positions |> List.map (fun position -> { position with Color = positionColor position })

    let PotentialPlaysForPosition (position: Position) (positions: Positions) =
        let otherColor = position.Color.Next
        let isPlayable (direction) =
            let isOpen = IsOpen position direction positions
            let testSquare = position.Square |> Square.Neighbor direction
            match testSquare with
                | Some testSquare' ->
                    let testPosition = Position.Of testSquare' otherColor
                    let hasEnclosingPosition = HasEnclosingSquares testPosition (Directions.Opposite direction) positions
                    let result = isOpen && hasEnclosingPosition
                    result
                | None -> false
        let potentialSquares = Directions.All 
                                |> List.map (fun direction -> if isPlayable direction then Square.Neighbor direction position.Square else None)
                                |> List.filterSome
        let result = potentialSquares |> List.map (fun square -> Position.Of square otherColor)
        result
       
    let PotentialPlays (positions: Positions) (color: Color) =
        let otherColor = color.Next
        let otherColorPositions = positions |> List.filter (fun position -> position.Color = otherColor)
        let result = 
            otherColorPositions 
            |> List.collect (fun position -> PotentialPlaysForPosition position positions)
            |> List.distinct
        result

    let ColorCount (color: Color) (positions: Positions) =
        positions |> List.filter (fun position -> position.Color = color) |> List.length

    let AddXYPositions (locations: (char * int) list) (color: Color) (positions: Positions) =
        let newPositions = locations |> List.map (fun x_y -> Position.Of (Square.At (fst x_y) (snd x_y)) color)
        positions @ newPositions

    let AddTriplesPositions (triples: (char * int * Color) list) (positions: Positions) =
        let make (triple: (char * int * Color)) =
            let x, y, c = triple
            Position.Of (Square.At x y) c
        let newPositions = triples |> List.map (fun triple -> make triple)
        positions @ newPositions

    let Squares (positions: Positions) = 
        positions |> List.map (fun position -> position.Square)
        
    let Sort (positions: Positions) =
        positions |> List.sortBy (fun position -> position.Square.Y, position.Square.X)