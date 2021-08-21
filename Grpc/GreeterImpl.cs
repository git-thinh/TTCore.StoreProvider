using Grpc.Core;
using Helloworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.Grpc
{
   public class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
    }
}
