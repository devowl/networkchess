using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC.ChessServer.Interfaces
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public interface IGameManager
    {
        void Move(string sessionId, int x1, int y1, int x2, int y2);
    }
}
