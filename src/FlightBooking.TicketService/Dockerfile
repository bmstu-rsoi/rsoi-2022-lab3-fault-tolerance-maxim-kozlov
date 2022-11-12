FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

COPY publish/TicketService/ .

EXPOSE 8070
ENTRYPOINT ["dotnet", "FlightBooking.TicketService.dll"]