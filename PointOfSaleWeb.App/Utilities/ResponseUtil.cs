using Microsoft.AspNetCore.Mvc;

namespace PointOfSaleWeb.App.Utilities
{
  public static class ResponseUtil
  {
    public static IResult CreateErrorResponse(string title, string detail, int statusCode) =>
        Results.BadRequest(new ProblemDetails
        {
          Title = title,
          Detail = detail,
          Status = statusCode
        });

    public static IResult CreateNotFoundResponse(string title, string detail) =>
        Results.NotFound(new ProblemDetails
        {
          Title = title,
          Detail = detail,
          Status = StatusCodes.Status404NotFound
        });
  }
}