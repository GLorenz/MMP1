public interface IInputCommand : ICommand
{
    Input ToInput(bool shouldShare);
}