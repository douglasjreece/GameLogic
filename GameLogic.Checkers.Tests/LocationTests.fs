namespace GameLogic.Checkers.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Linq
open GameLogic.Checkers
open Types

[<TestClass>]
type LocationTestsClass () =

    [<TestMethod>]
    member this.NextMoveLocations () =
        let initial = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })

        let initialMoveCounts = 
            initial
            |> List.map (fun l -> 
                            let g = l.ToGrid
                            if g.Row = 7
                                then 0
                                else 
                                    if g.Column = 0 || g.Column = 7
                                        then 1
                                        else 2
                        )

        let result = 
            initial 
            |> List.map (fun l -> Location.NextMoveLocations l Types.Direction.Increment)

        let resultMoveCounts = 
            result
            |> List.map (fun list -> list.Length)

        let resultRowDiffs = 
            (initial, result) 
            ||> List.map2 (fun l ls -> ls |> List.map (fun l' -> l'.ToGrid.Row - l.ToGrid.Row ))
            |> List.collect (fun list -> list)
            |> List.distinct

        let resultColDiffs = 
            (initial, result) 
            ||> List.map2 (fun l ls -> ls |> List.map (fun l' -> l'.ToGrid.Column - l.ToGrid.Column ))
            |> List.collect (fun list -> list)
            |> List.distinct
            |> List.sort

        Assert.AreEqual(initialMoveCounts, resultMoveCounts)
        Assert.AreEqual([1], resultRowDiffs)
        Assert.AreEqual([-1;1], resultColDiffs)


    [<TestMethod>]
    member this.NextJumpLocations () =
        let initial = 
            [1..32]
            |> List.map (fun x -> { Location.Number = x })

        let initialMoveCounts = 
            initial
            |> List.map (fun l -> 
                            let g = l.ToGrid
                            if g.Row >= 6
                                then 0
                                else 
                                    if g.Column <= 1 || g.Column >= 6
                                        then 1
                                        else 2
                        )

        let result = 
            initial 
            |> List.map (fun l -> Location.NextJumpLocations l Types.Direction.Increment)

        let resultMoveCounts = 
            result
            |> List.map (fun list -> list.Length)


        let resultRowDiffs = 
            (initial, result) 
            ||> List.map2 (fun l ls -> ls |> List.map (fun l' -> l'.ToGrid.Row - l.ToGrid.Row ))
            |> List.collect (fun list -> list)
            |> List.distinct

        let resultColDiffs = 
            (initial, result) 
            ||> List.map2 (fun l ls -> ls |> List.map (fun l' -> l'.ToGrid.Column - l.ToGrid.Column ))
            |> List.collect (fun list -> list)
            |> List.distinct
            |> List.sort

        Assert.AreEqual(initialMoveCounts, resultMoveCounts)
        Assert.AreEqual([2], resultRowDiffs)
        Assert.AreEqual([-2;2], resultColDiffs)

    [<TestMethod>]
    member this.MoveDirection () =
        let initial = { Location.Number = 10 }
        
        let incrementResult = Location.NextMoveLocations initial Direction.Increment
        let decrementResult = Location.NextMoveLocations initial Direction.Decrement

        Assert.AreEqual(initial.ToGrid.Row + 1, incrementResult.[0].ToGrid.Row)
        Assert.AreEqual(initial.ToGrid.Row - 1, decrementResult.[0].ToGrid.Row)

    [<TestMethod>]
    member this.JumpDirection () =
        let initial = { Location.Number = 10 }
        
        let incrementResult = Location.NextJumpLocations initial Direction.Increment
        let decrementResult = Location.NextJumpLocations initial Direction.Decrement

        Assert.AreEqual(initial.ToGrid.Row + 2, incrementResult.[0].ToGrid.Row)
        Assert.AreEqual(initial.ToGrid.Row - 2, decrementResult.[0].ToGrid.Row)

    [<TestMethod>]
    member this.JumpedLocationIncrement () =
        let fromLocation = { Location.Number = 10 }
        let toLocation = { Location.Number = 19 }

        let jumpedLoation = Location.JumpedLocation fromLocation toLocation

        Assert.AreEqual(15, jumpedLoation.Number)

    [<TestMethod>]
    member this.JumpedLocationDecrement () =
        let fromLocation = { Location.Number = 19 }
        let toLocation = { Location.Number = 10 }

        let jumpedLoation = Location.JumpedLocation fromLocation toLocation

        Assert.AreEqual(15, jumpedLoation.Number)
