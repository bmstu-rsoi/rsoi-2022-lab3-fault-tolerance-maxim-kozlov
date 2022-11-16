docker ps -a

docker exec -i postgres psql -U program flights < test/flights.dump.sql
docker exec -i postgres psql -U program tickets < test/tickets.dump.sql
docker exec -i postgres psql -U program privileges < test/privileges.dump.sql