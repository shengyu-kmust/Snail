## 参考 https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-nuget-push
echo 正在打包...
dotnet pack .\Snail.Core\Snail.Core.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet pack .\Snail.Core.Default\Snail.Core.Default.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet pack .\Snail.Common\Snail.Common.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet pack .\Snail.DAL\Snail.DAL.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet pack .\Snail.FileStore\Snail.FileStore.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet pack .\Snail.Office\Snail.Office.csproj -o .\NugetPackage -p:PackageVersion=1.0.2
dotnet pack .\Permission\Snail.Permission.csproj -o .\NugetPackage -p:PackageVersion=1.0.3
dotnet nuget push .\NugetPackage\*.nupkg -k oy2j7wk52md6qzngbzxqoitnybyaurve5iqxseukzuykgq -s https://api.nuget.org/v3/index.json --skip-duplicate
pause