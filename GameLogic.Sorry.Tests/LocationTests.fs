namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type LocationTestsClass () =

    [<TestMethod>]
    member this.NextOneSpace () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 1
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardN playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(Board.StartSpot + 1, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.NextTwoSpaces () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 2
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardN playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(Board.StartSpot + 2, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.NextIntoNextColorArea () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 11
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardN  playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(1, endLocation.Spot)
        Assert.AreEqual(Color.Blue, endLocation.Color)

    [<TestMethod>]
    member this.NextIntoSafety () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 59
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot

        // act
        let endLocation = Location.ForwardN playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Safety, endLocation.Zone)
        Assert.AreEqual(1, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.NextIntoHome () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 64
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardN playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Home, endLocation.Zone)
        Assert.AreEqual(availableHomeSpot, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.NextIntoHomeSpot3 () =
        // arrange
        let availableHomeSpot = 3
        let forwardCount = 64
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardN playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Home, endLocation.Zone)
        Assert.AreEqual(availableHomeSpot, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.NextInvalid () =
        // arrange
        let availableHomeSpot = 1
        let forwardCount = 65
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.ForwardNOption playerColor availableHomeSpot forwardCount startLocation
        
        // assert
        Assert.IsTrue(endLocation.IsNone)

    [<TestMethod>]
    member this.BackWithinColor () =
        // arrange
        let backwardCount = 4
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.BackwardN backwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(1, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)

    [<TestMethod>]
    member this.BackIntoPreviousColor () =
        // arrange
        let backwardCount = 5
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Board Board.StartSpot
        // act
        let endLocation = Location.BackwardN backwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(Board.MaxBoardSpotsPerColor, endLocation.Spot)
        Assert.AreEqual(Color.Green, endLocation.Color)

    [<TestMethod>]
    member this.BackOutOfSafety () =
        // arrange
        let backwardCount = 1
        let playerColor = Color.Red
        let startLocation = Location.Create playerColor Zone.Safety 1
        // act
        let endLocation = Location.BackwardN backwardCount startLocation
        
        // assert
        Assert.AreEqual(Zone.Board, endLocation.Zone)
        Assert.AreEqual(Board.SafetyEntrySpot, endLocation.Spot)
        Assert.AreEqual(playerColor, endLocation.Color)
