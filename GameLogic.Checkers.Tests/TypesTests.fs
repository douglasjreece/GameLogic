namespace GameLogic.Checkers.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open GameLogic.Checkers.Types
open System.Linq

[<TestClass>]
type TypesTestsClass () =
    
    [<TestMethod>]
    member this.LocationsAsGrids () =
        // act
        let result = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })
            |> List.map (fun l -> l.ToGrid)

        // assert
        let expected = 
            [0..7]
            |> List.collect (fun row -> [0..7] |> List.map (fun col -> (row,col)))
            |> List.map (fun c -> { Grid.Row = fst c; Grid.Column = snd c })
            |> List.filter (fun g -> g.Column % 2 <> g.Row % 2)
        CollectionAssert.AreEqual(expected.ToList(), result.ToList())

    [<TestMethod>]
    member this.GridsAsLocations () =
        // act
        let result = 
            [0..7]
            |> List.collect (fun row -> [0..7] |> List.map (fun col -> (row,col)))
            |> List.map (fun c -> { Grid.Row = fst c; Grid.Column = snd c })
            |> List.filter (fun g -> g.Column % 2 <> g.Row % 2)
            |> List.map (fun g -> g.ToLocation)

        // assert
        let expected = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })
        CollectionAssert.AreEqual(expected.ToList(), result.ToList())

    [<TestMethod>]
    member this.NextMoveToRight () =
        let initial = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })
            |> List.map (fun l -> l.ToGrid)
        let result = 
            initial
            |> List.map (fun g -> g.Next 1 -1)
        let resultValid = result |> List.map (fun g -> g.IsValid )

        let expected =
            initial
            |> List.map (fun g -> { Grid.Row = g.Row + 1; Grid.Column = g.Column - 1})

        let expectedValid = 
            initial
            |> List.map (fun g -> g.Column > 0 && g.Row < 7)

        CollectionAssert.AreEqual(result.ToList(), expected.ToList())
        CollectionAssert.AreEqual(resultValid.ToList(), expectedValid.ToList())

    [<TestMethod>]
    member this.NextMoveToLeft () =
        let initial = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })
            |> List.map (fun l -> l.ToGrid)
        let result = 
            initial
            |> List.map (fun g -> g.Next 1 1)
        let resultValid = result |> List.map (fun g -> g.IsValid )

        let expected =
            initial
            |> List.map (fun g -> { Grid.Row = g.Row + 1; Grid.Column = g.Column + 1})

        let expectedValid = 
            initial
            |> List.map (fun g -> g.Column < 7 && g.Row < 7)

        CollectionAssert.AreEqual(result.ToList(), expected.ToList())
        CollectionAssert.AreEqual(resultValid.ToList(), expectedValid.ToList())
            