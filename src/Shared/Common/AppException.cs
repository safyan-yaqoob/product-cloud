namespace Shared.Common
{
  public class AppException : Exception
  {
    public AppError Error { get; }

    public AppException(AppError error) : base(error.Message)
    {
      Error = error;
    }
  }
}
