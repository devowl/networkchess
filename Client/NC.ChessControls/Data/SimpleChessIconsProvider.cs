using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using NC.ChessControls.Interfaces;
using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.ChessControls.Data
{
    /// <summary>
    /// Icons provider for embedded simple icons.
    /// </summary>
    internal class SimpleChessIconsProvider : IIconsProvider
    {
        private const int ActualWidth = 2000;

        private const int ActualHeight = 667;

        private int FrameWidth = ActualWidth / 6;

        private int FrameHeight = ActualHeight / 2;

        private BitmapSource _simpleChessIconsImageSource;

        private BitmapSource SimpleChessIconsImageSource
            =>
                _simpleChessIconsImageSource ??
                (_simpleChessIconsImageSource =
                    new BitmapImage(
                        new Uri("pack://application:,,,/NC.ChessControls;component/Icons/simple-chess-pieces.png", UriKind.RelativeOrAbsolute)));

        /// <inheritdoc/>
        public Image GetIcon(ChessPiece chessPiece)
        {
            var fullName = chessPiece.ToString();
            var isBlack = fullName.StartsWith((PlayerColor.Black).ToString());
            var isWhite = fullName.StartsWith((PlayerColor.White).ToString());
            
            string name;
            if (isWhite)
            {
                name = fullName.Substring((PlayerColor.White).ToString().Length);
            }
            else if (isBlack)
            {
                name = fullName.Substring((PlayerColor.Black).ToString().Length);
            }
            else
            {
                // Unknown piece
                return null;
            }

            var offset = isWhite ? 0 : 1;
            switch (name)
            {
                case "King":
                    return GetCroppedBitmap(0, offset);
                case "Queen":
                    return GetCroppedBitmap(FrameWidth * 1, offset);
                case "Bishop":
                    return GetCroppedBitmap(FrameWidth * 2, offset);
                case "Knight":
                    return GetCroppedBitmap(FrameWidth * 3, offset);
                case "Rook":
                    return GetCroppedBitmap(FrameWidth * 4, offset);
                case "Pawn":
                    return GetCroppedBitmap(FrameWidth * 5, offset);
            }

            return null;
        }
        
        private Image GetCroppedBitmap(int x, int offsetY)
        {
            var bitmap = new CroppedBitmap(
                SimpleChessIconsImageSource,
                new Int32Rect(x, offsetY * FrameHeight, FrameWidth, FrameHeight));

            return new Image { Source = bitmap };
        }

    }
}
