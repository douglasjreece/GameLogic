namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type CardDrawnStateTestsClass () =

    [<TestMethod>]
    member this.MovesFor1CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor1Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(Location.ExitStart player.Color, result.[0].[0].To)
        Assert.AreEqual(player.Color, result.[0].[0].To.Color)

    [<TestMethod>]
    member this.MovesFor1CardOneInStartExit () =
        // arrange
        let card = Card.Number 1
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 1
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor1Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor1CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 1)

        // act
        let result = CardDrawnState.MovesFor1Card player.Color positions

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(1, result.[1].Length)
        let move1 = result.[0].[0]
        let move2 = result.[1].[0]
        Assert.AreNotEqual(move1.Pawn, move2.Pawn)
        Assert.IsTrue(move1.Pawn = pawn1 || move1.Pawn = pawn2)
        Assert.IsTrue(move2.Pawn = pawn1 || move2.Pawn = pawn2)

    [<TestMethod>]
    member this.MovesFor2CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor2Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(Location.ExitStart player.Color, result.[0].[0].To)
        Assert.AreEqual(player.Color, result.[0].[0].To.Color)

    [<TestMethod>]
    member this.MovesFor2CardOneInStartExit () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor2Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor2CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 1)

        // act
        let result = CardDrawnState.MovesFor2Card player.Color positions

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(1, result.[1].Length)
        let move1 = result.[0].[0]
        let move2 = result.[1].[0]
        Assert.AreNotEqual(move1.Pawn, move2.Pawn)
        Assert.IsTrue(move1.Pawn = pawn1 || move1.Pawn = pawn2)
        Assert.IsTrue(move2.Pawn = pawn1 || move2.Pawn = pawn2)

    [<TestMethod>]
    member this.MovesFor3CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor3Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor3CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 3
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor3Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor4CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor4Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor4CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Decrement 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor4Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor5CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor5Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor5CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 5
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor5Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor7CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor7Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor7CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 7
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor7Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor7CardTwoPawnsOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Decrement 4)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 4)

        // act
        let result = CardDrawnState.MovesFor7Card player.Color positions

        // assert
        Assert.AreEqual(8, result.Length)
        let singleMoves = result |> List.filter (fun x -> x.Length = 1)
        let maxSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.max
        let minSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.min
        Assert.AreEqual(7, maxSingleDistance)
        Assert.AreEqual(7, minSingleDistance)
        let multiMoves = result |> List.filter (fun x -> x.Length = 2)
        Assert.AreEqual(6, multiMoves.Length)
        let maxMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.max
        let minMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.min
        Assert.AreEqual(7, maxMultiDistance)
        Assert.AreEqual(7, minMultiDistance)

    [<TestMethod>]
    member this.MovesFor7CardThreePawnsOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let pawn3 = Pawn.Create player.Color 3
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Decrement 4)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 11)
                        |> Positions.SetPosition (Position.CreateStartExit pawn3 |> Position.Increment (11 + 15))

        // act
        let result = CardDrawnState.MovesFor7Card player.Color positions

        // assert
        Assert.AreEqual(21, result.Length)
        let singleMoves = result |> List.filter (fun x -> x.Length = 1)
        let maxSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.max
        let minSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.min
        Assert.AreEqual(7, maxSingleDistance)
        Assert.AreEqual(7, minSingleDistance)
        let multiMoves = result |> List.filter (fun x -> x.Length > 1)
        Assert.AreEqual(18, multiMoves.Length)
        let maxMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.max
        let minMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.min
        Assert.AreEqual(7, maxMultiDistance)
        Assert.AreEqual(7, minMultiDistance)

    [<TestMethod>]
    member this.MovesFor7CardTwoPawnsOnBoardWithConflict () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Decrement 4)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Decrement 3)

        // act
        let result = CardDrawnState.MovesFor7Card player.Color positions

        // assert
        Assert.AreEqual(7, result.Length)
        let singleMoves = result |> List.filter (fun x -> x.Length = 1)
        let maxSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.max
        let minSingleDistance = singleMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To)) |> List.min
        Assert.AreEqual(7, maxSingleDistance)
        Assert.AreEqual(7, minSingleDistance)
        let multiMoves = result |> List.filter (fun x -> x.Length = 2)
        Assert.AreEqual(5, multiMoves.Length)
        let maxMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.max
        let minMultiDistance = multiMoves |> List.map (fun x -> (Location.DistanceBetween x.[0].From x.[0].To) + (Location.DistanceBetween x.[1].From x.[1].To)) |> List.min
        Assert.AreEqual(7, maxMultiDistance)
        Assert.AreEqual(7, minMultiDistance)

    [<TestMethod>]
    member this.MovesFor8CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor8Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor8CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 8
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor8Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor10CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor10Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor10CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnForwardEndPosition = pawnStartPosition |> Position.Increment 10
        let pawnBackwardEndPosition = pawnStartPosition |> Position.Decrement 1
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor10Card player.Color positions

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnForwardEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)
        Assert.AreEqual(pawnBackwardEndPosition.Location, result.[1].[0].To)

    [<TestMethod>]
    member this.MovesFor10CardOnePawnOnBoardCloseToHome () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn |> Position.Decrement 2
        let pawnBackwardEndPosition = pawnStartPosition |> Position.Decrement 1
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor10Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)
        Assert.AreEqual(pawnBackwardEndPosition.Location, result.[0].[0].To)

    [<TestMethod>]
    member this.MovesFor11CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor11Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor11CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 11
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor11Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor11CardTwoPawnsOnBoardSameColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn1StartPosition = Position.CreateStartExit pawn1
        let pawn1EndPosition = pawn1StartPosition |> Position.Increment 11
        let pawn2 = Pawn.Create player.Color 2
        let pawn2StartPosition = Position.CreateStartExit pawn2 |> Position.Increment 2
        let pawn2EndPosition = pawn2StartPosition |> Position.Increment 11
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawn1StartPosition
                        |> Positions.SetPosition pawn2StartPosition

        // act
        let result = CardDrawnState.MovesFor11Card player.Color positions

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawn1EndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn1, result.[0].[0].Pawn)
        Assert.AreEqual(pawn2EndPosition.Location, result.[1].[0].To)
        Assert.AreEqual(pawn2, result.[1].[0].Pawn)

    [<TestMethod>]
    member this.MovesFor11CardTwoPawnsOnBoardDifferentColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn1StartPosition = Position.CreateStartExit pawn1
        let pawn2 = Pawn.Create player2.Color 1
        let pawn2StartPosition = Position.CreateStartExit pawn2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawn1StartPosition
                        |> Positions.SetPosition pawn2StartPosition

        // act
        let result = CardDrawnState.MovesFor11Card player1.Color positions

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(2, result.[1].Length)
        Assert.AreEqual(pawn2StartPosition.Location, result.[1].[0].To)
        Assert.AreEqual(pawn1, result.[1].[0].Pawn)
        Assert.AreEqual(pawn1StartPosition.Location, result.[1].[1].To)
        Assert.AreEqual(pawn2, result.[1].[1].Pawn)

    [<TestMethod>]
    member this.MovesFor12CardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesFor8Card player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesFor12CardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn |> Position.Decrement 1
        let pawnEndPosition = pawnStartPosition |> Position.Increment 12
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesFor12Card player.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(1, result.[0].Length)
        Assert.AreEqual(pawnEndPosition.Location, result.[0].[0].To)
        Assert.AreEqual(pawn, result.[0].[0].Pawn)

    [<TestMethod>]
    member this.MovesForSorryCardAllInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let positions = Positions.Initial players pawnCount

        // act
        let result = CardDrawnState.MovesForSorryCard player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesForSorryCardOnePawnOnBoard () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let pawnEndPosition = pawnStartPosition |> Position.Increment 11
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.MovesForSorryCard player.Color positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.MovesForSorryCardOnePawnOnBoardOtherColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn2 = Pawn.Create player2.Color 1
        let pawn2StartPosition = Position.CreateStartExit pawn2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawn2StartPosition

        // act
        let result = CardDrawnState.MovesForSorryCard player1.Color positions

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(pawn2StartPosition.Location, result.[0].[0].To)
        Assert.AreEqual(Zone.Start, result.[0].[1].To.Zone)

    [<TestMethod>]
    member this.Card1 () =
        // arrange
        let card = Card.Number 1
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(1, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)
        Assert.AreEqual(player, result.Player)
        Assert.AreEqual(card, result.Card)

    [<TestMethod>]
    member this.Card2 () =
        // arrange
        let card = Card.Number 2
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(2, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card3 () =
        // arrange
        let card = Card.Number 3
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(3, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card4 () =
        // arrange
        let card = Card.Number 4
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(4, Location.DistanceBetween result.PotentialMoves.[0].[0].To result.PotentialMoves.[0].[0].From)

    [<TestMethod>]
    member this.Card5 () =
        // arrange
        let card = Card.Number 5
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(5, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card7 () =
        // arrange
        let card = Card.Number 7
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(7, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card8 () =
        // arrange
        let card = Card.Number 8
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(8, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card10 () =
        // arrange
        let card = Card.Number 10
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(2, result.PotentialMoves.Length)
        Assert.AreEqual(10, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card11 () =
        // arrange
        let card = Card.Number 11
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(11, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.Card12 () =
        // arrange
        let card = Card.Number 12
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnStartPosition = Position.CreateStartExit pawn
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawnStartPosition

        // act
        let result = CardDrawnState.Create player card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(12, Location.DistanceBetween result.PotentialMoves.[0].[0].From result.PotentialMoves.[0].[0].To)

    [<TestMethod>]
    member this.CardSorry () =
        // arrange
        let card = Card.Sorry
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn2 = Pawn.Create player2.Color 1
        let pawn2StartPosition = Position.CreateStartExit pawn2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition pawn2StartPosition

        // act
        let result = CardDrawnState.Create player1 card positions

        // assert
        Assert.AreEqual(1, result.PotentialMoves.Length)
        Assert.AreEqual(pawn2StartPosition.Location, result.PotentialMoves.[0].[0].To)
