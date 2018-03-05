using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Domain
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        bool Deleted { get; set; }
    }
}
