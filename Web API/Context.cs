using Core.Errors;
using Core.Models;
using WebAPI.Services;
using WebAPI.Views;

namespace WebAPI;

public class Context {
   public Context(Core.Logic core, IHttpContextAccessor context) {
      Core = core;
      ContextAccessor = context;
   }

   readonly Core.Logic Core;
   readonly IHttpContextAccessor ContextAccessor;

   void SetStatusCode(int code) {
      ContextAccessor.HttpContext!.Response.StatusCode = code;
   }

   T Ok<T>(T data) {
      SetStatusCode(StatusCodes.Status200OK);
      return data;
   }

   T NotFound<T>(T data) {
      SetStatusCode(StatusCodes.Status404NotFound);
      return data;
   }

   T Unauthorized<T>(T data) {
      SetStatusCode(StatusCodes.Status401Unauthorized);
      return data;
   }

   T BadRequest<T>(T data) {
      SetStatusCode(StatusCodes.Status400BadRequest);
      return data;
   }

   T InternalError<T>(T data) {
      SetStatusCode(StatusCodes.Status500InternalServerError);
      return data;
   }

   public Task<Response<T>> Execute<T>(Func<Core.Logic, Task<T>> responder) => WrapResponse(() => responder(Core));

   public Task<Response<T>> ExecuteAuthenticated<T>(Func<User, Core.Logic, Task<T>> responder) => WrapResponse(async () => {
      string token = ContextAccessor.HttpContext.Request.Headers["Authentication"];
      var user = await Core.Authenticate(token?.Split(' ')?.Last())!;
      return await responder(user, Core);
   });

   async Task<Response<T>> WrapResponse<T>(Func<Task<T>> function) {
      try {
         var data = await function();
         if (data is Task)
            throw new Exception("Endpoint returned Task not awaited.  \nCheck endpoint body in API controller; await the task.");
         return Ok(Response<T>.Succeed(data));
      } catch (Exception ex) {
         var uex = Unwrap(ex);
         var error = uex as Error;
         if (error is not null) {
            var response = Response<T>.Fail(error);
            if (error is NotFound)
               return NotFound(response);
            if (error is NotAuthenticated or NotAuthorized)
               return Unauthorized(response);
            return BadRequest(response);
         } else {
            LogService.LogException(ex, uex.GetType().Name);
            return InternalError(Response<T>.Fail(uex));
         }
      }

      static Exception Unwrap(Exception ex) => ex.InnerException is not null ? Unwrap(ex.InnerException) : ex;
   }
}
