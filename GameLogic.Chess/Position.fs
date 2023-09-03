namespace GameLogic.Chess

type Position =
    {
        Piece: Piece
        Location: Location
    }
    static member Make (piece:Piece, location:Location) = { Piece = piece; Location = location }
    member this.SetLocation (location:Location) = { this with Location = location }
    member this.Color = Piece.ColorOf this.Piece
    member this.Type = Piece.TypeOf this.Piece
    member this.File = Location.FileOf this.Location
    member this.Rank = Location.RankOf this.Location

module Positions =
    open GameLogic.Library

    let Position' (piece:Piece, location:Location) = Position.Make(piece, location)

    let initial =
      let PiecesAt (piece:Piece, rank:int, files:char list) =
        files |> List.map (fun file -> Position'(piece, (file, rank)))
      [ // piece,         rank, files
        ((White, Pawn),   2,    ['a'..'h'])
        ((White, Rook),   1,    ['a';'h'])
        ((White, Knight), 1,    ['b';'g'])
        ((White, Bishop), 1,    ['c';'f'])
        ((White, Queen),  1,    ['d'])
        ((White, King),   1,    ['e'])
        ((Black, Pawn),   7,    ['a'..'h'])
        ((Black, Rook),   8,    ['a';'h'])
        ((Black, Knight), 8,    ['b';'g'])
        ((Black, Bishop), 8,    ['c';'f'])
        ((Black, Queen),  8,    ['d'])
        ((Black, King),   8,    ['e'])
      ] |> List.map PiecesAt |> List.collectAll        

    let PositionAt (location:Location) (positions:Position list): Position option =
        List.tryFind (fun x -> x.Location = location) positions

    let PositionsOf (piece:Piece) (positions:Position list): Position list =
        positions |> List.filter (fun position -> position.Piece = piece)

    let PositionsOfColor (color:Color) (positions:Position list): Position list =
        positions |> List.filter (fun position -> position.Color = color)

    let ColorAt (location:Location) (positions:Position list): Color option =
        let position = PositionAt location positions
        if position.IsSome
            then Some position.Value.Color
            else None

    let IsEmpty (location:Location) (positions:Position list): bool =
        (ColorAt location positions).IsNone
        
    let AreEmpty (locations:Location list) (positions:Position list): bool =
        let IsEmpty (location:Location) = (ColorAt location positions).IsNone
        locations |> List.all IsEmpty

