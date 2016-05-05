using DAL.Entity.Models;
using DAL.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace BLL.Service
{
    public interface IBaseService : IDisposable
    {
        string GetCurrentUser();
        PostResponse GetDataInvalidError();
        PostResponse GetModelStateError(ModelStateDictionary modelState);
        PostResponse GetExceptionError(Exception ex);
        PostResponse GetExceptionError(Exception ex, int characterAmount);
    }
}
