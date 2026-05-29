public class AgentService
{
    private readonly OpenRouterService _llm;
    private readonly ILogger<AgentService> _logger;

    public AgentService(OpenRouterService llm, ILogger<AgentService> logger)
    {
        _llm = llm ?? throw new ArgumentNullException(nameof(llm));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ModifyCodeAsync(
        string code,
        string instruction,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));
            
        if (string.IsNullOrWhiteSpace(instruction))
            throw new ArgumentException("Instruction cannot be empty", nameof(instruction));

        try
        {
            var prompt = $@"
You are an expert software engineer.

TASK:
{instruction.Replace("\"", "\\\"")}

CODE:
{code.Replace("\"", "\\\"")}

Return ONLY updated code.";

            _logger.LogDebug("Sending prompt to LLM: {Prompt}", prompt);
            var result = await _llm.Ask(prompt);
            _logger.LogDebug("Received LLM response: {Response}", result);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to modify code");
            throw;
        }
    }
}