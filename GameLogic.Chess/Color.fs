namespace GameLogic.Chess

type Color =
    | White
    | Black
    member this.Other =
        match this with
        | White -> Black
        | Black -> White
