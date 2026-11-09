---
title: "Chapter Opening — Backward and Forward Compatibility"
questions:
  - id: backward-def
    prompt: "Which statement defines backward compatibility?"
    options:
      - id: o51e3b4
        text: "Older code can read data written by newer code"
      - id: oc4950d
        text: "Data never changes format"
      - id: o86e9ad
        text: "Data can be read by any language"
      - id: oa0cb27
        text: "Newer code can read data written by older code"
        correct: true
    explanation: "“Backward compatibility Ensures that newer code can read data written by older code”."
  - id: forward-def
    prompt: "Which statement defines forward compatibility?"
    options:
      - id: ob55655
        text: "Clients always upgrade before servers"
      - id: o6597dc
        text: "Code can be rolled forward without tests"
      - id: o772383
        text: "Older code can read data written by newer code"
        correct: true
      - id: of2d70d
        text: "Newer code can read data written by older code"
    explanation: "“Forward compatibility Ensures that older code can read data written by newer code”."
  - id: old-client-new-service
    prompt: "An old client calls a newer service. Which compatibility is needed?"
    options:
      - id: ocd38ac
        text: "None, if the service uses HTTP"
      - id: o7ae3ba
        text: "Forward on the request, backward on the response"
      - id: o0fbd7c
        text: "Only backward compatibility"
      - id: oc14e77
        text: "Backward compatibility on the request and forward compatibility on the response"
        correct: true
    explanation: "“if you want an older client to be able to successfully call a newer service, you need backward compatibility on the request and forward compatibility on the response.”"
  - id: why-forward-harder
    prompt: "Why is forward compatibility typically harder than backward compatibility?"
    options:
      - id: o9fa7b5
        text: "Old code runs slower"
      - id: o2ff9f6
        text: "Old code must ignore additions it does not know about, whereas new code's author knows the old format"
        correct: true
      - id: odb3877
        text: "Forward compatibility requires a database"
      - id: o09742d
        text: "Backward compatibility is impossible"
    explanation: "“Backward compatibility is normally not hard to achieve. As the author of the newer code, you know the format of data written by older code ... Forward compatibility can be trickier, because it requires older code to ignore additions made by a newer version of the code.”"
  - id: lost-field
    prompt: "New code writes a record with a new field. Old code reads it into a model object, updates another field and writes it back. What can go wrong?"
    options:
      - id: o5b995e
        text: "The database rejects the write"
      - id: oe45036
        text: "The new field is lost if the model object does not preserve unknown fields"
        correct: true
      - id: o166562
        text: "Nothing; databases always preserve fields"
      - id: odc6bff
        text: "The old code crashes immediately"
    explanation: "“the desirable behavior is usually for the old code to keep the new field intact, even though it wasn’t able to interpret it. But if the record is decoded into a model object that does not explicitly preserve unknown fields, data can be lost”."
  - id: why-coexist
    prompt: "Why can't code changes happen instantaneously in a large application?"
    type: multiple
    options:
      - id: ob483d8
        text: "Rolling upgrades deploy to a few nodes at a time"
        correct: true
      - id: o7262a1
        text: "Users may not install client updates for some time"
        correct: true
      - id: o5740b0
        text: "Compilers are too slow"
      - id: oc1ce38
        text: "Databases forbid schema changes"
    explanation: "The chapter gives rolling upgrades (“deploying the new version to a few nodes at a time”) and client-side apps where “you’re at the mercy of the user, who may not install the update for some time.”"
---
Old and new code and data coexist during rolling upgrades and slow client updates, so data must be
compatible in both directions.

## Learning objectives

- Define backward and forward compatibility
- Apply them to requests and responses between clients and services
- Explain how old code can destroy new fields

## In a nutshell

- Backward: new code reads old data. Forward: old code reads new data.
- Old client → new service: backward on requests, forward on responses.
- Old code that decodes into a model without unknown fields and writes back can lose data.

## Connections to today

- **Mobile apps** stay unupdated for months; APIs must tolerate very old clients.
- **Kubernetes rolling deployments** mean every release is a period of mixed versions.
- **.NET's System.Text.Json** drops unknown members by default unless you add a `[JsonExtensionData]`
  property — exactly the Figure 5-1 trap.
