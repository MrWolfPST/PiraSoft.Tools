cls
Remove-Item -path .\TestResults -Recurse
Remove-Item -path .\output.coverage.xml
dotnet sonarscanner begin /k:"PiraSoft.Tools" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="43a9a82f917028b7482e036e1ec072c2d438b4a5"
dotnet build
dotnet test --no-build --results-directory:TestResults --collect:"Code Coverage"
#dotnet test --results-directory:TestResults --collect:"Code Coverage"
codecoverage merge *.coverage -r -f xml
dotnet sonarscanner end /d:sonar.login="43a9a82f917028b7482e036e1ec072c2d438b4a5"