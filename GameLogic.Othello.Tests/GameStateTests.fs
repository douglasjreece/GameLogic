namespace GameLogic.Othello.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Othello
open GameLogic.Othello.Types

[<TestClass>]
type GameStateTestsClass () =

    [<TestMethod>]
    member this.FirstPlay () =
        // arrange
        let state = GameState.Initial

        // act
        let playerUp = state.Step.AsPlayerUp
        let potentialPlays = playerUp.PotentialPlays |> Positions.Sort
        let play = potentialPlays.Head.Square
        let result = state |> GameState.ApplyPlay (Some play)

        // assert
        let blackCount = result.Positions |> Positions.ColorCount Color.Black
        let whiteCount = result.Positions |> Positions.ColorCount Color.White
        Assert.AreEqual(4, blackCount)
        Assert.AreEqual(1, whiteCount)
        Assert.AreEqual(Color.White, result.Step.AsPlayerUp.Player.Color)

    [<TestMethod>]
    member this.WhiteNoPlay () =
        // arrange
        let positions = Positions.AddTriplesPositions
                            [
                                ('B', 1, Color.White); ('C', 1, Color.Black); ('D', 1, Color.Black); ('E', 1, Color.Black); ('F', 1, Color.Black); ('G', 1, Color.Black); ('H', 1, Color.Black)
                                ('B', 8, Color.White); ('C', 8, Color.Black); ('D', 8, Color.Black); ('E', 8, Color.Black); ('F', 8, Color.Black); ('G', 8, Color.Black); ('H', 8, Color.Black)
                            ] []
        let potentialPlays = Positions.PotentialPlays positions Color.Black |> Positions.Sort

        let state = GameState.Of positions (Step.PlayerUp (PlayerStep.Of (Player.Of Color.Black) potentialPlays))

        // act
        let playerUp = state.Step.AsPlayerUp
        let potentialPlays = playerUp.PotentialPlays |> Positions.Sort
        let play = potentialPlays.Head.Square
        let result = state |> GameState.ApplyPlay (Some play)

        // assert
        let blackCount = result.Positions |> Positions.ColorCount Color.Black
        let whiteCount = result.Positions |> Positions.ColorCount Color.White
        Assert.AreEqual(14, blackCount)
        Assert.AreEqual(1, whiteCount)
        Assert.AreEqual(Color.White, result.Step.AsPlayerUp.Player.Color)

    [<TestMethod>]
    member this.GameOverWinner () =
        // arrange
        let positions = Positions.AddTriplesPositions
                            [
                                ('B', 1, Color.White); ('C', 1, Color.Black); ('D', 1, Color.Black); ('E', 1, Color.Black); ('F', 1, Color.Black); ('G', 1, Color.Black); ('H', 1, Color.Black)
                            ] []
        let potentialPlays = Positions.PotentialPlays positions Color.Black |> Positions.Sort

        let state = GameState.Of positions (Step.PlayerUp (PlayerStep.Of (Player.Of Color.Black) potentialPlays))

        // act
        let playerUp = state.Step.AsPlayerUp
        let potentialPlays = playerUp.PotentialPlays |> Positions.Sort
        let play = potentialPlays.Head.Square
        let result = state |> GameState.ApplyPlay (Some play)

        // assert
        let blackCount = result.Positions |> Positions.ColorCount Color.Black
        let whiteCount = result.Positions |> Positions.ColorCount Color.White
        Assert.AreEqual(8, blackCount)
        Assert.AreEqual(0, whiteCount)
        let gameOver = result.Step.AsGameOver
        Assert.AreEqual(Color.Black, gameOver.Winner.Value.Color)

    [<TestMethod>]
    member this.GameOverTie () =
        // arrange
        let positions = Positions.AddTriplesPositions
                            [
                                ('A', 1, Color.Black); ('B', 1, Color.White); ('C', 1, Color.White);   ('E', 1, Color.White); ('F', 1, Color.White); ('G', 1, Color.White); ('H', 1, Color.White)
                            ] []
        let potentialPlays = Positions.PotentialPlays positions Color.Black |> Positions.Sort

        let state = GameState.Of positions (Step.PlayerUp (PlayerStep.Of (Player.Of Color.Black) potentialPlays))

        // act
        let playerUp = state.Step.AsPlayerUp
        let potentialPlays = playerUp.PotentialPlays |> Positions.Sort
        let play = potentialPlays.Head.Square
        let result = state |> GameState.ApplyPlay (Some play)

        // assert
        let blackCount = result.Positions |> Positions.ColorCount Color.Black
        let whiteCount = result.Positions |> Positions.ColorCount Color.White
        Assert.AreEqual(4, blackCount)
        Assert.AreEqual(4, whiteCount)
        let gameOver = result.Step.AsGameOver
        Assert.IsFalse(gameOver.Winner.IsSome)
        