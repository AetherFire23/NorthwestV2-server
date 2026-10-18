# Projet is the pratical asssembly since it's containing the ef core context 

#  dotnet tool install --global dotnet-ef


dotnet ef migrations add "initial $([Guid]::NewGuid().ToString() )" --project "../src/Core";

# dotnet-ef migrations add "initial" --project "../src/Core";

# Just add the PATH export for this session and try
# export PATH="$PATH:$HOME/.dotnet/tools" && dotnet ef migrations add "initial" --project "../src/Core"

# permanent:
# echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc && source ~/.bashrc