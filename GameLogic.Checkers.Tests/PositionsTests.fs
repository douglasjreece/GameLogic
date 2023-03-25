namespace GameLogic.Checkers.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Linq
open GameLogic.Checkers
open Types


[<TestClass>]
type PositionsTests () =
    
    [<TestMethod>]
    member this.Initial() =
        let result = Positions.Initial

        let darkCount = result |> List.filter (fun p -> p.Color = Color.Dark) |> List.length
        let lightCount = result |> List.filter (fun p -> p.Color = Color.Light) |> List.length
        let manCount = result |> List.filter (fun p -> p.Piece = Piece.Man) |> List.length
        let kingCount = result |> List.filter (fun p -> p.Piece = Piece.King) |> List.length

        Assert.AreEqual(12, darkCount)
        Assert.AreEqual(12, lightCount)
        Assert.AreEqual(24, manCount)
        Assert.AreEqual(0, kingCount)


    [<TestMethod>]
    member this.IsEmtpy () =
        let initial = Positions.Initial
        let emptyLocation = Location.At 13
        let occupiedLocation = Location.At 9

        let isEmtpyTrue = Positions.IsEmpty emptyLocation initial
        let isEmtpyFalse = Positions.IsEmpty occupiedLocation initial

        Assert.IsTrue(isEmtpyTrue)
        Assert.IsFalse(isEmtpyFalse)

    [<TestMethod>]
    member this.HasColor () =
        let initial = Positions.Initial
        let emptyLocation = Location.At 13
        let darkLocation = Location.At 9
        let lightLocation = Location.At 24

        let hasColorFalse1 = Positions.HasColor emptyLocation Color.Dark initial
        let hasColorFalse2 = Positions.HasColor lightLocation Color.Dark initial
        let hasColorTrue = Positions.HasColor darkLocation Color.Dark initial

        Assert.IsFalse(hasColorFalse1)
        Assert.IsFalse(hasColorFalse2)
        Assert.IsTrue(hasColorTrue)


    [<TestMethod>]
    member this.ApplyRegularMove() =
        let move = { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
        
        let result = Positions.ApplyRegularMove move Positions.Initial

        let is9Empty = Positions.IsEmpty (Location.At 9) result
        let is14Color = Positions.HasColor (Location.At 14) Color.Dark result
        let darkCount = result |> List.filter (fun p -> p.Color = Color.Dark) |> List.length
        let lightCount = result |> List.filter (fun p -> p.Color = Color.Light) |> List.length
        let manCount = result |> List.filter (fun p -> p.Piece = Piece.Man) |> List.length
        let kingCount = result |> List.filter (fun p -> p.Piece = Piece.King) |> List.length

        Assert.IsTrue(is9Empty)
        Assert.IsTrue(is14Color)
        Assert.AreEqual(12, darkCount)
        Assert.AreEqual(12, lightCount)
        Assert.AreEqual(24, manCount)
        Assert.AreEqual(0, kingCount)


    [<TestMethod>]
    member this.ApplyRegularMoveToKing() =
        let moves = 
            [
                { RegularMove.From = Location.At 32; RegularMove.To = Location.At 20 }
                { RegularMove.From = Location.At 27; RegularMove.To = Location.At 19 }
                { RegularMove.From = Location.At 11; RegularMove.To = Location.At 27 }
            ]
        let move = { RegularMove.From = Location.At 27; RegularMove.To = Location.At 32}

        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial
        
        let result = Positions.ApplyRegularMove move positions

        let darkCount = result |> List.filter (fun p -> p.Color = Color.Dark) |> List.length
        let lightCount = result |> List.filter (fun p -> p.Color = Color.Light) |> List.length
        let manCount = result |> List.filter (fun p -> p.Piece = Piece.Man) |> List.length
        let kingCount = result |> List.filter (fun p -> p.Piece = Piece.King) |> List.length

        Assert.AreEqual(12, darkCount)
        Assert.AreEqual(12, lightCount)
        Assert.AreEqual(23, manCount)
        Assert.AreEqual(1, kingCount)

    [<TestMethod>]
    member this.IntialRegularMoves() =
        let initial = Positions.Initial

        let result = Positions.ValidMovesForColor Color.Dark initial
        let regularMoves = Moves.RegularMoves result
        let jumpMoves = Moves.JumpMoves result

        let minLocation = regularMoves |> List.map (fun m -> m.From.Number) |> List.min
        let maxLocation = regularMoves |> List.map (fun m -> m.From.Number) |> List.max

        Assert.AreEqual(7, regularMoves.Length)
        Assert.AreEqual(0, jumpMoves.Length)
        Assert.AreEqual(9, minLocation)
        Assert.AreEqual(12, maxLocation)    

    [<TestMethod>]
    member this.ValidJumpMoves() =
        let moves = 
            [
                { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let position = positions |> List.find (fun p -> p.Location.Number = 14)
        let result = Positions.ValidJumpMoves position positions

        Assert.AreEqual(1, result.Length)

    [<TestMethod>]
    member this.ValidCascadingJumpMoves() =
        let moves = 
            [
                { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let position = positions |> List.find (fun p -> p.Location.Number = 14)
        let result = Positions.ValidCascadingJumpMoves position positions

        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)

    [<TestMethod>]
    member this.ValidCascadingJumpMovesDouble() =
        let moves = 
            [
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 27; RegularMove.To = Location.At 23}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let position = positions |> List.find (fun p -> p.Location.Number = 9)
        let result = Positions.ValidCascadingJumpMoves position positions

        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(2, result.[0].Length)

    [<TestMethod>]
    member this.ValidCascadingJumpMovesMultiple() =
        let moves = 
            [
                { RegularMove.From = Location.At 24; RegularMove.To = Location.At 15 }
                { RegularMove.From = Location.At 21; RegularMove.To = Location.At 14 }
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let position = positions |> List.find (fun p -> p.Location.Number = 10)
        let result = Positions.ValidCascadingJumpMoves position positions

        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(1, result.[1].Length)

    [<TestMethod>]
    member this.ValidCascadingJumpMovesMultipleWtihDouble() =
        let moves = 
            [
                { RegularMove.From = Location.At 24; RegularMove.To = Location.At 15 }
                { RegularMove.From = Location.At 21; RegularMove.To = Location.At 14 }
                { RegularMove.From = Location.At 26; RegularMove.To = Location.At 21 }
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let position = positions |> List.find (fun p -> p.Location.Number = 10)
        let result = Positions.ValidCascadingJumpMoves position positions

        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(2, result.[0].Length)
        Assert.AreEqual(2, result.[1].Length)


    [<TestMethod>]
    member this.JumpMoves() =
        let moves = 
            [
                { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let result = Positions.ValidMovesForColor Color.Dark positions
        let regularMoves = Moves.RegularMoves result
        let jumpMoves = Moves.JumpMoves result

        Assert.AreEqual(0, regularMoves.Length)
        Assert.AreEqual(1, jumpMoves.Length)
        Assert.AreEqual(1, jumpMoves.[0].Length)

 
    [<TestMethod>]
    member this.ApplyJumpMove() =
        let moves = 
            [
                { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let validMoves = Positions.ValidMovesForColor Color.Dark positions
        let jumpMoves = Moves.JumpMoves validMoves
        let result = Positions.ApplyJumpMoves jumpMoves.[0] positions

        let is18Empty = Positions.IsEmpty (Location.At 18) result
        let darkCount = result |> List.filter (fun p -> p.Color = Color.Dark) |> List.length
        let lightCount = result |> List.filter (fun p -> p.Color = Color.Light) |> List.length
        let manCount = result |> List.filter (fun p -> p.Piece = Piece.Man) |> List.length
        let kingCount = result |> List.filter (fun p -> p.Piece = Piece.King) |> List.length

        Assert.IsTrue(is18Empty)
        Assert.AreEqual(12, darkCount)
        Assert.AreEqual(11, lightCount)
        Assert.AreEqual(23, manCount)
        Assert.AreEqual(0, kingCount)
 
    [<TestMethod>]
    member this.ApplyJumpMoveToKing() =
        let moves = 
            [
                { RegularMove.From = Location.At 9; RegularMove.To = Location.At 14}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
                { RegularMove.From = Location.At 32; RegularMove.To = Location.At 20}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let validMoves = Positions.ValidMovesForColor Color.Dark positions
        let jumpMoves = Moves.JumpMoves validMoves
        let result = Positions.ApplyJumpMoves jumpMoves.[0] positions

        let is18Empty = Positions.IsEmpty (Location.At 18) result
        let darkCount = result |> List.filter (fun p -> p.Color = Color.Dark) |> List.length
        let lightCount = result |> List.filter (fun p -> p.Color = Color.Light) |> List.length
        let manCount = result |> List.filter (fun p -> p.Piece = Piece.Man) |> List.length
        let kingCount = result |> List.filter (fun p -> p.Piece = Piece.King) |> List.length

        Assert.IsTrue(is18Empty)
        Assert.AreEqual(12, darkCount)
        Assert.AreEqual(10, lightCount)
        Assert.AreEqual(21, manCount)
        Assert.AreEqual(1, kingCount)

    [<TestMethod>]
    member this.KingJumpMoves() =
        let moves = 
            [
                { RegularMove.From = Location.At 32; RegularMove.To = Location.At 20}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
                { RegularMove.From = Location.At 12; RegularMove.To = Location.At 32}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let result = Positions.ValidMovesForColor Color.Dark positions
        let regularMoves = Moves.RegularMoves result
        let jumpMoves = Moves.JumpMoves result

        Assert.AreEqual(0, regularMoves.Length)
        Assert.AreEqual(1, jumpMoves.Length)
        Assert.AreEqual(2, jumpMoves.[0].Length)

    [<TestMethod>]
    member this.KingJumpMovesBothDirections() =
        let moves = 
            [
                { RegularMove.From = Location.At 32; RegularMove.To = Location.At 20}
                { RegularMove.From = Location.At 23; RegularMove.To = Location.At 18}
                { RegularMove.From = Location.At 12; RegularMove.To = Location.At 32}
                { RegularMove.From = Location.At 32; RegularMove.To = Location.At 23}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) Positions.Initial

        let result = Positions.ValidMovesForColor Color.Dark positions
        let regularMoves = Moves.RegularMoves result
        let jumpMoves = Moves.JumpMoves result
        let maxToLocation = jumpMoves |> List.map (fun m -> m.[0].To.Number) |> List.max
        let minToLocation = jumpMoves |> List.map (fun m -> m.[0].To.Number) |> List.min

        Assert.AreEqual(0, regularMoves.Length)
        Assert.AreEqual(2, jumpMoves.Length)
        Assert.AreEqual(1, jumpMoves.[0].Length)
        Assert.AreEqual(1, jumpMoves.[1].Length)
        Assert.AreEqual(32, maxToLocation)
        Assert.AreEqual(14, minToLocation)
 