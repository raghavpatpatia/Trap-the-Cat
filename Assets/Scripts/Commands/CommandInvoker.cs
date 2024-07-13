using System.Collections.Generic;

public class CommandInvoker
{
    private Stack<ICommand> commandRegistery = new Stack<ICommand>();
    public void ProcessCommand(ICommand command)
    {
        RegisterCommand(command);
        ExecuteCommand(command);
    }
    private void RegisterCommand(ICommand command) => commandRegistery.Push(command);
    private void ExecuteCommand(ICommand command) => command.Execute();
}