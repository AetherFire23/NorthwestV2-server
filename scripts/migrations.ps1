# Projet is the pratical asssembly since it's containing the ef core context 

#  dotnet tool install --global dotnet-ef

$pathWithProjectContainingEfCoreContext = "../src/NorthwestV2.Practical";
dotnet ef migrations add "initial $([Guid]::NewGuid().ToString() )" --project "../src/NorthwestV2.Practical";