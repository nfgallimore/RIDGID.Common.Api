﻿using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using RIDGID.Common.Api.Core.ErrorHandling;
using RIDGID.Common.Api.Core.Objects;

namespace RIDGID.Common.Api.Core.Controllers
{
    public class RidgidApiController : ApiController
    {
        public IHttpActionResult ErrorResponse(int errorId, string debugErrorMessage, HttpStatusCode httpStatusCode)
        {
            var errors = new List<ErrorMessage>
            {
                new ErrorMessage
                {
                    DebugErrorMessage = debugErrorMessage,
                    ErrorId = errorId
                }
            };
            var errorsResponse = new ErrorsResponse
            {
                Errors = errors
            };
            return new HttpGenericResult(this, httpStatusCode, errorsResponse);
        }

        public virtual IHttpActionResult Created()
        {
            return new HttpGenericResult(this, HttpStatusCode.Created, null);
        }

        public virtual IHttpActionResult NoContent()
        {
            return new HttpGenericResult(this, HttpStatusCode.NoContent, null);
        }

        public virtual IHttpActionResult Response(ErrorsResponse errorsResponse, HttpStatusCode httpStatusCode)
        {
            return new HttpGenericResult(this, httpStatusCode, errorsResponse);
        }

        public virtual IHttpActionResult BadRequest(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.BadRequest);
        }

        public virtual IHttpActionResult Conflict(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.Conflict);
        }

        public virtual IHttpActionResult NotFound(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.NotFound);
        }

        public virtual IHttpActionResult Forbidden(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.Forbidden);
        }

        public virtual IHttpActionResult Unauthorized(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.Unauthorized);
        }

        public virtual IHttpActionResult InternalServerError(int errorId, string debugErrorMessage)
        {
            return this.ErrorResponse(errorId, debugErrorMessage, HttpStatusCode.InternalServerError);
        }
    }
}