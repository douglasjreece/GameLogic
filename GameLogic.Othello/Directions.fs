namespace GameLogic.Othello

module Directions =
    open GameLogic.Library
    open Types

    let YDirections = [Up; Down; Neither]
    let XDirections = [Left; Right; Neither]

    let All = 
        YDirections 
        |> List.cartesian XDirections 
        |> List.filter (fun xdir_ydir -> (fst xdir_ydir) <> (snd xdir_ydir))

    let Opposite (direction: Direction * Direction) =
        match direction with
        | (Left, Up) -> (Right, Down)
        | (Right, Up) -> (Left, Down)
        | (Neither, Up) -> (Neither, Down)
        | (Left, Down) -> (Right, Up)
        | (Right, Down) -> (Left, Up)
        | (Neither, Down) -> (Neither, Up)
        | (Left, Neither) -> (Right, Neither)
        | (Right, Neither) -> (Left, Neither)
        | _ -> failwith "not implemented"
