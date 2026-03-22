# Projet is the pratical asssembly since it's containing the ef core context 

#  dotnet tool install --global dotnet-ef


$pathWithProjectContainingEfCoreContext = "../src/NorthwestV2.Practical";
dotnet ef migrations add "initial $([Guid]::NewGuid().ToString() )" --project "../src/NorthwestV2.Infrastructure";

# dotnet-ef migrations add "initial" --project "../src/NorthwestV2.Infrastructure";

# Just add the PATH export for this session and try
# export PATH="$PATH:$HOME/.dotnet/tools" && dotnet ef migrations add "initial" --project "../src/NorthwestV2.Infrastructure"

# permanent:
# echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc && source ~/.bashrc