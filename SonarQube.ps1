cls
Remove-Item -path .\TestResults -Recurse
Remove-Item -path .\output.coverage.xml
Remove-Item -path .\test.results.trx
dotnet sonarscanner begin /k:"PiraSoft.Tools" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="sqp_6b9df7dd7a2becc61adf385d5a0f92337a1c7269"
dotnet build
dotnet test --no-build --logger "trx;logfilename=.\..\test.results.trx" --results-directory:TestResults --collect:"Code Coverage"
codecoverage merge *.coverage -r -f xml
dotnet sonarscanner end /d:sonar.login="sqp_6b9df7dd7a2becc61adf385d5a0f92337a1c7269"