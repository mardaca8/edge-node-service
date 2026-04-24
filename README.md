# edge-node-service
A small HTTP service that acts as an **edge node**: ingests sensor readings over HTTP, processes them locally with per-sensor threshold alerts, and exposes endpoints to inspect state. Containerised with Docker.

Build with **.NET8**

## Endpoints
The service accept sensor readings via a POST request 

POST /data

Request:

`{"sensor_id": "temp-01", "value": 24.7, "timestamp": "2026-04-22T10:30:00Z"}`

Response:

`{"sensor_id": "temp-01", "value": 24/7, "status": "ok", "min": 0, "max": 50, "reading_timestamp": "2026-04-22T10:30:00Z", "processed_at": "2026-04-24T14:02:11.482Z"}`

status is one of:
  - "ok" — value within [min, max]
  - "below_min" — value below the configured minimum                                                                        
  - "above_max" — value above the configured maximum 

GET /sensors — list all known sensors and their last state

GET /health  - check service

## Configuring thresholds
Thresholds saved in src/EdgeNode/appsettings.json:

```
{            
  "Thresholds": {               
    "Default":  { "Min": -50, "Max": 150 },                                                                               
    "Sensors": {                                                                                                        
      "temp-01":     { "Min": 0,   "Max": 50  },
      "temp-02":     { "Min": -10, "Max": 40  },                                                                        
      "humidity-01": { "Min": 0,   "Max": 100 }                                                                           
    }                                                             
  }                                                                                                                       
}
```
Unknown sensors fall back to Default

## Proving the Edge Concept

### Setup
- `edge-node` container on :8080 — no latency
- `cloud-node` container on :8081 — `tc qdisc add dev eth0 rootnetem delay 100ms 10ms`
- 20 sequential POSTs per endpoint
- Tool: curl -w "%{time_total}

### Real tests
Terminal output of ./bench.sh on my machine (My specs MB pro, M1 pro, 16 gb, macOS tahoe)

```
LOCAL (edge-node :8080) Median of 20 requests: 4.221ms
CLOUD (cloud-node :8081, +100ms) Median of 20 requests: 214.31ms
```

### How to reproduce
`docker compose up -d --build`

`./bench.sh`


### Result
Median local response: ~4.221ms. Median cloud response: 214.31ms. Local processing is ~50x faster on median request. Emitting at 100 Hz, a 100ms cloud round-trip would back up ~10 reading (Freq * Latency) before the first one returned. This simulation only models fixed delay on stable LAN. It ignores real-world issues like wireless jitter or packet loss. 

#### Architecture
I picked C# with .NET 8 because I've used it before.

A threshold check (`below_min` / `above_max` / `ok`) is the kind of thing I'd actually understand to do at the edge — decide if a reading is bad, without waiting on the cloud.

The Dockerfile uses two stages: one with the .NET SDK to build the app, and a smaller runtime image that only contains what's needed to run it. This keeps the final image small and the container has no compiler or source code in it.
                                                                                                       
`docker-compose.yml` runs the same image twice: once as `edge-node` with no latency, and once as `cloud-node` with `tc netem` adding 100 ms of delay.

#### Retrospective
A lot time went into Docker and `tc netem`. Getting `tc` to work inside the container also caught me out. I had to install it at startup. My first benchmark numbers looked weird because the very first request was way slower than the rest, so I added a few warmup calls in `bench.sh`.

#### Next Steps
I would swap **C++** with `cpp-httplib` to get a much smaller and faster container. Also, I would add storage so the state could survive a restart