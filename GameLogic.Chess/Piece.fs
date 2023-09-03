namespace GameLogic.Chess

type Type =
    | Pawn
    | Knight
    | Bishop
    | Rook
    | Queen
    | King

type Piece = Color * Type

module Types =
    let ForPromotion = [Type.Knight; Type.Bishop; Type.Rook; Type.Queen]

module Piece =
    let ColorOf (piece:Piece) = fst piece
    let TypeOf (piece:Piece) = snd piece
