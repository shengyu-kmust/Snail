using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Dto
{
    public class ApiResultDto
    {
        public int Code { get; set; }
        public object Data { get; set; }
        public string Msg { get; set; }

        public static ApiResultDto SuccessResult(object data)
        {
            return new ApiResultDto
            {
                Code = ApiResultCode.success,
                Data = data
            };
        }
        public static ApiResultDto BadRequestResult(string msg)
        {
            return new ApiResultDto
            {
                Code = ApiResultCode.badRequest,
                Msg = msg
            };
        }
        public static ApiResultDto ServerErrorResult(string error)
        {
            return new ApiResultDto
            {
                Code = ApiResultCode.serverError,
                Msg = error
            };
        }

        public static ApiResultDto NoAuthenticateResult()
        {
            return new ApiResultDto
            {
                Code = ApiResultCode.noAuthenticate
            };
        }

        public static ApiResultDto NoAuthorizeResult()
        {
            return new ApiResultDto
            {
                Code = ApiResultCode.noAuthorize
            };
        }
    }

    public static class ApiResultCode
    {
        public const int success = 2000;
        public const int noAuthenticate = 4001;
        public const int noAuthorize = 4003;
        /// <summary>
        /// 业务异常
        /// </summary>
        public const int badRequest = 4000;
        /// <summary>
        /// 服务器异常
        /// </summary>
        public const int serverError = 5000;
    }
}
