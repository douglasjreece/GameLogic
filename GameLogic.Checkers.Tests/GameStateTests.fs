namespace GameLogic.Checkers.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Linq
open GameLogic.Checkers
open Types


[<TestClass>]
type GameStateTests () =
    
    [<TestMethod>]
    member this.Initial() =
        let result = GameState.Initial

        let regularMoves = Moves.RegularMoves result.AvailableMoves

        let minLocation = regularMoves |> List.map (fun m -> m.From.Number) |> List.min
        let maxLocation = regularMoves |> List.map (fun m -> m.From.Number) |> List.max

        Assert.AreEqual(7, regularMoves.Length)
        Assert.AreEqual(9, minLocation)
        Assert.AreEqual(12, maxLocation)    
        Assert.AreEqual(Positions.Initial, result.Positions)
        Assert.AreEqual(Color.Dark, result.PlayerUp)
        Assert.IsFalse(result.WinnerColor.IsSome)

    [<TestMethod>]
    member this.ApplyMove() =
        let initial = GameState.Initial

        let result = GameState.ApplyMove initial.AvailableMoves.Head initial 

        let maxDarkLocation = 
            result.Positions 
            |> List.filter (fun p -> p.Color = Color.Dark) 
            |> List.map (fun p -> p.Location.Number)
            |> List.max

        Assert.AreEqual(Color.Light, result.PlayerUp)
        Assert.IsFalse(result.WinnerColor.IsSome)
        Assert.AreEqual(13, maxDarkLocation)

    [<TestMethod>]
    member this.GameEndBlocked() =
        let initial = GameState.Initial

        let justOne =
            initial.Positions
            |> List.filter (fun p -> p.Color = Color.Dark || p.Location.Number = 32)

        let moves = 
            [
                { RegularMove.From = Location.At 12; RegularMove.To = Location.At 28}
                { RegularMove.From = Location.At 11; RegularMove.To = Location.At 27}
                { RegularMove.From = Location.At 7; RegularMove.To = Location.At 18}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) justOne

        let modified = 
            { initial with Positions = positions }

        let move = RegularMove( { RegularMove.From = Location.At 18; RegularMove.To = Location.At 23})

        let result = GameState.ApplyMove move modified 

        Assert.AreEqual(Color.Light, result.PlayerUp)
        Assert.IsTrue(result.WinnerColor.IsSome)
        Assert.AreEqual(result.WinnerColor.Value, Color.Dark)


    [<TestMethod>]
    member this.GameEndJumped() =
        let initial = GameState.Initial

        let justOne =
            initial.Positions
            |> List.filter (fun p -> p.Color = Color.Dark || p.Location.Number = 27)

        let moves = 
            [
                { RegularMove.From = Location.At 11; RegularMove.To = Location.At 23}
            ]
        let positions = 
            moves |> List.fold (fun p m -> Positions.ApplyRegularMove m p) justOne

        let modified = 
            { initial with Positions = positions }
        let availableMoves = Positions.ValidMovesForColor Color.Dark positions

        let result = GameState.ApplyMove availableMoves.Head modified 

        Assert.AreEqual(Color.Light, result.PlayerUp)
        Assert.IsTrue(result.WinnerColor.IsSome)
        Assert.AreEqual(result.WinnerColor.Value, Color.Dark)


