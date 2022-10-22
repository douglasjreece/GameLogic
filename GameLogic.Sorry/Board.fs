namespace GameLogic.Sorry

module Board =
    let SafetyEntrySpot = 3
    let StartSpot = 5
    let MaxBoardSpotsPerColor = 15
    let MaxSafetySportsPerColor = 5
    let SlideAreas = 
        [
            { StartSpot = 2; EndSpot = 5 }
            { StartSpot = 10; EndSpot = 14 }
        ]
    let SlideAreaForSpot (spot: int) =
        let match_ = SlideAreas |> List.filter (fun x -> x.StartSpot = spot)
        match match_ with
        | h::t -> Some h
        | _ -> None

