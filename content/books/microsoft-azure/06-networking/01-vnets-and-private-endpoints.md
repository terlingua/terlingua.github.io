---
title: Virtual networks and private endpoints
questions:
  - id: private-endpoint
    prompt: What does a private endpoint do?
    options:
      - text: Hides a service from Azure Monitor
      - id: private-ip
        text: Gives a PaaS resource (such as a storage account or SQL database) a private IP address inside your virtual network
        correct: true
      - text: Encrypts data at rest
      - text: Creates a VPN to your office
    explanation: Traffic stays on the Microsoft backbone and the resource's public endpoint can be disabled.
  - id: nsg
    prompt: What is a network security group (NSG)?
    options:
      - text: An Entra group of network administrators
      - text: A type of load balancer
      - id: rules
        text: A set of allow/deny rules filtering traffic to and from subnets or network interfaces by IP, port and protocol
        correct: true
    explanation: NSGs are stateful — return traffic for an allowed flow is permitted automatically.
  - id: dns
    prompt: Which statements about private endpoints are true?
    type: multiple
    options:
      - text: Private DNS zones (such as `privatelink.database.windows.net`) make the service's normal hostname resolve to the private IP
        correct: true
      - text: App Service and Functions need VNet integration for outbound calls to reach private endpoints
        correct: true
      - text: You can disable public network access on the target resource once private endpoints are in place
        correct: true
      - text: Private endpoints require changing your application to use IP addresses instead of hostnames
    explanation: With DNS configured, code keeps using the same hostnames and connection strings.
---
## Learning objectives

- Design virtual networks and subnets
- Secure traffic with NSGs
- Reach PaaS services privately with private endpoints

## Virtual networks

A **VNet** is your private network in a region, with an address space (for example
`10.20.0.0/16`) divided into **subnets**:

```text
vnet-shop-prod (10.20.0.0/16)
├── snet-app        10.20.1.0/24   App Service VNet integration (delegated)
├── snet-private    10.20.2.0/24   Private endpoints (SQL, storage, Key Vault)
└── snet-aks        10.20.16.0/20  AKS nodes
```

VNets connect to each other with **peering**, and to on-premises networks with **VPN Gateway** or
**ExpressRoute**. A **NAT Gateway** gives subnets a stable outbound IP.

## Network security groups

```bash
az network nsg rule create -g rg-shop-prod --nsg-name nsg-private -n deny-internet-inbound \
  --priority 4000 --direction Inbound --access Deny --source-address-prefixes Internet --destination-port-ranges '*'
```

Rules are evaluated by priority (lowest number first). Service tags such as `AzureLoadBalancer`
or `Sql` keep rules readable.

## Private endpoints

```bash
az network private-endpoint create -g rg-shop-prod -n pe-sql-shop \
  --vnet-name vnet-shop-prod --subnet snet-private \
  --private-connection-resource-id $(az sql server show -n sql-shop-prod -g rg-shop-prod --query id -o tsv) \
  --group-id sqlServer --connection-name sql-shop
az sql server update -n sql-shop-prod -g rg-shop-prod --enable-public-network false
```

Link a private DNS zone (`privatelink.database.windows.net`) to the VNet so
`sql-shop-prod.database.windows.net` resolves to `10.20.2.x` from inside the network.

## Private endpoints vs service endpoints

| | Private endpoint | Service endpoint |
|-|------------------|------------------|
| Mechanism | Private IP for a specific resource in your VNet | Optimised route from a subnet to a service's public endpoint |
| Data exfiltration protection | Strong (scoped to one resource) | Weaker |
| On-premises access | Yes (via VPN/ExpressRoute) | No |
| Cost | Hourly + data charges | Free |

Private endpoints are the default recommendation for production data services.
