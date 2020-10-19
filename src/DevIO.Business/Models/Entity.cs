using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Models
{

    /// <summary>
    /// Todas as classes irao derivar desta classe base. É uma classe abstrata, isto é, só pode ser herdada.
    /// </summary>
    public abstract class Entity
    {

        public Guid Id { get; set; } = Guid.NewGuid();
    
    }

}
