namespace WebAPI.Views;

public class Response<T> {
   public bool          Succeeded { get; set; }
   public T             Data      { get; set; }
   public ResponseError Error     { get; set; }

   public static Response<T> Succeed(T data) => new Response<T> { Succeeded = true, Data = data };
   public static Response<T> Fail(Core.Errors.Error error) => new Response<T> { Succeeded = false, Error = new ResponseError(error) };
   public static Response<T> Fail(Exception ex) => new Response<T> { Succeeded = false, Error = new ResponseError(new Core.Errors.Error(ex)) };
}

public class ResponseError {
   public string Code    { get; set; }
   public string Message { get; set; }

   public ResponseError(string code, string message) { Code = code; Message = message; }
   public ResponseError(Core.Errors.Error error): this(error.Code, error.Message) { }
}