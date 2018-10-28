using System;
using System.Collections;
using System.Collections.Generic;

namespace ConEmu.TasksEditor.Models
{
    /**
typedef DWORD CETASKFLAGS;
const CETASKFLAGS
	CETF_NEW_DEFAULT    = 0x0001,
	CETF_CMD_DEFAULT    = 0x0002,
	CETF_NO_TASKBAR     = 0x0004,
	CETF_ADD_TOOLBAR    = 0x0008,
	CETF_FLAGS_MASK     = 0xFFFF, // Supported flags mask
	CETF_DEFAULT4NEW    = CETF_NO_TASKBAR, // Default flags for newly created task
	CETF_MAKE_UNIQUE    = 0x40000000,
	CETF_DONT_CHANGE    = 0x80000000,
	CETF_NONE           = 0;
     */
    public sealed class ConEmuTask : IEquatable<ConEmuTask>, IEnumerable<Command>
    {
        public Guid Guid { get; private set; }
        public string Name { get; set; }
        public string GuiArgs { get; set; }
        public List<Command> Commands { get; set; }
        public bool Active { get; set; }
        public int Count { get; set; }
        public string Hotkey { get; set; }
        public string Flags { get; set; }


        public ConEmuTask()
        {
            Guid = Guid.NewGuid();
            Commands = new List<Command>();
        }

        public ConEmuTask Clone()
        {
            ConEmuTask task = new ConEmuTask
            {
                Guid = Guid,
                Name = Name,
                GuiArgs = GuiArgs,
                Commands = new List<Command>(Commands),
                Active = Active,
                Count = Count,
                Hotkey = Hotkey,
                Flags = Flags
            };

            return task;
        }

        public void AddCommand(Command command)
        {
            Commands.Add(command);
        }

        public void RemoveCommand(Command command)
        {
            Commands.Remove(command);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Command> GetEnumerator()
        {
            foreach (Command command in Commands)
            {
                yield return command;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConEmuTask);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Guid.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GuiArgs != null ? GuiArgs.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Commands != null ? Commands.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Active.GetHashCode();
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ (Hotkey != null ? Hotkey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Flags != null ? Flags.GetHashCode() : 0);

                return hashCode;
            }
        }

        public bool Equals(ConEmuTask other)
        {
            return other != null &&
                   Guid.Equals(other.Guid) &&
                   Name == other.Name &&
                   GuiArgs == other.GuiArgs &&
                   EqualityComparer<List<Command>>.Default.Equals(Commands, other.Commands) &&
                   Active == other.Active &&
                   Count == other.Count &&
                   Hotkey == other.Hotkey &&
                   Flags == other.Flags;
        }

        public static bool operator ==(ConEmuTask task1, ConEmuTask task2)
        {
            return EqualityComparer<ConEmuTask>.Default.Equals(task1, task2);
        }

        public static bool operator !=(ConEmuTask task1, ConEmuTask task2)
        {
            return !(task1 == task2);
        }
    }
}