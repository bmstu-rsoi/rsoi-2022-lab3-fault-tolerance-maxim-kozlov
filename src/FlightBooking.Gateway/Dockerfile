FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

COPY publish/FlightBookingGateway/ .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FlightBooking.Gateway.dll"]