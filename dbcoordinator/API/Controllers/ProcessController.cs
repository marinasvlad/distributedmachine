using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProcessController : BaseApiController
{
    private readonly IAccountRepository _accountRepo;
    public ProcessController(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

    [HttpGet]
    public async Task<ActionResult<int>> Process()
    {
        Thread.Sleep(10000);
        var useri = await _accountRepo.GetUserByEmailAsync("email1@distributedmachine.ro");
        return Ok(10);
    }
}
