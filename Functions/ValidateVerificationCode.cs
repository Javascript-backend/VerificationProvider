using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Services;

namespace VerificationProvider.Functions
{
    public class ValidateVerificationCode
    {
        private readonly ILogger<ValidateVerificationCode> _logger;
        private readonly ValidateVerificationCodeService _service;

        public ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, ValidateVerificationCodeService service)
        {
            _logger = logger;
            _service = service;
        }

        [Function("ValidateVerificationCode")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "validate")] HttpRequest req)
        {
            try
            {
                var validateRequest = await _service.UnpackValidateRequestAsync(req);
                if (validateRequest != null)
                {
                    var validateResult = await _service.ValidateCodeAsync(validateRequest);
                    if (validateResult)
                    {
                        return new OkResult();
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"ERROR : ValidateVerificationCode.Run  :: {ex.Message}");
            }

            return new UnauthorizedResult();
        }
    }
}
