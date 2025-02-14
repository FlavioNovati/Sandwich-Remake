using System.Collections.Generic;

using Input_System;
using Grid_System;

namespace Record_System
{
    public class RecordController
    {
        public delegate void RecordControllerCallback();
        public event RecordControllerCallback OnNewEntryCallback;

        public Stack<RecordEntry> RecordStack => _recordStack;
        private Stack<RecordEntry> _recordStack;

        public RecordController()
        {
            _recordStack = new Stack<RecordEntry>();
        }

        public void AddEntry(PlateCell cell, SwipeDirection direction)
        {
            _recordStack.Push(new RecordEntry(cell, direction));
            OnNewEntryCallback?.Invoke();
        }
    }
}
