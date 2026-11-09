---
title: "Formats for Encoding Data"
questions:
  - id: encoding-terms
    prompt: "Why does the book prefer the word *encoding* over *serialization*?"
    options:
      - id: o940f73
        text: "Encoding is faster"
      - id: o80f57a
        text: "Serialization also has a completely different meaning in transactions"
        correct: true
      - id: o956836
        text: "Serialization is a Java-only term"
      - id: o3401fc
        text: "Encoding implies compression"
    explanation: "“The term serialization is unfortunately also used in the context of transactions ... with a completely different meaning. To avoid overloading the word, we’ll stick with encoding in this book”."
  - id: zero-copy
    prompt: "What are Cap'n Proto and FlatBuffers examples of?"
    options:
      - id: ob7ebf8
        text: "Text formats"
      - id: o696b33
        text: "Zero-copy formats used both at runtime and on disk or network without an explicit conversion step"
        correct: true
      - id: o2e8c4b
        text: "Language-specific serializers"
      - id: ocdc436
        text: "Compression codecs"
    explanation: "“There are also zero-copy data formats that are designed to be used both at runtime and on disk/on the network, without an explicit conversion step, such as Cap’n Proto and FlatBuffers.”"
  - id: lang-specific-problems
    prompt: "Which are problems with language-specific formats such as pickle or java.io.Serializable?"
    type: multiple
    options:
      - id: oe53b19
        text: "Tied to one language"
        correct: true
      - id: o7ed98e
        text: "Versioning is an afterthought"
        correct: true
      - id: obf8c40
        text: "They cannot store strings"
      - id: ob1f67b
        text: "Efficiency is often poor"
        correct: true
      - id: od92f0b
        text: "Decoding arbitrary classes enables remote code execution"
        correct: true
    explanation: "The chapter lists language lock-in, that decoding “needs to be able to instantiate arbitrary classes. This is frequently a source of security problems”, that “Versioning data is often an afterthought”, and that “Efficiency ... is also often an afterthought.”"
  - id: lang-specific-when
    prompt: "When is it acceptable to use a language's built-in encoding?"
    options:
      - id: o55cd3f
        text: "For database storage"
      - id: o301e77
        text: "For public APIs"
      - id: oe80eb3
        text: "Only for very transient purposes"
        correct: true
      - id: o996fb1
        text: "For long-term archives"
    explanation: "“it’s generally a bad idea to use your language’s built-in encoding for anything other than very transient purposes.”"
  - id: json-numbers
    prompt: "Why does X's API return post IDs both as a JSON number and as a string?"
    options:
      - id: o31f363
        text: "Because JSON cannot store numbers"
      - id: o1d1ebc
        text: "To make responses larger for caching"
      - id: ob0ca47
        text: "To support XML clients"
      - id: o4a3bf7
        text: "JavaScript parses JSON numbers as doubles, which can't exactly represent integers above 2^53"
        correct: true
    explanation: "JSON “doesn’t distinguish integers and floating-point numbers, and it doesn’t specify a precision.” X includes post IDs “once as a JSON number and once as a decimal string, to work around the incorrect parsing of numbers by JavaScript applications”."
  - id: xml-csv-numbers
    prompt: "In XML and CSV, how can you tell whether \"00123\" is a number or a string?"
    options:
      - id: oc5c3a9
        text: "CSV quotes strings"
      - id: oea9594
        text: "By checking for leading zeros"
      - id: o67679f
        text: "XML always treats digits as numbers"
      - id: o42c038
        text: "You can't, except by referring to an external schema"
        correct: true
    explanation: "“In XML and CSV, you cannot distinguish between a number and a string that happens to consist of digits (except by referring to an external schema).”"
  - id: base64-hack
    prompt: "How do JSON and XML carry binary data, and at what cost?"
    options:
      - id: o7c7b86
        text: "They gzip it automatically"
      - id: oa3716a
        text: "They cannot carry binary data at all"
      - id: o69af3c
        text: "They support binary strings natively"
      - id: o300c88
        text: "Base64-encode it as text, increasing size by about a third"
        correct: true
    explanation: "They “don’t support binary strings ... people get around this limitation by encoding the binary data as text using Base64 ... it’s somewhat hacky and increases the data size by about a third.”"
  - id: interchange
    prompt: "Why will JSON, XML and CSV remain popular for data interchange despite their flaws?"
    options:
      - id: o0120cc
        text: "Getting organizations to agree on anything matters more than prettiness or efficiency"
        correct: true
      - id: o428ed2
        text: "They are the most compact formats"
      - id: o83f531
        text: "They are required by law"
      - id: o5158be
        text: "They have the best schema support"
    explanation: "“as long as people agree on the format, it often doesn’t matter how pretty or efficient it is. The difficulty of getting different organizations to agree on anything outweighs most other concerns.”"
  - id: open-content
    prompt: "In JSON Schema, `additionalProperties` defaults to true. What does that imply?"
    options:
      - id: of90588
        text: "An open content model: schemas mostly define what isn't permitted rather than what is"
        correct: true
      - id: ode36ff
        text: "Schemas cannot be validated"
      - id: oa83662
        text: "Unknown fields are rejected"
      - id: oe5b3b5
        text: "All fields are optional strings"
    explanation: "“The open content model in JSON Schema is enabled when additionalProperties is set to true, which is the default. Thus, JSON Schemas are usually a definition of what isn’t permitted ... rather than what is permitted.”"
  - id: json-schema-pain
    prompt: "Why can JSON Schema be hard to evolve compatibly?"
    options:
      - id: o5c8e7b
        text: "It has no types"
      - id: o49151c
        text: "Its power — remote references, conditional rules and more — makes reasoning about changes challenging"
        correct: true
      - id: o008b5c
        text: "It does not support objects"
      - id: o118745
        text: "It forbids versioning"
    explanation: "JSON Schema's features “make for unwieldy definitions. It can be challenging to resolve remote schemas, reason about conditional rules, or evolve schemas in a forward- or backward-compatible way”."
  - id: messagepack
    prompt: "Why does MessagePack save only a little space over JSON (66 vs 81 bytes)?"
    options:
      - id: of04983
        text: "It encodes text as UTF-16"
      - id: oa01364
        text: "Without a schema it must still include every field name in the encoded data"
        correct: true
      - id: o46d206
        text: "It adds checksums"
      - id: o4cb878
        text: "It does not support integers"
    explanation: "“since they don’t prescribe a schema, they need to include all the object field names within the encoded data.” The book questions “whether such a small space reduction ... is worth the loss of human-readability.”"
  - id: protobuf-tags
    prompt: "What replaces field names in Protocol Buffers' encoded data?"
    options:
      - id: o7df1f7
        text: "Numeric field tags from the schema"
        correct: true
      - id: o2380c7
        text: "Field positions only, with no markers"
      - id: ofb5018
        text: "Hashes of field names"
      - id: oa8a7c4
        text: "Nothing; field names are included"
    explanation: "“Instead, the encoded data contains field tags, which are numbers (1, 2, and 3). Those are the numbers that appear in the schema definition.”"
  - id: varint-range
    prompt: "In Protocol Buffers' variable-length encoding, how many bytes does 1337 take?"
    options:
      - id: ob80e21
        text: "4"
      - id: o9c8f1c
        text: "2"
        correct: true
      - id: oa9a240
        text: "8"
      - id: oca8030
        text: "1"
    explanation: "“the number 1337 is encoded in two bytes” and “numbers from –8,192 to 8,191 are encoded in two bytes”."
  - id: rename-vs-retag
    prompt: "Which Protocol Buffers schema changes are safe?"
    type: multiple
    options:
      - id: o7b33f1
        text: "Changing an existing field's tag number"
      - id: o416865
        text: "Adding a field with a new tag number"
        correct: true
      - id: o623346
        text: "Renaming a field"
        correct: true
      - id: oe210a6
        text: "Reusing the tag number of a removed field"
    explanation: "“You can change the name of a field in the schema, since the encoded data never refers to field names, but you cannot change a field’s tag” and “You can never use the same tag number again”."
  - id: protobuf-forward
    prompt: "How does old Protocol Buffers code cope with a field tag it does not recognize?"
    options:
      - id: o6f2239
        text: "It crashes"
      - id: o6aa510
        text: "It treats the field as a string"
      - id: ob06718
        text: "The datatype annotation tells it how many bytes to skip, and it can preserve the unknown field"
        correct: true
      - id: o062347
        text: "It asks the server for the schema"
    explanation: "“The datatype annotation allows the parser to determine how many bytes it needs to skip while preserving the unknown fields ... This maintains forward compatibility”."
  - id: protobuf-default
    prompt: "New Protocol Buffers code reads an old record lacking a new string field. What value does the field get?"
    options:
      - id: ob95044
        text: "null"
      - id: oe60386
        text: "An error is raised"
      - id: ob85f90
        text: "The field name"
      - id: o1f15f3
        text: "A default value — the empty string"
        correct: true
    explanation: "“it is filled in with a default value (e.g., the empty string if the field type is string, or 0 if it’s a number).”"
  - id: int32-64
    prompt: "You change an int32 field to int64. Which direction is risky?"
    options:
      - id: ob62228
        text: "Old code reading new data, because a large value is truncated into a 32-bit variable"
        correct: true
      - id: o442f5a
        text: "New code reading old data"
      - id: o33b335
        text: "Neither; protobuf forbids this change"
      - id: o316bc5
        text: "Both directions are equally safe"
    explanation: "“New code can easily read data written by old code ... However, if old code reads data written by new code, the old code is still using a 32-bit variable to hold the value. If the decoded 64-bit value won’t fit in 32 bits, it will be truncated.”"
  - id: avro-no-types
    prompt: "Avro's binary encoding contains no field identifiers or type markers. How is it decoded?"
    options:
      - id: o91adb4
        text: "It cannot be decoded without JSON"
      - id: oca50f7
        text: "By guessing types from byte patterns"
      - id: o63876b
        text: "By reading field names embedded in the data"
      - id: oe6f4de
        text: "By walking the schema's fields in order, using the schema to know each type — so the exact writer's schema is required"
        correct: true
    explanation: "“To parse the binary data, you go through the fields in the order that they appear in the schema ... the binary data can be decoded correctly only if the code reading the data is using the exact same schema as the code that wrote the data.”"
  - id: avro-resolution
    prompt: "How does Avro resolve differences between the writer's and reader's schemas?"
    options:
      - id: oecf72f
        text: "It matches fields by name; ignores writer-only fields; fills reader-only fields with the reader's default"
        correct: true
      - id: o573bb9
        text: "It matches fields by position only"
      - id: o3374a6
        text: "It uses tag numbers"
      - id: o7b35e5
        text: "It fails on any difference"
    explanation: "“the schema resolution matches up the fields by field name. If the code reading the data encounters a field that appears in the writer’s schema but not in the reader’s schema, it is ignored.” Missing fields are “filled in with a default value declared in the reader’s schema.”"
  - id: avro-default-rule
    prompt: "In Avro, which fields may you add or remove while keeping full compatibility?"
    options:
      - id: o948d0f
        text: "Only required fields"
      - id: oea1881
        text: "Only fields of type string"
      - id: oade080
        text: "Any field"
      - id: o1627ed
        text: "Only fields that have a default value"
        correct: true
    explanation: "“To maintain compatibility, you may add or remove only a field that has a default value”."
  - id: avro-break
    prompt: "You add an Avro field with no default. What breaks?"
    options:
      - id: ob04abf
        text: "Forward compatibility — old readers can't read new data"
      - id: of3b40b
        text: "Nothing"
      - id: o387802
        text: "Both"
      - id: ob78cc0
        text: "Backward compatibility — new readers can't read old data"
        correct: true
    explanation: "“If you were to add a field that has no default value, new readers wouldn’t be able to read data written by old writers, so you would break backward compatibility.”"
  - id: avro-null
    prompt: "How does Avro allow a field to be null?"
    options:
      - id: odd828f
        text: "Using a special nullable keyword"
      - id: oaa803f
        text: "All fields are nullable by default"
      - id: ocfaa92
        text: "Null is not supported"
      - id: o79542c
        text: "With a union type that includes null; null can be the default only if it is the first branch"
        correct: true
    explanation: "“if you want to allow a field to be null, you have to use a union type ... You can use null as a default value only if it is the first branch of the union.”"
  - id: avro-rename
    prompt: "Renaming a field via aliases in the reader's schema is which kind of compatible?"
    options:
      - id: od47692
        text: "Forward compatible but not backward compatible"
      - id: obbee60
        text: "Both"
      - id: oe1fbd9
        text: "Neither"
      - id: o3b3369
        text: "Backward compatible but not forward compatible"
        correct: true
    explanation: "“changing a field name is backward compatible but not forward compatible. Similarly, adding a branch to a union type is backward compatible but not forward compatible.”"
  - id: writer-schema
    prompt: "How does a reader learn the writer's schema for individually written database records?"
    options:
      - id: oc43276
        text: "Embed the full schema in every record"
      - id: oda1230
        text: "Always use the latest schema"
      - id: o23c809
        text: "Prefix each record with a schema version number and look the schema up in a registry"
        correct: true
      - id: o1d5628
        text: "Guess from the data"
    explanation: "“The simplest solution in this case is to include a version number at the beginning of every encoded record and keep a list of schema versions in your database.” Confluent's schema registry works this way."
  - id: writer-schema-file
    prompt: "For a large Avro file of millions of records with one schema, where is the schema stored?"
    options:
      - id: o150f17
        text: "Nowhere"
      - id: o30e313
        text: "Once at the beginning of the file (object container file)"
        correct: true
      - id: o391505
        text: "In every record"
      - id: o1815fe
        text: "In a separate registry only"
    explanation: "“the writer of that file can just include the schema once at the beginning of the file. Avro specifies a file format (object container files) to do this.”"
  - id: dynamic-schema
    prompt: "Why is Avro friendlier than Protocol Buffers for dumping a relational database whose schema changes?"
    options:
      - id: o1205a1
        text: "Protocol Buffers cannot encode integers"
      - id: ob1f93f
        text: "Avro compresses better"
      - id: o2f9aaa
        text: "Schemas can be generated automatically from table columns with no tag numbers to assign by hand"
        correct: true
      - id: o7c4174
        text: "Avro is faster to parse"
    explanation: "“Avro is friendlier to dynamically generated schemas ... if you were using Protocol Buffers for this purpose, the field tags would likely have to be assigned by hand.”"
  - id: asn1
    prompt: "What does the chapter say about ASN.1?"
    options:
      - id: oabef44
        text: "It was replaced by JSON in X.509 certificates"
      - id: oa57a5f
        text: "It is the most popular format for new web APIs"
      - id: oe5deda
        text: "It shares ideas with Protobuf, including tag-based evolution, but is complex and badly documented, so it's a poor choice for new applications"
        correct: true
      - id: obaf29d
        text: "It cannot evolve schemas"
    explanation: "ASN.1 “supports schema evolution using tag numbers, similar to Protocol Buffers. However, it’s also very complex and badly documented, so ASN.1 is probably not a good choice for new applications.”"
  - id: schema-merits
    prompt: "Which are merits of schema-based binary encodings?"
    type: multiple
    options:
      - id: od30f6f
        text: "The schema is always up-to-date documentation"
        correct: true
      - id: oa437f8
        text: "Code generation enables compile-time type checking"
        correct: true
      - id: oe9f9f4
        text: "More compact than binary JSON variants"
        correct: true
      - id: o80a52d
        text: "Data is human-readable without decoding"
      - id: od8e348
        text: "Compatibility can be checked before deployment"
        correct: true
    explanation: "The chapter lists compactness, documentation that is “up-to-date”, checking “forward and backward compatibility of schema changes before anything is deployed”, and code generation for static typing. Its downside is that data “needs to be decoded before it is human-readable.”"
---
From language-specific serialization through JSON and friends to schema-driven binary formats, and why
schemas are worth having.

## Learning objectives

- Explain the risks of language-specific serialization
- List the pitfalls of JSON, XML and CSV, and the power and pain of JSON Schema
- Explain how Protocol Buffers uses field tags for evolution
- Explain Avro's writer's and reader's schemas and its evolution rules
- Argue the merits of schemas

## In a nutshell

- Encoding (serialization) turns in-memory structures into bytes; decoding reverses it.
- Language-specific formats: lock-in, remote code execution risks, poor versioning and efficiency.
- JSON: number ambiguity, no binary strings; binary JSON variants save little.
- Protobuf: 33 bytes, field tags; never reuse a tag.
- Avro: 32 bytes, no tags; resolution by field name between writer and reader schemas; good for generated schemas.

## Connections to today

- **Deserialization vulnerabilities** (e.g., in .NET's BinaryFormatter, now removed in .NET 9) are the
  security problem the chapter warns about.
- **Schema registries** such as Confluent's and Apicurio are standard in Kafka deployments.
- **Buf and protobuf breaking-change detection** in CI automate the chapter's compatibility checks.
- **Twitter/X snowflake IDs** sent as strings are a live example of the 2^53 problem.
