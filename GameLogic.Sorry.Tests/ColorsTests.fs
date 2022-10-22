namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type ColorsTestsClass () =

    [<TestMethod>]
    member this.ColorsInPlay_DefaultOrder () =
        // act
        let colors = Colors.InPlay Colors.All Color.Red
        Assert.AreEqual(Colors.All, colors)

    [<TestMethod>]
    member this.ColorsInPlay_YellowFirstPlayer () =
        // act
        let colors = Colors.InPlay Colors.All Color.Yellow
        Assert.AreEqual([Color.Yellow; Color.Green; Color.Red; Color.Blue], colors)

    [<TestMethod>]
    member this.ColorsInPlay_TwoPlayers () =
        // act
        let colors = Colors.InPlay [Color.Blue; Color.Green] Color.Green
        Assert.AreEqual([Color.Green; Color.Blue], colors)
