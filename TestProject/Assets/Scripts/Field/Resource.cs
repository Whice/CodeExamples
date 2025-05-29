using System;
using Utility;

namespace View
{
    public class Resource : MonoBehaviourLogger
    {
        public event Action<Resource> readyChanged;
        private bool _isReady = true;
        public bool isReady
        {
            get => _isReady;
            set
            {
                if (_isReady != value)
                {
                    _isReady = value;
                    readyChanged?.Invoke(this);
                    SetActiveObject(value);
                }
            }
        }
    }
}