namespace GameLogic.Chess

type Location = char * int // file and rank
    
module Location =
    let files = ['a';'b';'c';'d';'e';'f';'g';'h']
    let maxSpaces = 8

    let FileOf (location:Location) = fst location
    let RankOf (location:Location) = snd location

    let FileIndex (file:char): int = List.findIndex (fun x -> x = file) files

    let RelativeFile (offset:int) (file:char): char option =
        let index = FileIndex file
        let relativeIndex = index + offset
        if 0 <= relativeIndex && relativeIndex <= 7 
            then Some files.[relativeIndex]
            else None
    
    let RelativeRank (offset:int) (rank:int): int option =
        let relativeRank = rank + offset
        if 1 <= relativeRank && relativeRank <= 8 
            then Some relativeRank
            else None
        
    let RelativeLocation (fileOffset:int) (rankOffset:int) (from:Location): Location option =
        let adjacentFile = RelativeFile fileOffset (FileOf from)
        let adjacentRank = RelativeRank rankOffset (RankOf from)
        if adjacentFile.IsSome && adjacentRank.IsSome 
            then Some (adjacentFile.Value, adjacentRank.Value)
            else None

    let AdjacentLocations (fileDirection:int) (rankDirection:int) (count:int) (from:Location): Location list =
        let rec aux (count:int) (acc:Location list): Location list =
            if count > 0
                then
                    let previous = if acc.IsEmpty then from else acc.Head
                    let adjacent = RelativeLocation fileDirection rankDirection previous
                    if adjacent.IsSome
                        then aux (count - 1) (adjacent.Value :: acc)
                        else acc
                else acc
        let list = aux count []
        List.rev list

        
