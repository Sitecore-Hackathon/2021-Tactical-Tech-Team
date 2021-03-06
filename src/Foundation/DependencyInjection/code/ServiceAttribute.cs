using System;

namespace TTT.Foundation.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class ServiceAttribute : Attribute
  {
    public ServiceAttribute()
    {
    }

    public ServiceAttribute(Type serviceType)
    {
      this.ServiceType = serviceType;
    }

    public ServiceAttribute(Type serviceType, Lifetime lifeTime)
    {
      this.ServiceType = serviceType;
      this.Lifetime = lifeTime;
    }

    public Lifetime Lifetime { get; set; } = Lifetime.Singleton;

    public Type ServiceType { get; set; }
  }
}