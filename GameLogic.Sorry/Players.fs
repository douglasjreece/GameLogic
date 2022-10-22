namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Players =
    let ListFromColors (colorsInPlay: Colors) =
        [1..colorsInPlay.Length] |> List.map (fun x -> Player.Create x colorsInPlay.[x - 1])

    let Next (players: Players) (player: Player) =
        let orderedPlayers = players |> List.sortBy (fun x -> x.Number)
        let index = orderedPlayers |> List.findIndex (fun x -> x.Number = player.Number)
        if index < orderedPlayers.Length - 1 
            then orderedPlayers[index + 1]
            else orderedPlayers[0]
