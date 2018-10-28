using System;
using ConEmu.TasksEditor.Enums;

namespace ConEmu.TasksEditor.Models
{
    public class ConEmuArgs : EventArgs
    {
        public ConEmuTask Task { get; }
        public CloseReason CloseReason { get; }

        public ConEmuArgs(CloseReason closeReason, ConEmuTask task)
        {
            CloseReason = closeReason;
            Task = task;
        }
    }
}