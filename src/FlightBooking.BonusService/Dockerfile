FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

COPY publish/BonusService/ .

EXPOSE 8050
ENTRYPOINT ["dotnet", "FlightBooking.BonusService.dll"]