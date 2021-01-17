project='NETStandard.Shim'
project_format='csproj'

restore $project.$project_format
if [[ $? -ne 0 ]]; then
  read -rsp "Press [Enter] to quit"
  echo ""
  exit
fi
