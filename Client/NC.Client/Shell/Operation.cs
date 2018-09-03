using System;

using NC.Client.ViewModels;

namespace NC.Client.Shell
{
    /// <summary>
    /// Operation executor.
    /// </summary>
    public class WaitOperation : IDisposable
    {
        private readonly WaitViewModel _waitViewModel;

        /// <summary>
        /// Constructor for <see cref="WaitOperation"/>.
        /// </summary>
        public WaitOperation(WaitViewModel waitViewModel)
        {
            if (waitViewModel == null)
            {
                throw new ArgumentNullException(nameof(waitViewModel));
            }

            _waitViewModel = waitViewModel;
            _waitViewModel.Waiting = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _waitViewModel.Waiting = false;
        }
    }
}