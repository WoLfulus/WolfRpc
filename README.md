# WolfRpc

Even though it has RPC in the name, this isn't that **remote** since transport is not something I'm worried about. If service calls can be dispatched from a serialized payload and results are serialized with caller identification, the transport is just a detail.

## Goals

- [ ] Everything must be async by default
- [ ] Requests (calls) must be serializable
- [ ] Responses (results) must be serializable
- [ ] Must support exceptions
- [ ] All requests and responses must be identifiable and give context to be matched as a pair
- [ ] Must play well with any kind of transport (socket, web, udp, message queues, ...)
- [ ] Must be able to use it in Unity (both server and consumer)
  - [ ] Must be compatible with il2cpp
- [ ] Must be able to use it in net6+ (both server and consumer)

