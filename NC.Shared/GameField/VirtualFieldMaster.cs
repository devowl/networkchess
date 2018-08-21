using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NC.Shared.Data;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Game field master.
    /// </summary>
    public class VirtualFieldMaster
    {
        private readonly VirtualField _virtualField;

        /// <summary>
        /// Constructor for <see cref="VirtualFieldMaster"/>.
        /// </summary>
        public VirtualFieldMaster(VirtualField virtualField)
        {
            _virtualField = virtualField;
        }
    }
}
