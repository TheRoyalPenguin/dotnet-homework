FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY "7HW-FindExpertsTG.csproj" .
RUN dotnet restore "7HW-FindExpertsTG.csproj"

COPY . .

RUN dotnet build "7HW-FindExpertsTG.csproj" -c Release --no-restore
RUN dotnet publish "7HW-FindExpertsTG.csproj" -c Release -o /app/publish \
	--no-restore --no-build

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "7HW-FindExpertsTG.dll"]
