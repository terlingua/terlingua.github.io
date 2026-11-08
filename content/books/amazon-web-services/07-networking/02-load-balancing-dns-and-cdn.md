---
title: Load balancing, Route 53 and CloudFront
questions:
  - id: alb
    prompt: Which load balancer supports routing HTTP requests by path and host header to different target groups?
    options:
      - text: Network Load Balancer
      - text: Gateway Load Balancer
      - id: alb
        text: Application Load Balancer
        correct: true
      - text: Classic DNS round robin
    explanation: The ALB operates at layer 7 and can route to EC2, IP targets (including ECS tasks) and Lambda functions.
  - id: alias
    prompt: What is a Route 53 alias record?
    options:
      - id: alias
        text: A Route 53 extension that maps a name, including the zone apex, to AWS resources such as a load balancer or CloudFront distribution
        correct: true
      - text: A CNAME that only works for subdomains
      - text: An e-mail forwarding rule
    explanation: Unlike a CNAME, an alias works at the zone apex (example.com) and queries to AWS resources are free.
  - id: routing
    prompt: Which are real Route 53 routing policies?
    type: multiple
    options:
      - text: Latency-based
        correct: true
      - text: Failover, using health checks
        correct: true
      - text: Weighted
        correct: true
      - text: Geolocation
        correct: true
    explanation: Route 53 also supports simple, geoproximity, multivalue answer and IP-based routing.
---
## Learning objectives

- Choose the right Elastic Load Balancer
- Route traffic globally with Route 53
- Accelerate and protect content with CloudFront

## Elastic Load Balancing

| Type | Layer | Highlights |
|------|-------|------------|
| Application Load Balancer | 7 | Path/host/header routing, WebSockets, HTTP/2, authentication, Lambda targets |
| Network Load Balancer | 4 | TCP/UDP/TLS, millions of requests per second, static IPs, ultra-low latency |
| Gateway Load Balancer | 3 | Inserting third-party network appliances (firewalls) |

Target groups perform **health checks**; unhealthy targets stop receiving traffic, and Auto Scaling
replaces them.

## Route 53

```text
example.com        A  ALIAS → d1234.cloudfront.net
api.example.com    A  ALIAS → shop-alb-123.eu-central-1.elb.amazonaws.com  (latency record, eu-central-1)
api.example.com    A  ALIAS → shop-alb-456.us-east-1.elb.amazonaws.com     (latency record, us-east-1)
```

| Policy | Use |
|--------|-----|
| Simple | One resource |
| Weighted | Canary releases, A/B splits |
| Latency | Send users to the lowest-latency Region |
| Failover | Active-passive disaster recovery |
| Geolocation / geoproximity | Compliance or localisation by user location |

## CloudFront

CloudFront caches content at edge locations worldwide and terminates TLS close to users:

- **Origins** — S3 (with Origin Access Control, keeping the bucket private), ALBs, API Gateway, any HTTP server
- **Cache behaviours** — per path pattern TTLs and policies
- **AWS WAF** and **Shield** — filter malicious traffic; Shield Standard DDoS protection is included at no extra cost
- **CloudFront Functions / Lambda@Edge** — lightweight logic such as redirects and header manipulation

A static Blazor WebAssembly app is a natural fit: S3 for the files, CloudFront in front, and a
custom error response that serves `index.html` for unknown paths so client-side routing works.
