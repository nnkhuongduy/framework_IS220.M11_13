using _99phantram.Entities;
using System.Collections.Generic;

namespace _99phantram.Models
{
  public class ServiceTypeBody
  {
    public string Name { get; set; }
    public Dictionary<string, object> Value {get; set;}
    public ServiceTypeStatus Status {get; set;}
    public ServiceTypeBody() { }
  }
}