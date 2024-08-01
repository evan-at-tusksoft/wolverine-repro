## Debugging

1. Navigate to the `WolverineRepro` project directory and run `docker compose up -d`
2. Start debugging the app
3. Using the Swagger UI, execute the `sendmessage` endpoint
4. Observe the following exception on each retry attempt:
```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Wolverine.Runtime.WorkerQueues.DurableReceiver.<>c.<.ctor>b__15_2(Envelope env, CancellationToken _) in /home/runner/work/wolverine/wolverine/src/Wolverine/Runtime/WorkerQueues/DurableReceiver.cs:line 73
   at Wolverine.Util.Dataflow.LambdaItemHandler`1.ExecuteAsync(T message, CancellationToken cancellation) in /home/runner/work/wolverine/wolverine/src/Wolverine/Util/Dataflow/RetryBlock.cs:line 23
   at Wolverine.Util.Dataflow.RetryBlock`1.PostAsync(T message) in /home/runner/work/wolverine/wolverine/src/Wolverine/Util/Dataflow/RetryBlock.cs:line 94
```
5. Wait for two retries to occur
6. Execute the `sendmessage` endpoint again
7. Observe the following exception thrown immediately:
```
System.ArgumentOutOfRangeException: Erroneous persistence of an incoming envelope to 'any' node (Parameter 'Envelope')
   at Wolverine.RDBMS.MessageDatabase`1.StoreIncomingAsync(Envelope envelope) in /home/runner/work/wolverine/wolverine/src/Persistence/Wolverine.RDBMS/MessageDatabase.Incoming.cs:line 52
   at Wolverine.Transports.Local.DurableLocalQueue.storeAndEnqueueAsync(Envelope envelope) in /home/runner/work/wolverine/wolverine/src/Wolverine/Transports/Local/DurableLocalQueue.cs:line 200
   at Wolverine.Util.Dataflow.RetryBlock`1.PostAsync(T message) in /home/runner/work/wolverine/wolverine/src/Wolverine/Util/Dataflow/RetryBlock.cs:line 94

```