using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Client.ViewModels
{
    public class ChessCallback : IChessServiceCallback
    {
        public void Message(string text)
        {
            
        }

        public void Move(int fromX, int fromY, int toX, int toY, ChessPiece[,] virtualField)
        {
            
        }

        public void GameHasStarted(WcfGameInfo gameInfo)
        {
            
        }
    }
}
