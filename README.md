# edge-node-service
small HTTP service that acts as an edge node

dotnet build
dotnet run --project src/EdgeNode/EdgeNode.csproj 

curl -s -X POST http://localhost:8080/data \
   -H 'Content-Type: application/json' \
   -d '{"sensor_id":"temp-01","value":33,"timestamp":"2026-04-22T10:30:00Z"}'

http://localhost:8080/sensors

## Proving the Edge Concept

for i in {1..20}; do
    curl -s -o /dev/null -w "%{time_total}\n" -X POST http://localhost:8080/data \
   -H 'Content-Type: application/json' \
   -d '{"sensor_id":"temp-01","value":33,"timestamp":"2026-04-22T10:30:00Z"}'                                     
done 

for i in {1..20}; do
    curl -s -o /dev/null -w "%{time_total}\n" -X POST http://localhost:8081/data \
   -H 'Content-Type: application/json' \
   -d '{"sensor_id":"temp-01","value":33,"timestamp":"2026-04-22T10:30:00Z"}'                                     
done 