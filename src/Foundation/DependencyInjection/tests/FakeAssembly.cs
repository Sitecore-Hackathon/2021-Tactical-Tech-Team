using System.Reflection;
using System.Runtime.Serialization;

namespace TTT.Foundation.DependencyInjection.Tests
{
    public abstract class FakeAssembly : Assembly
  {
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
    }
  }
}
