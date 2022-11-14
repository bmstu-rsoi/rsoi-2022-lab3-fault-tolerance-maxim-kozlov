./scripts/publish.sh

docker compose build 

./scripts/start.sh

export WAIT_PORTS=8080,8070,8060,8050 
./scripts/wait-script.sh