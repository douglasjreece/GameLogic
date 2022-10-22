namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Colors =
    let All = [Color.Red; Color.Blue; Color.Yellow; Color.Green]

    let InPlay (selectedColors: Colors) (firstPlayerColor: Color) =
        let orderedColors = selectedColors |> List.sortBy (fun x -> x.AsOrdinal)
        let firstPlayerColorIndex = orderedColors |> List.findIndex (fun x -> x = firstPlayerColor)
        let colorsInPlay = [0..firstPlayerColorIndex-1] |> List.fold (fun acc x -> acc.Tail @ [acc.Head]) orderedColors
        colorsInPlay

