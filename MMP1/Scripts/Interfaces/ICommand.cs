public interface ICommand
{
    object subject { get; set; }
    void execute();
}