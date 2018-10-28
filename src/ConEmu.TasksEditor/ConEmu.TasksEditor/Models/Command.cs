using System.Collections.Generic;

namespace ConEmu.TasksEditor.Models
{
    public class Command
    {
        public string CommandString { get; set; }

        public Command(string command)
        {
            CommandString = command;
        }

        public static implicit operator string(Command cmd)
        {
            return cmd.CommandString;
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return CommandString;
        }
    }
}