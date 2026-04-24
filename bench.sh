#!/usr/bin/env bash

PAYLOAD='{"sensor_id":"temp-01","value":24.7,"timestamp":"2026-04-22T10:30:00Z"}'
N=20

bench() {
    local name=$1 url=$2

    local median=$(for i in $(seq 1 5); do
        curl -s -o /dev/null -w "%{time_total}\n" -X POST "$url" \
            -H 'Content-Type: application/json' -d "$PAYLOAD"
    done | sort -n | awk '{a[NR]=$1*1000} END {if (NR%2==1) print a[(NR+1)/2]; else print (a[NR/2]+a[NR/2+1])/2}')


    local median=$(for i in $(seq 1 $N); do
        curl -s -o /dev/null -w "%{time_total}\n" -X POST "$url" \
            -H 'Content-Type: application/json' -d "$PAYLOAD"
    done | sort -n | awk '{a[NR]=$1*1000} END {if (NR%2==1) print a[(NR+1)/2]; else print (a[NR/2]+a[NR/2+1])/2}')

    echo "$name Median of $N requests: ${median}ms"
    
}

bench "LOCAL (edge-node :8080)" "http://localhost:8080/data"
bench "CLOUD (cloud-node :8081, +100ms RTT)" "http://localhost:8081/data"