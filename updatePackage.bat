## 参考 https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-nuget-push
cd G:\mywork\Snail\Snail
echo 正在打包...
dotnet pack .\Snail.Core\Snail.Core.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Snail.Core.Default\Snail.Core.Default.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Snail.Common\Snail.Common.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Snail.DAL\Snail.DAL.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Snail.FileStore\Snail.FileStore.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Snail.Office\Snail.Office.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Permission\Snail.Permission.csproj -o G:\mywork\Snail\Snail\NugetPackage -p:PackageVersion=1.0.2
dotnet nuget push G:\mywork\Snail\Snail\NugetPackage\*.nupkg -k oy2hp437jpjcgohirf2aoqt6y7mmxslu3j7pgnezbkjsae -s https://api.nuget.org/v3/index.json --skip-duplicate
pause