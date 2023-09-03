namespace GameLogic.Chess.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open GameLogic.Chess

[<TestClass>]
type MovesTestClass () =

    [<TestMethod>]
    member this.InitialMoves () =
        let positions = Positions.initial
        let possibleMoves = Moves.MovesForPlayer Color.White [] positions
        Assert.AreEqual(20, possibleMoves.Length)
