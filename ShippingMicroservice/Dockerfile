#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ShippingMicroservice/ShippingMicroservice.csproj", "ShippingMicroservice/"]
RUN dotnet restore "ShippingMicroservice/ShippingMicroservice.csproj"
COPY . .
WORKDIR "/src/ShippingMicroservice"
RUN dotnet build "ShippingMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShippingMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShippingMicroservice.dll"]