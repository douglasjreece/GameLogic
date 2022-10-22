namespace GameLogic.Sorry

exception LocationError of string

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Location =
    let Create (color: Color) (zone: Zone) (spot: int): Location = { Color = color; Zone = zone; Spot = spot }

    let Forward (playerColor: Color) (openHomeSpot: int) (location: Location) =
        match location.Zone with
        | Zone.Start -> 
            raise (LocationError("Cannot move forward in Start."))
        | Zone.Board -> 
            match location.Spot with
            | spot when spot = Board.SafetyEntrySpot ->
                    match location.Color with
                    | color when color = playerColor -> { location with Zone = Zone.Safety; Spot = 1 }
                    | _ -> { location with Spot = spot + 1 }
            | spot when spot < Board.MaxBoardSpotsPerColor -> { location with Spot = spot + 1 }
            | _ -> { location with Color = location.Color.Next; Spot = 1 }
        | Zone.Safety -> 
            match location.Spot with
            | spot when spot < Board.MaxSafetySportsPerColor -> { location with Spot = spot + 1 }
            | _ -> { location with Zone = Zone.Home; Spot = openHomeSpot }
        | Zone.Home -> 
            raise (LocationError("Cannot move forward in Home."))

    let ForwardNOption (playerColor: Color) (openHomeSpot: int) (count: int) (location: Location) =
        let rec aux acc cnt =
            if cnt > 0 
                then 
                    match acc.Zone with
                    | Zone.Board | Zone.Safety -> aux (Forward playerColor openHomeSpot acc) (cnt - 1)
                    | _ -> None
                else Some acc
        aux location count
    
    let ForwardN (playerColor: Color) (openHomeSpot: int) (count: int) (location: Location) =
        (ForwardNOption playerColor openHomeSpot count location).Value

    let Backward (location: Location) =
        match location.Zone with
        | Zone.Start -> 
            raise (LocationError("Backward within Zone.Start is not valid."))
        | Zone.Board -> 
            match location.Spot with
            | spot when spot > 1 -> { location with Spot = spot - 1 }
            | _ -> { location with Color = location.Color.Previous; Spot = Board.MaxBoardSpotsPerColor }
        | Zone.Safety ->
            { location with Zone = Zone.Board; Spot = Board.SafetyEntrySpot }
        | Zone.Home ->
            raise (LocationError("Backward within Zone.Home is not valid."))

    let BackwardN (count: int) (location: Location) =
        let rec aux acc cnt =
            if cnt > 0 
                then aux (Backward acc) (cnt - 1)
                else acc
        aux location count
                
    let DirectionN (playerColor: Color) (openHomeSpot: int) (count: int) (location: Location) =
        match count with
        | c when c > 0 -> ForwardN playerColor openHomeSpot count location
        | c when c < 0 -> BackwardN (-count) location
        | _ -> raise (LocationError("Direction move must not be 0."))
        
    let ExitStart (color: Color) = { Color = color; Zone = Zone.Board; Spot = Board.StartSpot }

    let Start (color: Color) (spot: int) = { Color = color; Zone = Zone.Start; Spot = spot }

    let SlideArea1Start (color: Color) = { Color = color; Zone = Zone.Board; Spot = Board.SlideAreas[0].StartSpot}

    let SlideArea2Start (color: Color) = { Color = color; Zone = Zone.Board; Spot = Board.SlideAreas[1].StartSpot}

    let IsZone (zone: Zone) (location: Location) = location.Zone = zone

    let HasLocation (locations: Locations) (location: Location) = locations |> List.contains location

    let NotHasLocation (locations: Locations) (location: Location) = not (HasLocation locations location)

    let DistanceRelativeToFirstRedSpot (location: Location) = location.Spot + location.Color.AsOrdinal * Board.MaxBoardSpotsPerColor
        
    let DistanceBetween (location1: Location) (location2: Location) =
        match location1.Zone, location2.Zone with
        | Zone.Board, Zone.Board -> (DistanceRelativeToFirstRedSpot location2) - (DistanceRelativeToFirstRedSpot location1)
        | Zone.Board, Zone.Safety -> 
            let location1Value = DistanceRelativeToFirstRedSpot location1
            let location2Value = 
                let safetyValue = DistanceRelativeToFirstRedSpot (Create location2.Color Zone.Board Board.SafetyEntrySpot)
                safetyValue + location2.Spot
            location2Value - location1Value
        | Zone.Board, Zone.Home ->
            let location1Value = DistanceRelativeToFirstRedSpot location1
            let location2Value = 
                let safetyValue = DistanceRelativeToFirstRedSpot (Create location2.Color Zone.Board Board.SafetyEntrySpot)
                safetyValue + 6
            location2Value - location1Value
        | Zone.Safety, Zone.Home ->
            let location1Value = location1.Spot
            let location2Value = 6 
            location2Value - location1Value
        | _ -> raise (LocationError("Distance calculation not supported"))
            
