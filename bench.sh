#!/usr/bin/env bash
PAYLOAD='{"sensor_id":"temp-01","value":24.7,"timestamp":"2026-04-22T10:30:00Z"}'
N=20

echo "Local test:" >> bench.txt
for i in $(seq 1 $N); do
    curl -s -o /dev/null -w "%{time_total}\n" -X POST http://localhost:8080/data \
   -H 'Content-Type: application/json' \
   -d "$PAYLOAD"                                
done >> bench.txt

echo "Cloud test:" >> bench.txt
for i in $(seq 1 $N); do
    curl -s -o /dev/null -w "%{time_total}\n" -X POST http://localhost:8081/data \
   -H 'Content-Type: application/json' \
   -d "$PAYLOAD" >> bench.txt                                    
done 

bench() {

}