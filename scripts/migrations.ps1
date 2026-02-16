# Projet is the pratical asssembly since it's containing the ef core context 

$pathWithProjectContainingEfCoreContext = "../src/ERP.Practical";
dotnet ef migrations add "initial $([Guid]::NewGuid().ToString() )" --project "../src/ERP.Practical/NorthwestV2.Practical.csproj"





