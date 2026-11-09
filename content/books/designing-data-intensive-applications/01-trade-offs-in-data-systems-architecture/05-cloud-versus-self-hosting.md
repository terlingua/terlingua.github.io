---
title: "Cloud Versus Self-Hosting"
questions:
  - id: build-buy
    prompt: "According to the common rule of thumb in the chapter, what should you build in-house?"
    options:
      - id: oa94a2a
        text: "Everything that touches customer data"
      - id: od1068a
        text: "Anything open source"
      - id: o4d7970
        text: "Things that are a core competency or competitive advantage"
        correct: true
      - id: o5480b3
        text: "Only CPUs and hardware"
    explanation: "“A common rule of thumb is that things that are a core competency or a competitive advantage of your organization should be done in-house, whereas things that are non-core, routine, or commonplace should be left to a vendor”."
  - id: spectrum
    prompt: "Downloading MySQL and running it on a VM you manage in AWS sits where on the chapter's spectrum?"
    options:
      - id: o9db8ce
        text: "A fully managed cloud native database"
      - id: o6bd57e
        text: "A SaaS product operated by a vendor"
      - id: oc85c02
        text: "Bespoke software written and run in-house"
      - id: of04992
        text: "Off-the-shelf software that you self-host (IaaS)"
        correct: true
    explanation: "“The middle ground is off-the-shelf software (open source or commercial) that you self-host, or deploy yourself—for example, if you download MySQL and install it on a server you control. This could be on your own hardware ... or on a virtual machine (VM) in the cloud (infrastructure as a service, or IaaS).”"
  - id: when-self-host-cheaper
    prompt: "A company has a stable, predictable workload and an experienced ops team that already runs its databases. What does the chapter suggest?"
    options:
      - id: o78aec6
        text: "It should migrate to serverless immediately"
      - id: o5bab25
        text: "Buying its own machines and self-hosting is often cheaper"
        correct: true
      - id: o13346b
        text: "It should outsource operations to a SaaS vendor"
      - id: oaf84b9
        text: "Cloud is always cheaper due to economies of scale"
    explanation: "“If you already have experience setting up and operating the systems you need, and if your load is quite predictable ... then it’s often cheaper to buy your own machines and run the software on them yourself”."
  - id: analytics-variable
    prompt: "Why are analytical workloads a particularly good fit for cloud elasticity?"
    options:
      - id: o11f9e6
        text: "Analytical queries are always small"
      - id: o2b806d
        text: "Cloud providers give analytics compute for free"
      - id: o20d131
        text: "Large interactive queries need lots of parallel resources briefly, then sit idle; cloud lets you return unused resources"
        correct: true
      - id: oee7733
        text: "Analytical data cannot be stored on premises"
    explanation: "“analytical systems often have extremely variable load. Running a large analytical query quickly requires a lot of computing resources in parallel, but once the query completes, those resources sit idle” and “using the cloud can save money, since you can return unused resources to the provider rather than leaving them idle.”"
  - id: interactive-variability
    prompt: "For interactive analytical queries, what happens to workload variability as you demand faster responses?"
    options:
      - id: oe893f8
        text: "It decreases"
      - id: oab0733
        text: "It disappears if queries are enqueued"
      - id: oa186a9
        text: "It increases"
        correct: true
      - id: oa63ddd
        text: "It stays constant"
    explanation: "“for interactive queries, the faster you want them to complete, the more variable the workload becomes.” Scheduled reports can be smoothed by queuing; interactive ones cannot."
  - id: cloud-downsides
    prompt: "Which are downsides of cloud services listed in the chapter?"
    type: multiple
    options:
      - id: ob60ea8
        text: "Diagnosing performance problems is hard without access to internals"
        correct: true
      - id: o781d8d
        text: "Vendor lock-in when there is no standard API"
        correct: true
      - id: o7e3c6a
        text: "You must buy hardware up front"
      - id: o7c2032
        text: "You cannot implement a missing feature yourself"
        correct: true
      - id: of206cb
        text: "Risk of being locked out because of sanctions in a political conflict"
        correct: true
    explanation: "The chapter states “The biggest downside of a cloud service is that you have no control over it” and lists missing features, waiting out outages, difficult diagnosis, being forced to migrate if the vendor changes or shuts down, sanctions, and trust in security."
  - id: tune-yourself
    prompt: "What can self-hosting offer that a cloud service usually will not?"
    options:
      - id: o853ed2
        text: "Configuring and tuning the system for your particular workload"
        correct: true
      - id: o5dc1ef
        text: "Zero operations work"
      - id: ob31cae
        text: "Automatic multi-region failover"
      - id: o7a623b
        text: "Metered billing"
    explanation: "“if you run the service, you can configure and tune it to perform well on your particular workload. A cloud service would not likely be willing to make such customizations on your behalf.”"
  - id: hft
    prompt: "Why does high-frequency trading remain a case for in-house systems?"
    options:
      - id: oc31718
        text: "Very latency-sensitive applications require full control of the hardware"
        correct: true
      - id: o2d2af0
        text: "Trading data is too small for the cloud"
      - id: ofd1c6e
        text: "Cloud providers ban financial workloads"
      - id: o99d0d3
        text: "HFT does not need reliability"
    explanation: "“very latency-sensitive applications such as high-frequency trading require full control of the hardware.”"
  - id: cloud-native-advantages
    prompt: "Which advantages does the chapter attribute to systems designed from the ground up to be cloud native?"
    type: multiple
    options:
      - id: o719ed6
        text: "Quickly scaling compute to match load"
        correct: true
      - id: o23d6b9
        text: "Better performance on the same hardware"
        correct: true
      - id: ob6da37
        text: "Faster recovery from failures"
        correct: true
      - id: o1b68b3
        text: "Supporting larger datasets"
        correct: true
      - id: ob832e6
        text: "No need for any network"
    explanation: "Cloud native systems “have been shown to have several advantages: better performance on the same hardware, faster recovery from failures, being able to quickly scale computing resources to match the load, and supporting larger datasets”."
  - id: classify-native
    prompt: "Using Table 1-2, which of these is a cloud native analytical system?"
    options:
      - id: o20309c
        text: "Teradata"
      - id: oc158f7
        text: "Snowflake"
        correct: true
      - id: oad8c0a
        text: "ClickHouse"
      - id: o81d5ed
        text: "MongoDB"
    explanation: "Table 1-2 lists “Snowflake [26], Google BigQuery, Azure Synapse Analytics” as cloud native analytical systems and “Teradata, ClickHouse, Spark” as self-hosted."
  - id: layering
    prompt: "What is the key idea behind cloud native services according to the \"Layering of cloud services\" discussion?"
    options:
      - id: oc1046f
        text: "Use only GPUs"
      - id: o050b84
        text: "Run the same software on VMs instead of physical machines"
      - id: oad3e7c
        text: "Build higher-level services on lower-level cloud services, such as a warehouse built on object storage"
        correct: true
      - id: od84e2c
        text: "Avoid all managed services"
    explanation: "“the key idea of cloud native services is not only to use the computing resources managed by your operating system, but also to build upon lower-level cloud services to create higher-level services.” Example: Snowflake “relies on S3 for data storage”."
  - id: object-store-api
    prompt: "What do object stores trade away compared with a filesystem, and what do they gain?"
    options:
      - id: o1ab9f6
        text: "Durability in exchange for speed"
      - id: o32e46b
        text: "Encryption in exchange for capacity"
      - id: o1249f4
        text: "A more limited API, in exchange for hiding the physical machines and not losing data if machines fail"
        correct: true
      - id: o646771
        text: "Higher latency in exchange for POSIX semantics"
    explanation: "Object stores “provide more limited APIs than a typical filesystem (basic file reads and writes), but they have the advantage that they hide the underlying physical machines” and “Even if some machines or their disks fail entirely, no data is lost.”"
  - id: higher-level-choice
    prompt: "When should you build from lower-level cloud components rather than use a higher-level service?"
    options:
      - id: o3f4b04
        text: "Always, because higher-level services are less reliable"
      - id: o55506e
        text: "When no high-level system meets your needs"
        correct: true
      - id: o6e3b0a
        text: "Never; building is always worse"
      - id: o442ea2
        text: "Only when the data is under 1 GB"
    explanation: "“If your needs match the situations for which a higher-level system is designed, using the existing higher-level system will probably meet your needs with much less hassle ... if no high-level system meets your needs, building it yourself from lower-level components is the only option.”"
  - id: local-disk-ephemeral
    prompt: "Why do cloud native systems treat local instance disks more like an ephemeral cache?"
    options:
      - id: o88144d
        text: "The disk becomes inaccessible if the instance fails or is replaced with a different size on another machine"
        correct: true
      - id: ocbf76e
        text: "Cloud providers forbid writing to local disks"
      - id: od52cb9
        text: "Local disks are slower than object storage"
      - id: o0c7cd1
        text: "Local disks cannot store more than 4 KiB"
    explanation: "“the local disk becomes inaccessible if the associated instance fails, or if the instance is replaced with a bigger or a smaller one (on a different physical machine) to adapt to changes in load.”"
  - id: virtual-disk-issue
    prompt: "A traditional database runs on a network-attached virtual block device (e.g., EBS). What problem does the chapter highlight?"
    options:
      - id: o2143e2
        text: "Every I/O is a network call, making the database sensitive to network glitches, and block emulation adds overhead"
        correct: true
      - id: oa8fdf2
        text: "Virtual disks lose data on every reboot"
      - id: o7d0ba1
        text: "Virtual disks do not support files"
      - id: o6fb9d1
        text: "Virtual disks cannot be detached"
    explanation: "“the block device emulation introduces overheads that can be avoided in systems that are designed from the ground up for the cloud” and “every I/O operation on the virtual block device is a network call”."
  - id: small-values
    prompt: "Object stores suit files from hundreds of kilobytes to gigabytes, but database rows are small. How do cloud databases typically reconcile this?"
    options:
      - id: o6754b2
        text: "Avoid object storage completely"
      - id: o140c4f
        text: "Manage small values in a separate service and store larger blocks of many values in the object store"
        correct: true
      - id: o220f25
        text: "Pad rows to 1 MB"
      - id: o52d5f1
        text: "Store each row as its own object"
    explanation: "“cloud databases therefore typically manage smaller values in a separate service and store larger data blocks (containing many individual values) in an object store”."
  - id: disaggregation-cost
    prompt: "Separating storage and compute means that to analyze data in S3 you run code elsewhere. What is the inherent cost?"
    options:
      - id: oa60463
        text: "Data must be transferred over the network"
        correct: true
      - id: odebb57
        text: "Data must be converted to CSV"
      - id: o03ebfc
        text: "You cannot use SQL"
      - id: o262e03
        text: "The data becomes mutable"
    explanation: "“S3 only stores files, and if you want to analyze that data, you will have to run the analysis code somewhere outside of S3. This implies transferring the data over the network”."
  - id: multitenancy
    prompt: "What engineering challenge comes with multitenancy?"
    options:
      - id: o5a3e90
        text: "Each customer needs dedicated hardware"
      - id: ofbe5c5
        text: "It forbids encryption"
      - id: of3060b
        text: "It prevents scaling"
      - id: o25d40a
        text: "Ensuring one customer's activity does not affect others' performance or security"
        correct: true
    explanation: "Multitenancy “can enable better hardware utilization, easier scalability, and easier management ... but it also requires careful engineering to ensure that one customer’s activity does not affect the performance or security of the system for other customers”."
  - id: ops-shift
    prompt: "In the cloud era, what do capacity planning and performance optimization turn into?"
    options:
      - id: o10aad9
        text: "Financial planning and cost optimization"
        correct: true
      - id: o290801
        text: "Hardware procurement and overclocking"
      - id: o068c75
        text: "They disappear entirely"
      - id: ob719a0
        text: "Security auditing and penetration testing"
    explanation: "“Capacity planning becomes financial planning, and performance optimization becomes cost optimization”."
  - id: ops-not-outsourced
    prompt: "Which operational concerns cannot be fully outsourced to cloud services?"
    type: multiple
    options:
      - id: o515fce
        text: "Replacing failed disks in the provider's datacenter"
      - id: o3004d3
        text: "Monitoring load and tracking down causes of outages"
        correct: true
      - id: o5960b7
        text: "Security of your application and its libraries"
        correct: true
      - id: o694d7a
        text: "Interactions between your own services"
        correct: true
    explanation: "“Other operational aspects that cannot fully be outsourced to cloud services include maintaining the security of an application and the libraries it uses, managing the interactions between your own services, monitoring the load on your services, and tracking down the cause of problems”."
  - id: quotas
    prompt: "Your batch job suddenly fails because the account hit a limit on concurrent processes. Which point from the chapter did the team miss?"
    options:
      - id: oc99ac9
        text: "Metered billing removes all limits"
      - id: oe6aa5f
        text: "Object stores have unlimited throughput"
      - id: o317ec4
        text: "Serverless functions never fail"
      - id: o4b4110
        text: "Cloud services have resource limits or quotas that you must know and plan for"
        correct: true
    explanation: "“cloud services do have resource limits or quotas (such as the maximum number of processes you can run concurrently), which you need to know about and plan for before you run into them”."
  - id: devops-emphasis
    prompt: "Which are emphases of the DevOps/SRE philosophy listed in the chapter?"
    type: multiple
    options:
      - id: o47dd9a
        text: "Avoiding frequent application updates"
      - id: oe484a1
        text: "Ephemeral VMs and services rather than long-running servers"
        correct: true
      - id: o46d8f9
        text: "Automation and repeatable processes over manual one-off jobs"
        correct: true
      - id: o9d3b24
        text: "Learning from incidents"
        correct: true
    explanation: "The list includes “Setting up automation”, “Using ephemeral VMs and services rather than long-running servers”, “Enabling frequent application updates”, “Learning from incidents” and preserving organizational knowledge."
---
Build or buy, run or rent: this section weighs cloud services against self-hosted software, then shows
how "cloud native" systems are built differently — on object storage, with storage and compute
separated, and often multitenant.

## Learning objectives

- Decide when a cloud service is likely cheaper or more expensive than self-hosting
- List the control-related risks of cloud services
- Explain layering, separation of storage and compute, and multitenancy
- Describe how operations changes in the cloud era

## In a nutshell

- Cloud wins for variable load and unfamiliar systems; self-hosting can win for predictable load and
  existing expertise.
- The big downside of cloud is loss of control: features, outages, debugging, pricing, lock-in, sanctions,
  trust.
- Cloud native systems treat local disks as caches and build on object storage.
- Capacity planning becomes financial planning; performance optimization becomes cost optimization.

## Connections to today

- **Cloud repatriation.** Several companies with steady workloads have moved off public cloud to save
  money — the "predictable load plus existing skills" case the book describes.
- **Disaggregated databases.** Neon (Postgres), Aurora, and warehouse engines such as Snowflake split
  compute from storage so compute can scale to zero.
- **FinOps.** Dedicated cost-optimization teams are the "capacity planning becomes financial planning"
  shift made into a job title.
