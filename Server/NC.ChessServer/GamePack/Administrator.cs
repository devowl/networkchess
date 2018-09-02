using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using NC.ChessServer.Interfaces;
using NC.Shared.Contracts;
using NC.Shared.Exceptions;

namespace NC.ChessServer.GamePack
{
    /// <summary>
    /// Service administrator.
    /// </summary>
    public class Administrator : IStartable, IDisposable, IPlayerManager, IGameManager
    {
        private const int SecondsTimeout = 3;

        private TimeSpan _inactivityTimeout = TimeSpan.FromMinutes(5);

        private readonly Game.Factory _gameFactory;

        private Task _gameMakerMonitor;

        private Task _playerActivityMonitor; 

        private readonly CancellationToken _shutdownToken = new CancellationToken();

        private CancellationTokenSource _cancellationTokenSource;

        private CancellationToken _cancelationToken;

        private readonly SpecialQueue<Player> _playersQueue = new SpecialQueue<Player>();

        private readonly object _queueSyncObj = new object();

        private readonly object _playingGamesSyncObj = new object();

        private readonly List<Game> _playingGames = new List<Game>();

        /// <summary>
        /// Constructor for <see cref="Administrator"/>.
        /// </summary>
        public Administrator(Game.Factory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancelationToken = _cancellationTokenSource.Token;

            _gameMakerMonitor = Task.Factory.StartNew(
                GameMonitorThread,
                _shutdownToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);

            _playerActivityMonitor = Task.Factory.StartNew(
                ActivityMonitor,
                _shutdownToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        private void GameMonitorThread()
        {
            while (!_cancelationToken.IsCancellationRequested)
            {
                lock (_queueSyncObj)
                {
                    var tempQueue = new Queue<Player>(_playersQueue.Where(player => player.IsReady));
                    while (tempQueue.Count >= 2)
                    {
                        var player1 = tempQueue.Dequeue();
                        var player2 = tempQueue.Dequeue();

                        _playersQueue.Remove(player1);
                        _playersQueue.Remove(player2);
                        var game = _gameFactory(player1, player2);

                        
                        AddGame(game);
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(SecondsTimeout));
            }
        }

        private void ActivityMonitor()
        {
            while (!_cancelationToken.IsCancellationRequested)
            {
                // _inactivityTimeout
                Thread.Sleep(TimeSpan.FromSeconds(SecondsTimeout));
            }
        }

        private void AddGame(Game game)
        {
            lock (_playingGamesSyncObj)
            {
                _playingGames.Add(game);
                game.GameEnded += OnGameEnded;
                game.Initialize();
                _playingGames.Add(game);
            }
        }

        private void OnGameEnded(object sender, EventArgs eventArgs)
        {
            lock (_playingGamesSyncObj)
            {
                // some statistics
                var game = (Game)sender;
                _playingGames.Remove(game);
            }
        }

        /// <inheritdoc/>
        public void AddToQueue(Player player)
        {
            lock (_queueSyncObj)
            {
                _playersQueue.Enqueue(player);
            }
        }

        /// <inheritdoc/>
        public void RemoveFromQueue(string sessionId)
        {
            lock (_queueSyncObj)
            {
                var player = _playersQueue.FirstOrDefault(p => p.SessionId == sessionId);
                if (player != null)
                {
                    _playersQueue.Remove(player);
                }
            }
        }

        /// <inheritdoc/>
        public void Ready(string sessionId, IChessServiceCallback callback)
        {
            lock (_queueSyncObj)
            {
                var player = _playersQueue.FirstOrDefault(p => p.SessionId == sessionId);
                if (player == null)
                {
                    throw new SessionNotFoundedException();
                }

                Player.SetCallback(player, callback);
                Player.SetActive(player);
                Player.SetIsReady(player);
            }
        }

        /// <inheritdoc/>
        public bool HasSession(string sessionId)
        {
            lock (_queueSyncObj)
            {
                return _playersQueue.Any(p => p.SessionId == sessionId);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _gameMakerMonitor.Dispose();
            _playerActivityMonitor.Dispose();
        }

        /// <inheritdoc/>
        public void Move(string sessionId, int x1, int y1, int x2, int y2)
        {
            lock (_playingGamesSyncObj)
            {
                var game =
                    _playingGames.FirstOrDefault(
                        g => g.Player1.SessionId == sessionId || g.Player2.SessionId == sessionId);

                if (game == null)
                {
                    throw new SessionNotFoundedException();
                }

                game.Move(sessionId, x1, y1, x2, y2);
            }
        }
    }
}