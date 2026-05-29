using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/agent")]
public class AgentController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly AgentService _agentService;

    public AgentController(FileService fileService, AgentService agentService)
    {
        _fileService = fileService;
        _agentService = agentService;
    }

    [HttpPost("modify")]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Path) || string.IsNullOrWhiteSpace(request.Instruction))
        {
            return BadRequest("Path and Instruction are required.");
        }

        try
        {
            var code = await _fileService.ReadFileAsync(request.Path);
            var updatedCode = await _agentService.ModifyCodeAsync(code, request.Instruction);
            await _fileService.SaveFileAsync(request.Path, updatedCode);
            
            return Ok(updatedCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing request: {ex.Message}");
        }
    }
}

public class ModifyRequest
{
    public string Path { get; set; }
    public string Instruction { get; set; }
}