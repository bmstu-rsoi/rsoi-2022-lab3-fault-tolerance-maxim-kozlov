FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

COPY publish/FlightService/ .

EXPOSE 8060
ENTRYPOINT ["dotnet", "FlightBooking.FlightService.dll"]