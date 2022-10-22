namespace GameLogic.Sorry.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Sorry

[<TestClass>]
type GameStateTestsClass () =
    
    [<TestMethod>]
    member this.CardDrawn () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let initial = GameState.Create players pawnCount
        let state = { initial with Deck = [Card.Number 1]}

        // act
        let result = GameState.DrawCard state

        // assert
        let cardDrawn = 
            match result.Step with
            | CardDrawn s -> s
            | _ -> failwith "unexpected"
        Assert.AreEqual(1, cardDrawn.PotentialMoves.Length)
        Assert.AreEqual(Zone.Start, cardDrawn.PotentialMoves.[0].[0].From.Zone)
        Assert.AreEqual(Zone.Board, cardDrawn.PotentialMoves.[0].[0].To.Zone)

    [<TestMethod>]
    member this.NextPlayer () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let initial = GameState.Create players pawnCount
        let state1 = { initial with Deck = [Card.Number 1; Card.Number 2]}
        let state2 = GameState.DrawCard state1

        // act
        let cardDrawn = 
            match state2.Step with
            | CardDrawn s -> s
            | _ -> failwith "unexpected"
        let result = GameState.ApplyMoves players pawnCount (Some cardDrawn.PotentialMoves.[0]) state2

        // assert
        let playerUp = 
            match result.Step with
            | PlayerUp s -> s
            | _ -> failwith "unexpected"
        Assert.AreEqual(2, playerUp.Number)

    [<TestMethod>]
    member this.NextPlayerIsSame () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let initial = GameState.Create players pawnCount
        let state1 = { initial with Deck = [Card.Number 2; Card.Number 1]}
        let state2 = GameState.DrawCard state1

        // act
        let cardDrawn = 
            match state2.Step with
            | CardDrawn s -> s
            | _ -> failwith "unexpected"
        let result = GameState.ApplyMoves players pawnCount (Some cardDrawn.PotentialMoves.[0]) state2

        // assert
        let playerUp = 
            match result.Step with
            | PlayerUp s -> s
            | _ -> failwith "unexpected"
        Assert.AreEqual(1, playerUp.Number)

    [<TestMethod>]
    member this.ShuffleNeeded () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let initial = GameState.Create players pawnCount
        let state1 = { initial with Deck = [Card.Number 1]}
        let state2 = GameState.DrawCard state1

        // act
        let cardDrawn = 
            match state2.Step with
            | CardDrawn s -> s
            | _ -> failwith "unexpected"
        let result = GameState.ApplyMoves players pawnCount (Some cardDrawn.PotentialMoves.[0]) state2

        // assert
        let shuffleNeeded = 
            match result.Step with
            | ShuffleNeeded s -> s
            | _ -> failwith "unexpected"
        Assert.IsTrue(true)

    [<TestMethod>]
    member this.AfterShuffle () =
        // arrange
        let players = Players.ListFromColors Colors.All
        let pawnCount = 4
        let initial = GameState.Create players pawnCount
        let state1 = { initial with Deck = [Card.Number 1]}
        let state2 = GameState.DrawCard state1
        let cardDrawn = 
            match state2.Step with
            | CardDrawn s -> s
            | _ -> failwith "unexpected"
        let state3 = GameState.ApplyMoves players pawnCount (Some cardDrawn.PotentialMoves.[0]) state2

        // act
        let result = GameState.ShuffleDeck state3

        // assert
        // assert
        let playerUp = 
            match result.Step with
            | PlayerUp s -> s
            | _ -> failwith "unexpected"
        Assert.AreEqual(2, playerUp.Number)
        Assert.AreEqual(Cards.All.Length, result.Deck.Length)

