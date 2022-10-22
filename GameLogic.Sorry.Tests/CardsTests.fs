namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type CardsTestsClass () =

    [<TestMethod>]
    member this.AllCards () =
        // act
        let groups = Cards.All |> List.groupBy (fun x -> x)
        let minCount = groups |> List.map (fun x -> (snd x).Length) |> List.min
        let maxCount = groups |> List.map (fun x -> (snd x).Length) |> List.max
        
        // assert
        Assert.AreEqual(11, groups.Length)
        Assert.AreEqual(4, minCount)
        Assert.AreEqual(4, maxCount)

