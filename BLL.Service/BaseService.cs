using DAL.Entity.Models;
using DAL.Entity.ViewModels;
using DAL.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ModelBinding;

namespace BLL.Service
{
    public class BaseService : IBaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseService([Dependency("UnitOfWork")] IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public string GetCurrentUser()
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            return currentUser;
        }

        public PostResponse GetDataInvalidError()
        {
            return new PostResponse { ResponseCode = "1001", ResponseMsg = "Input data is invalid." };
        }

        public PostResponse GetModelStateError(ModelStateDictionary modelState)
        {
            string errorMessage = string.Empty;
            foreach (var state in modelState.Values)
            {
                errorMessage += string.Format("{0},", (string.IsNullOrEmpty(state.Errors[0].ErrorMessage) ? state.Errors[0].Exception.Message.ToString() : state.Errors[0].ErrorMessage));
            }
            errorMessage = errorMessage.Substring(0, errorMessage.Length - 1);

            return new PostResponse { ResponseCode = "1002", ResponseMsg = errorMessage };
        }

        #region ### Exception Error ###
        private Exception GetException(Exception ex)
        {
            Exception exception = ex;
            if (exception.InnerException != null)
                exception = exception.InnerException;

            return exception;
        }

        /// <summary>
        /// Get exception message
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public PostResponse GetExceptionError(Exception ex)
        {
            Exception exception = GetException(ex);
            if (exception.InnerException != null)
                exception = exception.InnerException;

            return new PostResponse { ResponseCode = "9999", ResponseMsg = exception.Message };
        }

        /// <summary>
        /// Get exception message with character amount.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="characterAmount"></param>
        /// <returns></returns>
        public PostResponse GetExceptionError(Exception ex, int characterAmount)
        {
            string errorMessage = string.Empty;
            Exception exception = GetException(ex);
            if (exception.InnerException != null)
                exception = exception.InnerException;

            errorMessage = exception.Message.Length > characterAmount ? exception.Message.Substring(0, characterAmount) : exception.Message;

            return new PostResponse { ResponseCode = "9999", ResponseMsg = exception.Message };
        }

        #endregion
    }
}
