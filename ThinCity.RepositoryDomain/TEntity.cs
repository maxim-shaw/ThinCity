using System;
using System.Collections.Generic;
using System.Text;

namespace ThinCity.RepositoryDomain
{
    /// <summary>
    /// Entity type abstaction that supports different type of entitie's primary key property.
    /// </summary>
    /// <typeparam name="ID">Entitie's primary key.</typeparam>
    public abstract class TEntity<ID>
    {
        public abstract ID GetId();
    }
}
