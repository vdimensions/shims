project='NETStandard.Shim.Tests'

./restore.sh

dotnet clean $project.csproj && dotnet build $project.csproj && dotnet pack $project.csproj --no-build --no-restore
if [ $? -ne 0 ]; then
  read -rsp "Press [Enter] to quit"
  echo ""
  exit
fi