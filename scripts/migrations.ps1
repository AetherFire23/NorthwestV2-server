# Projet is the pratical asssembly since it's containing the ef core context 

$pathWithProjectContainingEfCoreContext = "../ERP.Practical";
dotnet ef migrations add "initial $([Guid]::NewGuid().ToString() )" --project $pathWithProjectContainingEfCoreContext;





