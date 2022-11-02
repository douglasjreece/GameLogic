namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type PositionsTestsClass () =

    [<TestMethod>]
    member this.Initial () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let playerCount = players.Length
        let pawnCount = 4

        // act
        let initial = Positions.Initial players pawnCount
        let zones = initial |> List.groupBy (fun x -> x.Location.Zone)

        // assert
        Assert.AreEqual(pawnCount * playerCount, initial.Length)
        Assert.AreEqual(initial.Length, (snd zones.[0]).Length)
        Assert.AreEqual(Zone.Start, fst zones.[0])

    [<TestMethod>]
    member this.ValidMovesNone () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4

        // act
        let positions = Positions.Initial players pawnCount
        let moves = Positions.ValidDirectionMoves positions Color.Red 1

        // assert
        Assert.AreEqual(0, moves.Length)

    [<TestMethod>]
    member this.ValidMoves_OnePawnOneSpotForward () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnCount = 4
        let initial = Positions.Initial players pawnCount |> Positions.ForPawnColor player.Color

        // act
        let positions = Positions.ApplyMove (Move.CreateStartExit pawn) initial
        let moves = Positions.ValidDirectionMoves positions player.Color 1

        // assert
        Assert.AreEqual(1, moves.Length)
        Assert.AreEqual(Zone.Board, moves.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot + 1, moves.[0].To.Spot)

    [<TestMethod>]
    member this.ValidMoves_OnePawnOneSpotBackward () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn = Pawn.Create player.Color 1
        let pawnCount = 4
        let initial = Positions.Initial players pawnCount |> Positions.ForPawnColor player.Color

        // act
        let positions = Positions.ApplyMove (Move.CreateStartExit pawn) initial
        let moves = Positions.ValidDirectionMoves positions player.Color -1

        // assert
        Assert.AreEqual(1, moves.Length)
        Assert.AreEqual(Zone.Board, moves.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot - 1, moves.[0].To.Spot)

    [<TestMethod>]
    member this.ValidMoves_TwoPawnOneSpot () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit  pawn1)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 2)

        // act
        let result = Positions.ValidDirectionMoves positions player.Color 1 |> Move.Sort

        // assert
        Assert.AreEqual(2, result.Length)
        Assert.AreEqual(Zone.Board, result.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot + 1, result.[0].To.Spot)
        Assert.AreEqual(Board.StartSpot + 3, result.[1].To.Spot)

    [<TestMethod>]
    member this.ValidMoves_TwoPawnOneSpotWithConflict () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit  pawn1)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 1)

        // act
        let result = Positions.ValidDirectionMoves positions player.Color 1

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(Zone.Board, result.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot + 2, result.[0].To.Spot)

    [<TestMethod>]
    member this.ValidMoves_TwoPawnOneSpotWithConflictOtherColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 2
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit  pawn1)
                        |> Positions.SetPosition (Position.Create pawn2 (Location.ExitStart pawn1.Color) |> Position.Increment 1)

        // act
        let result = Positions.ValidDirectionMoves positions player1.Color 1

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(Zone.Board, result.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot + 1, result.[0].To.Spot)

    [<TestMethod>]
    member this.ValidMoves_OnePawnMultipleSpotsInvalid () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Decrement 3)

        // act
        let result = Positions.ValidDirectionMoves positions player.Color 8

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.ValidStartMove () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount

        // act
        let result = Positions.ValidStartMove positions player.Color

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(Zone.Board, result.[0].To.Zone)
        Assert.AreEqual(Board.StartSpot, result.[0].To.Spot)

    [<TestMethod>]
    member this.ValidStartNoneWithConflict () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1)

        // act
        let result = Positions.ValidStartMove positions player.Color

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.ValidStartNoneInStart () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player = players.Head
        let pawn1 = Pawn.Create player.Color 1
        let pawn2 = Pawn.Create player.Color 2
        let pawn3 = Pawn.Create player.Color 3
        let pawn4 = Pawn.Create player.Color 4
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 2)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 3)
                        |> Positions.SetPosition (Position.CreateStartExit pawn3 |> Position.Increment 4)
                        |> Positions.SetPosition (Position.CreateStartExit pawn4 |> Position.Increment 5)


        // act
        let result = Positions.ValidStartMove positions player.Color

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.SideEffectsNone () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawnCount = 4
        let pawn1ExitMove = Move.CreateStartExit pawn1
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 1)

        // act
        let result = Positions.SideEffectMoves positions pawn1ExitMove

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.SideEffectsOne () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 1
        let pawnCount = 4
        let pawn2ExitMove = Move.CreateStartExit pawn2
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 15)

        // act
        let result = Positions.SideEffectMoves positions pawn2ExitMove

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(pawn1, result.[0].Pawn)

    [<TestMethod>]
    member this.SideEffectsOnSlideSameColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1)

        // act
        let moveToSlideHead = Move.CreateStartExit pawn1 |> Move.NextDirection 5
        let result = Positions.SideEffectMoves positions moveToSlideHead

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.SideEffectsOnSlide () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 1
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 28)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2)

        // act
        let moveToSlideHead = Move.CreateStartExit pawn2 |> Move.NextDirection 12 // move to head of yellow slide
        let result = Positions.SideEffectMoves positions moveToSlideHead

        // assert
        Assert.AreEqual(2, result.Length)
        let slideEffectMove = result |> List.find (fun x -> x.Type = MoveType.SideEffect)
        let slideMove = result |> List.find (fun x -> x.Type = MoveType.Slide)
        Assert.AreEqual(pawn1, slideEffectMove.Pawn)
        Assert.AreEqual(pawn2, slideMove.Pawn)
        Assert.AreEqual(Board.SlideAreas.[0].EndSpot, slideMove.To.Spot)

    [<TestMethod>]
    member this.SideEffectsOnSlideWithSameColor () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 1
        let pawn3 = Pawn.Create player2.Color 2
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 28)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 14)
                        |> Positions.SetPosition (Position.CreateStartExit pawn3)

        // act
        let moveToSlideHead = Move.CreateStartExit pawn3 |> Move.NextDirection 12 // move to head of blue slide
        let result = Positions.SideEffectMoves positions moveToSlideHead

        // assert
        Assert.AreEqual(3, result.Length)
        let slideEffectMoves = result |> List.filter (fun x -> x.Type = MoveType.SideEffect)
        let slideMove = result |> List.find (fun x -> x.Type = MoveType.Slide)
        Assert.AreEqual(2, slideEffectMoves.Length)
        Assert.IsTrue(slideEffectMoves |> List.exists (fun x -> x.Pawn = pawn1))
        Assert.IsTrue(slideEffectMoves |> List.exists (fun x -> x.Pawn = pawn2))
        Assert.AreEqual(pawn3, slideMove.Pawn)

    [<TestMethod>]
    member this.SideEffectsForSwap () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 1
        let pawnCount = 4
        let pawn2Location = Location.ExitStart pawn1.Color
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2)

        // act
        let move = Move.CreateSwap pawn1 (Location.ExitStart pawn1.Color) (Location.ExitStart pawn2.Color)
        let result = Positions.SideEffectMoves positions move

        // assert
        Assert.AreEqual(1, result.Length)
        Assert.AreEqual(pawn2, result.[0].Pawn)
        Assert.AreEqual(pawn2Location, result.[0].To)


    [<TestMethod>]
    member this.SideEffectsForSwapOnSlide () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let player2 = players.Tail.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn2 = Pawn.Create player2.Color 1
        let pawnCount = 4
        let pawn1Location = Location.SlideArea2Start pawn1.Color
        let pawn2Location = Location.SlideArea2Start pawn2.Color
        let positions = Positions.Initial players pawnCount
                        |> Positions.SetPosition (Position.CreateStartExit pawn1 |> Position.Increment 5)
                        |> Positions.SetPosition (Position.CreateStartExit pawn2 |> Position.Increment 5)

        // act
        let move = Move.CreateSwap pawn1 pawn1Location pawn2Location
        let result = Positions.SideEffectMoves positions move |> Move.Sort

        // assert
        Assert.AreEqual(3, result.Length)
        Assert.AreEqual(pawn1, result.[0].Pawn)
        Assert.AreEqual(pawn2, result.[1].Pawn)
        Assert.AreEqual(pawn2, result.[2].Pawn)
        Assert.AreEqual(player1.Color, result.[1].To.Color)
        Assert.AreEqual(player1.Color, result.[2].To.Color)
        Assert.AreEqual(Board.SlideAreas[1].EndSpot, result.[2].To.Spot)

    [<TestMethod>]
    member this.ApplyMove () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount

        // act
        let move = Move.CreateStartExit pawn1
        let result = positions |> Positions.ApplyMove move

        // assert
        let pawnResult = Positions.ForPawn pawn1 result
        Assert.AreEqual(Zone.Board, pawnResult.Location.Zone)
        Assert.AreEqual(Board.StartSpot, pawnResult.Location.Spot)


    [<TestMethod>]
    member this.ApplyMoves () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let player1 = players.Head
        let pawn1 = Pawn.Create player1.Color 1
        let pawn1StartPosition = Position.CreateStartExit pawn1
        let pawn1EndPosition = pawn1StartPosition |> Position.Increment 2
        let pawn2 = Pawn.Create player1.Color 2
        let pawn2StartPosition = Position.CreateStartExit pawn2 |> Position.Increment 5
        let pawn2EndPosition = pawn2StartPosition |> Position.Increment 3
        let pawnCount = 4
        let positions = Positions.Initial players pawnCount
                            |> Positions.SetPosition pawn1StartPosition
                            |> Positions.SetPosition pawn2StartPosition
        // act
        let moves = 
            [
                Move.Create pawn1 pawn1StartPosition.Location pawn1EndPosition.Location
                Move.Create pawn2 pawn2StartPosition.Location pawn2EndPosition.Location
            ]
        let result = Positions.ApplyMoves moves positions

        // assert
        let pawn1Result = Positions.ForPawn pawn1 result
        let pawn2Result = Positions.ForPawn pawn2 result
        Assert.AreEqual(pawn1EndPosition.Location, pawn1Result.Location)
        Assert.AreEqual(pawn2EndPosition.Location, pawn2Result.Location)

