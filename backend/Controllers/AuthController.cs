using Models.Connection;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Login;
using Data.DataBaseOperations;
using Services.AuthServices;


namespace Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<LoginResponse> Post(LoginRequest request) {
        int usr_id = DbOps.UserId(request.UserEmail);

        if (Authentication.validate_password(usr_id, request.Password))
        {
            int user_id = DbOps.UserId(request.UserEmail);
            var token = "token";

            return LoginResponse.Success(user_id.ToString(), token);

        }

        return LoginResponse.Error();
    }


    [HttpGet("recover_acc/{email}")]
    public ActionResult<RecoverAccResponse> Get(string email)
    {
        if (Authentication.send_recover_code(email))
        {
            return RecoverAccResponse.Success();
        }

        return RecoverAccResponse.Error();
    }


    [HttpPost("recover_acc")]
    public ActionResult<RecoverAccTokenValidationResponse> Post(RecoverAccTokenValidationRequest request)
    {
        if (Authentication.validate_recovery_code(request.UserEmail, request.RecoverCode))
        {
            int user_id = DbOps.UserId(request.UserEmail);

            string token = Authentication.gen_token(user_id);

            return RecoverAccTokenValidationResponse.Success(token);
        }
        
        return RecoverAccTokenValidationResponse.Error();
    }

    [HttpPost("change_passw")]
    public ActionResult<ChangePasswResponse> Post(ChangePasswRequest request)
    {
        int user_id = DbOps.UserId(request.UserEmail);

        if (Authentication.change_passw(user_id, request.OldPassword, request.NewPassword))
        {
            return ChangePasswResponse.Success();
        }

        return ChangePasswResponse.Error();
    }

}