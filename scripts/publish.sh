dotnet restore src/FlightBooking.sln

dotnet publish src/FlightBooking.Gateway/FlightBooking.Gateway.csproj -c Release -o publish/FlightBookingGateway --no-restore
dotnet publish src/FlightBooking.TicketService/FlightBooking.TicketService.csproj -c Release -o publish/TicketService --no-restore
dotnet publish src/FlightBooking.FlightService/FlightBooking.FlightService.csproj -c Release -o publish/FlightService --no-restore
dotnet publish src/FlightBooking.BonusService/FlightBooking.BonusService.csproj -c Release -o publish/BonusService --no-restore
