namespace GameLogic.Othello.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open GameLogic.Othello
open GameLogic.Othello.Types

[<TestClass>]
type PositionsTestsClass () =

    [<TestMethod>]
    member this.EnclosingSquaresC3 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'C' 3
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Down) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresD3 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'D' 3
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Down, Direction.Neither) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresE3 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'E' 3
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Down) positions

        // assert
        Assert.AreEqual(2, result.Length)
        let xs = result |> List.map (fun x -> x.X) |> List.distinct
        Assert.AreEqual([square.X], xs)
        let ys = result |> List.map (fun x -> x.Y) |> List.distinct |> List.sort
        Assert.AreEqual([square.Y |> Square.IncY; square.Y |> Square.IncY |> Square.IncY], ys)

    [<TestMethod>]
    member this.EnclosingSquaresE4 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'E' 4
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Down) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresC4 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'C' 4
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Neither) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresF4 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'F' 4
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Left, Direction.Neither) positions

        // assert
        Assert.AreEqual(2, result.Length)
        let xs = result |> List.map (fun x -> x.X) |> List.distinct |> List.sort
        Assert.AreEqual([square.X |> Square.DecX |> Square.DecX; square.X |> Square.DecX], xs)
        let ys = result |> List.map (fun x -> x.Y) |> List.distinct |> List.sort
        Assert.AreEqual([square.Y], ys)

    [<TestMethod>]
    member this.EnclosingSquaresC5 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'C' 5
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Neither) positions

        // assert
        Assert.AreEqual(2, result.Length)
        let xs = result |> List.map (fun x -> x.X) |> List.distinct |> List.sort
        Assert.AreEqual([square.X |> Square.IncX; square.X |> Square.IncX |> Square.IncX], xs)
        let ys = result |> List.map (fun x -> x.Y) |> List.distinct |> List.sort
        Assert.AreEqual([square.Y], ys)

    [<TestMethod>]
    member this.EnclosingSquaresF5 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'F' 5
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Left, Direction.Neither) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresC6 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'C' 6
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Up) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresD6 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'D' 6
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Up) positions

        // assert
        Assert.AreEqual(2, result.Length)
        let xs = result |> List.map (fun x -> x.X) |> List.distinct |> List.sort
        Assert.AreEqual([square.X], xs)
        let ys = result |> List.map (fun x -> x.Y) |> List.distinct |> List.sort
        Assert.AreEqual([square.Y |> Square.DecY |> Square.DecY; square.Y |> Square.DecY], ys)

    [<TestMethod>]
    member this.EnclosingSquaresE6 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'E' 6
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Up) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresF6 () =
        // arrange
        let positions = Positions.InitialSetup
        let square = Square.At 'F' 6
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Left, Direction.Up) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1RightNone () =
        // arrange
        let locations = List.cartesian ['B'..'H'] [1]
        let positions = Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Neither) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1RightAll () =
        // arrange
        let locations = List.cartesian ['B'..'G'] [1]
        let positions = Position.Of (Square.At 'H' 1) Color.White :: Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Neither) positions

        // assert
        Assert.AreEqual(7, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1DownNone () =
        // arrange
        let locations = List.cartesian ['A'] [2..8]
        let positions = Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Down) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1DownAll () =
        // arrange
        let locations = List.cartesian ['A'] [2..7]
        let positions = Position.Of (Square.At 'A' 8) Color.White :: Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Neither, Direction.Down) positions

        // assert
        Assert.AreEqual(7, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1DiagonalNone () =
        // arrange
        let locations = List.zip ['B'..'H'] [2..8]
        let positions = Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Down) positions

        // assert
        Assert.AreEqual(0, result.Length)

    [<TestMethod>]
    member this.EnclosingSquaresA1DiagonalAll () =
        // arrange
        let locations = List.zip ['B'..'G'] [2..7]
        let positions = Position.Of (Square.At 'H' 8) Color.White :: Positions.AddXYPositions locations Color.Black []
        let square = Square.At 'A' 1
        let color = Color.White
        let position = Position.Of square color

        // act
        let result = Positions.EnclosingSquares position (Direction.Right, Direction.Down) positions

        // assert
        Assert.AreEqual(7, result.Length)


    [<TestMethod>]
    member this.PotentialPlaysWhite () =
        // arrange
        let positions = Positions.InitialSetup

        // act
        let result = Positions.PotentialPlays positions Color.White |> Positions.Squares |>  List.sortBy (fun x -> x.X, x.Y)

        // assert
        Assert.AreEqual([Square.At 'C' 5; Square.At 'D' 6; Square.At 'E' 3; Square.At 'F' 4], result)

    [<TestMethod>]
    member this.PotentialPlaysBlack () =
        // arrange
        let positions = Positions.InitialSetup

        // act
        let result = Positions.PotentialPlays positions Color.Black |> Positions.Squares |> List.sortBy (fun x -> x.X, x.Y)

        // assert
        Assert.AreEqual([Square.At 'C' 4; Square.At 'D' 3; Square.At 'E' 6; Square.At 'F' 5], result)
