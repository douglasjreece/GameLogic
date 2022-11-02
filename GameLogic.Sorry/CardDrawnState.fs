namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CardDrawnState =
    open GameLogic.Library

    let MovesForNumber (color: Color) (number: int) (positions: Positions): MovesList =
        let moves = (Positions.ValidDirectionMoves positions color number)
        let moveAndSideEffects = Positions.MoveAndSideEffects positions
        let result = moves |> List.map moveAndSideEffects
        result

    let MovesFor1Card (color: Color) (positions: Positions): MovesList = 
        let startMove = Positions.ValidStartMove positions color
        let inPlayMoves = (Positions.ValidForwardMoves positions color 1)
        let moves = startMove @ inPlayMoves
        let result = moves |> Positions.MovesAndSideEffects positions
        result

    let MovesFor2Card (color: Color) (positions: Positions): MovesList = 
        let startMove = Positions.ValidStartMove positions color
        let inPlayMoves = (Positions.ValidForwardMoves positions color 2)
        let moves = startMove @ inPlayMoves
        let result = moves |> Positions.MovesAndSideEffects positions
        result

    let MovesFor3Card (color: Color) (positions: Positions): MovesList = 
        MovesForNumber color 3 positions

    let MovesFor4Card (color: Color) (positions: Positions): MovesList = 
        MovesForNumber color -4 positions

    let MovesFor5Card (color: Color) (positions: Positions): MovesList = 
        MovesForNumber color 5 positions

    let MovesFor7Card (color: Color) (positions: Positions): MovesList =
        let movesForSplitTups (count1: int) =
            let count2 = 7 - count1
            let moves1 = Positions.ValidForwardMoves positions color count1
            let moves2 = 
                let movesAux (move: Move) =
                    let positionsAux = positions |> Positions.ApplyMove move
                    let movesAux = Positions.ValidForwardMoves positionsAux color count2
                    let result = movesAux |> List.filter (fun move -> move.Pawn <> move.Pawn)
                    result
                moves1 |> List.collect (fun x -> movesAux x)
            let combinedMovesDups = moves1 |> List.cartesian moves2
            let combinedMoves = combinedMovesDups |> List.filter (fun move_move -> (fst move_move).Pawn <> (snd move_move).Pawn) |> List.distinct
            combinedMoves
        let splits = [1..3] // 1,6 2,5 3,4
        let movesForSplitsTups = splits |> List.map movesForSplitTups
        //let z = movesForSplitsTups.[0] |> List.distinct
        let extractSplits (lst: (Move * Move) list) =
            lst |> List.map (fun move_move -> (Positions.MoveAndSideEffects positions (fst move_move)) @ (Positions.MoveAndSideEffects positions (snd move_move)))
        let movesAndEffectsForSplits = movesForSplitsTups |> List.collect (fun move_move -> extractSplits move_move)
        let movesFor7 = Positions.ValidForwardMoves positions color 7
        let movesAndEffectsFor7 = Positions.MovesAndSideEffects positions movesFor7
        let result = movesAndEffectsFor7 @ movesAndEffectsForSplits
        result

    let MovesFor8Card (color: Color) (positions: Positions): MovesList =
        MovesForNumber color 8 positions

    let MovesFor10Card (color: Color) (positions: Positions): MovesList =
        let movesFor10 = MovesForNumber color 10 positions
        let movesForBack1 = MovesForNumber color -1 positions
        let result = movesFor10 @ movesForBack1
        result

    let MovesFor11Card (color: Color) (positions: Positions): MovesList =
        let forwardMoves = Positions.ValidForwardMoves positions color 11
        let colorBoardPositions = positions |> Positions.ForPawnColor color |> Positions.OnBoard
        let otherColorBoardPositions = positions |> Positions.ExcludePawnColor color |> Positions.OnBoard
        let swapPositionsTups = List.cartesian colorBoardPositions otherColorBoardPositions
        let swapMoves = swapPositionsTups |> List.map (fun position_position -> Move.CreateSwap (fst position_position).Pawn (fst position_position).Location (snd position_position).Location)
        let moves = forwardMoves @ swapMoves
        let result = moves |> Positions.MovesAndSideEffects positions
        result

    let MovesFor12Card (color: Color) (positions: Positions): MovesList =
        MovesForNumber color 12 positions

    let MovesForSorryCard (color: Color)  (positions: Positions): MovesList =
        let positionsInStart = positions |> Positions.ForPawnColor color |> Positions.InStart
        match positionsInStart with
        | startPosition::_ -> 
                let otherColorPositions = positions |> Positions.ExcludePawnColor color |> Positions.InPlay
                let createMove = Move.Create startPosition.Pawn startPosition.Location
                let moves = otherColorPositions |> Positions.Locations |> List.map createMove
                let result = moves |> Positions.MovesAndSideEffects positions
                result
        | _ -> []


    let Create (player: Player) (card: Card) (positions: Positions): CardDrawnState =
        let potentialMoves =
            match card with
            | Number number -> 
                match number with
                | 1 -> MovesFor1Card player.Color positions
                | 2 -> MovesFor2Card player.Color positions
                | 3 -> MovesFor3Card player.Color positions
                | 4 -> MovesFor4Card player.Color  positions
                | 5 -> MovesFor5Card player.Color positions
                | 7 -> MovesFor7Card player.Color positions
                | 8 -> MovesFor8Card player.Color positions
                | 10 -> MovesFor10Card player.Color positions
                | 11 -> MovesFor11Card player.Color positions
                | 12 -> MovesFor12Card player.Color positions
                | _ -> raise (System.NotImplementedException("not implemented"))
            | Sorry -> MovesForSorryCard player.Color positions
        {
            Player = player
            Card = card
            PotentialMoves = potentialMoves
        }

