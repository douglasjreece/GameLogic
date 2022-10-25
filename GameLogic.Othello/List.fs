namespace GameLogic.Othello

module List =
    let oneOrEmpty (list: 'a list) = 
        if list.Length > 0
            then [list.[0]]
            else []

    let cartesian xs ys = 
        xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

    let any (pred: 'a -> bool) (list: 'a list) = list |> List.filter (fun x -> pred x) |> List.length > 0

    let findOrNone (pred: 'a -> bool) (list: 'a list) = 
        let rec aux ls =
            match ls with
            | head::tail -> 
                        if pred head 
                        then Some head
                        else aux tail
            | _ -> None
        aux list

    let randomize (seed: int option) list = 
        let rnd = if seed.IsSome then System.Random (seed.Value) else System.Random ()
        list |> List.sortBy(fun _ -> rnd.Next(1, list.Length) )

    let rec intersection (list1: 'a list) (list2: 'a list) = 
        let result = list1 |> cartesian list2
                        |> List.filter (fun tup -> (fst tup) = (snd tup))
                        |> List.map (fun tup -> fst tup)
        result

    let intersects list1 list2 = not (intersection list1 list2).IsEmpty

    let difference list1 list2 = list2 |> List.filter (fun x -> not (list1 |> List.contains x))

    let filterSome (list: 'a option list): 'a list =
        list |> List.filter (fun x -> x.IsSome) |> List.map (fun x -> x.Value)

    let elementOrNone (index: int) (list: 'a list): 'a option =
        if index >= 0 && index < list.Length
        then Some list.[index]
        else None