using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileService _fileService;

    public FilesController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var files = await _fileService.GetFilesAsync();
        var feFile = await _fileService.GetFeFilesAsync();
        return Ok(new
        {
            files,
            feFile
        });
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetFileContent([FromQuery] string path)
    {
        if (string.IsNullOrEmpty(path))
            return BadRequest("Path is required.");

        try
        {
            var content = await _fileService.ReadFileAsync(path);
            return Ok(content);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}