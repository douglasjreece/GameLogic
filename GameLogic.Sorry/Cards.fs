namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Cards = 
    let All = [1..Card.CountOfEachCard] |> List.collect (fun _ -> Card.Denomonations)

    let Shuffled (seed: int option) =
        All |> List.randomize seed