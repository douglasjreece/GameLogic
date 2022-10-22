namespace GameLogic.Sorry

module List =
    let oneOrEmpty (list: 'a list) = 
        if list.Length > 0
            then [list.[0]]
            else []

    let cartesian xs ys = 
        xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

    let randomize (seed: int option) list = 
        let rnd = if seed.IsSome then System.Random (seed.Value) else System.Random ()
        list |> List.sortBy(fun _ -> rnd.Next(1, list.Length) )

    let first (test: 'a -> bool) (list: 'a list): 'a =
        list |> List.filter test |> List.head