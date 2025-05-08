namespace SharedKernal.CQRS
{
  public interface ICommand { }
  public interface ICommand<TResult> : ICommand { }
}
