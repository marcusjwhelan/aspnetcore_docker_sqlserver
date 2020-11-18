using System.Collections.Generic;
using Commander.Models;

namespace Commander.Repository
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>
            {
                new Command{ Id = 0, HowTo = "Boil an egg", Line = "Boil water", Platform = "Kettle & Pan"},
                new Command{ Id = 1, HowTo = "Cut Bread", Line = "Get a Knife", Platform = "Knife & chopping board"},
                new Command{ Id = 2, HowTo = "Make cup of tea", Line = "Place tea bag in cup", Platform = "Kettle & Cup"}
            };
            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command{ Id = 0, HowTo = "Boil an egg", Line = "Boil water", Platform = "Kettle & Pan"};
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}