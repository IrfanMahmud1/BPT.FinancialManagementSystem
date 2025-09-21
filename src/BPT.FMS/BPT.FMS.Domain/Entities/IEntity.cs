using System;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
