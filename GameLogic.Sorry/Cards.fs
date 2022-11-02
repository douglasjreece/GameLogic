namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Cards = 
    open GameLogic.Library

    let All = [1..Card.CountOfEachCard] |> List.collect (fun _ -> Card.Denomonations)

    let Shuffled (seed: int option) =
        All |> List.randomize seed