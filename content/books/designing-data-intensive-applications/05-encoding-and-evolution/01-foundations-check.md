---
title: "Foundations Check — Before Chapter 5"
questions:
  - id: double-precision
    prompt: "Why can't a 64-bit IEEE 754 double represent every 64-bit integer exactly?"
    options:
      - id: o367e95
        text: "Doubles cannot represent negative numbers"
      - id: odd56f4
        text: "It has only a 53-bit significand, so integers above 2^53 lose precision"
        correct: true
      - id: o87b800
        text: "Doubles store integers as text"
      - id: of0e583
        text: "Doubles are only 32 bits wide"
    explanation: "A double has 52 explicit significand bits plus an implicit leading bit. Beyond 2^53, consecutive integers can no longer all be represented — the root of the JavaScript ID problem in the chapter."
  - id: base64
    prompt: "Base64 encodes every 3 bytes as 4 ASCII characters. What is the size overhead?"
    options:
      - id: o24eeff
        text: "None"
      - id: o0b0d89
        text: "About 33%"
        correct: true
      - id: o25d053
        text: "About 10%"
      - id: oe8f867
        text: "About 50%"
    explanation: "4/3 ≈ 1.33. The chapter notes Base64 “increases the data size by about a third”."
  - id: utf8
    prompt: "In UTF-8, how many bytes does the ASCII letter \"A\" take?"
    options:
      - id: o1a7d5e
        text: "4"
      - id: ob821b6
        text: "Depends on the font"
      - id: o2d9a6c
        text: "1"
        correct: true
      - id: o9aca9f
        text: "2"
    explanation: "UTF-8 is ASCII-compatible: code points below 128 take one byte. Other characters take 2–4 bytes."
  - id: varint
    prompt: "A variable-length integer encoding uses the top bit of each byte as a \"more bytes follow\" flag. How many value bits does each byte carry?"
    options:
      - id: o0464cd
        text: "4"
      - id: o3196ae
        text: "7"
        correct: true
      - id: ofe9493
        text: "8"
      - id: o0363a8
        text: "16"
    explanation: "One bit is the continuation flag; seven carry the value. Protocol Buffers and Avro both use variable-length integers."
  - id: http-idempotent
    prompt: "Which HTTP method is defined as idempotent?"
    options:
      - id: o9864bd
        text: "PUT"
        correct: true
      - id: ofd8b3e
        text: "PATCH (in general)"
      - id: o6f6853
        text: "CONNECT"
      - id: oa200de
        text: "POST"
    explanation: "Repeating a PUT with the same body should leave the resource in the same state. POST is not idempotent by definition — which matters when retrying after a timeout."
  - id: http-status
    prompt: "A REST API returns 503. What does that usually mean?"
    options:
      - id: o17ae2c
        text: "The service is temporarily unavailable, often overloaded or in maintenance"
        correct: true
      - id: o0d3162
        text: "The request succeeded"
      - id: o220b64
        text: "The client is unauthorized"
      - id: ob5e5e6
        text: "The resource was not found"
    explanation: "5xx codes indicate server-side failures; 503 Service Unavailable often comes with a Retry-After header."
  - id: endianness
    prompt: "What does \"endianness\" describe?"
    options:
      - id: of4614c
        text: "The order in which the bytes of a multi-byte number are stored"
        correct: true
      - id: o5ad425
        text: "The character encoding of text"
      - id: o3747a3
        text: "The number of bits in a byte"
      - id: od32783
        text: "The length of a string"
    explanation: "Little-endian stores the least significant byte first. Portable binary encodings must specify byte order."
---
Chapter 5 assumes basic knowledge of bytes, character encodings, floating-point numbers and HTTP. These
questions are not drawn from the book's text.

## Learning objectives

- Recall how integers, floats and text are represented in bytes
- Recall Base64 overhead and HTTP verbs and status codes
