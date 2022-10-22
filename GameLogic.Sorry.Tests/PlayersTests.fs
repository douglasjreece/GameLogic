namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type PlayerssTestsClass () =

    [<TestMethod>]
    member this.NextPlayer () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let firstTurn = players.[0]
        let next = Players.Next players

        // act
        let secondTurn = next firstTurn
        let thirdTurn = next secondTurn
        let fourthTurn = next thirdTurn
        let fifthTurn = next fourthTurn

        // assert
        Assert.AreEqual(1, firstTurn.Number)
        Assert.AreEqual(2, secondTurn.Number)
        Assert.AreEqual(3, thirdTurn.Number)
        Assert.AreEqual(4, fourthTurn.Number)
        Assert.AreEqual(1, fifthTurn.Number)
