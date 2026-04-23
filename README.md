# edge-node-service
small HTTP service that acts as an edge node

dotnet build
dotnet run --project src/EdgeNode/EdgeNode.csproj 

curl -s -X POST http://localhost:5187/data \ 
   -H 'Content-Type: application/json' \
   -d '{"sensor_id":"temp-01","value":24.7,"timestamp":"2026-04-22T10:30:00Z"}'

http://localhost:5187/sensors