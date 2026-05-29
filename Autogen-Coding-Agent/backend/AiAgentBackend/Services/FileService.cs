public class FileService
{
    private readonly IWebHostEnvironment _env;

    public string ProjectPath { get; set; }
    public string RootPath { get; set; }
    public string RootPathFE { get; set; }

    public FileService(IWebHostEnvironment env)
    {
        ProjectPath = env.ContentRootPath;
        if (ProjectPath.Contains("\\bin\\"))
        {
            ProjectPath = ProjectPath.Substring(0, ProjectPath.IndexOf("\\bin\\"));
        }
        RootPath = @$"{ ProjectPath }";
        RootPathFE = @"D:\Projects\POCS\MS-Autogen-projects\Projects\Autogen-Coding-Agent\frontend\src";
    }

    public async Task<IEnumerable<string>> GetFilesAsync()
    {
        return await Task.Run(() =>
        {
            var result = new List<string>();

            result.Add(ProjectPath);

            string rootFile = Path.Combine(RootPath, "Program.cs");
            if (File.Exists(rootFile))
                result.Add(rootFile);


            string[] allowedFolders =
            {
                Path.Combine(RootPath, "Controllers"),
                Path.Combine(RootPath, "Services")
            };

            foreach (var folder in allowedFolders)
            {
                if (Directory.Exists(folder))
                {
                    result.AddRange(Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories));
                }
            }

            return result;
        });
    }

    public async Task<IEnumerable<string>> GetFeFilesAsync()
    {
        return await Task.Run(() =>
        {
            var result = new List<string>();

            string rootFileApp = Path.Combine(RootPathFE, "app.js");
            if (File.Exists(rootFileApp))
                result.Add(rootFileApp);

            string rootFileCss = Path.Combine(RootPathFE, "index.css");
            if (File.Exists(rootFileCss))
                result.Add(rootFileCss);

            string[] allowedFEFolders =
            {
                Path.Combine(RootPathFE, "components"),
            };

            foreach (var folder in allowedFEFolders)
            {
                if (Directory.Exists(folder))
                {
                    result.AddRange(Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories));
                }
            }

            return result;
        });
    }

    public async Task<string> ReadFileAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task SaveFileAsync(string path, string content)
    {
        await File.WriteAllTextAsync(path, content);
    }
}