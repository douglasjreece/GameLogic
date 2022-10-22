namespace GameLogic.Sorry

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Card = 
    let Denomonations: Cards = 
        [
            Number(1);
            Number(2);
            Number(3);
            Number(4);
            Number(5);
            Number(7);
            Number(8);
            Number(10);
            Number(11);
            Number(12);
            Sorry
        ]
    
    let CountOfEachCard = 4
