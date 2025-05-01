namespace Shared.Common
{
  public class AppError(string code, string message)
  {
    public string Code { get; } = code;
    public string Message { get; } = message;

    public static AppError Validation(string message) => new("validation_error", message);
    public static AppError NotFound(string message) => new("not_found", message);
    public static AppError Internal(string message) => new("internal_error", message);
  }
}
