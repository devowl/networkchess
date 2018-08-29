using NC.ChessControls.Data;
using NC.ChessControls.Prism;
using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Virtual chess field view model.
    /// </summary>
    public class GameViewModel : NotificationObject
    {
        private VirtualField _gameField;

        private GameController _controller;

        /// <summary>
        /// Constructor for <see cref="GameViewModel"/>.
        /// </summary>
        public GameViewModel()
        {
            var chessDefaultField = VirtualFieldUtils.CreateDefaultField();
            _gameField = new VirtualField(chessDefaultField);
            _controller = new GameController();
            _controller.Movement += OnChessPieceMovement;
        }

        /// <summary>
        /// Chess game field.
        /// </summary>
        public VirtualField GameField
        {
            get
            {
                return _gameField;
            }

            private set
            {
                _gameField = value;
                RaisePropertyChanged(() => GameField);
            }
        }

        /// <summary>
        /// Game controller.
        /// </summary>
        public GameController Controller
        {
            get
            {
                return _controller;
            }

            private set
            {
                _controller = value;
            }
        }

        private void OnChessPieceMovement(object sender, MovementArgs args)
        {
            var piece = _gameField[(int)args.From.X, (int)args.From.Y];
            _gameField[(int)args.To.X, (int)args.To.Y] = piece;
            _gameField[(int)args.From.X, (int)args.From.Y] = ChessPiece.Empty;

            GameField = new VirtualField(_gameField.CloneMatrix());
        }
    }
}