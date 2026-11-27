// Utility class to make a Union of DesktopGL/Native API reference entries across platforms.  Highlights codepaths unique between Native and the DesktopGL baseline.

using System.Text.Json;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: dotnet run tools/ApiPlatformMarker.cs -- <published-api-dir> <baseline-api-dir> <output-file>");
    return 1;
}

string outputFile = args[2];
DocFXApiMetaDataCatalog? published = DocFXApiMetaDataCatalog.Load(args[0]);
DocFXApiMetaDataCatalog? baseline = DocFXApiMetaDataCatalog.Load(args[1]);

if (published == null || baseline == null)
{
    return 1;
}

List<string> nativeOnly = published.MissingFrom(baseline);
List<string> desktopGLOnly = baseline.MissingFrom(published);

// Overwrite native pages with the DesktopGL versions, anything left is Native only.
HashSet<string> pagesWithNativeOnlyMembers = nativeOnly.Select(published.MetaDataPageMap).ToHashSet();
HashSet<string> restored = desktopGLOnly
    .Select(baseline.MetaDataPageMap)
    .Where(page => published.Contains(page) && !pagesWithNativeOnlyMembers.Contains(page))
    .ToHashSet();

try
{
    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputFile))!);
    File.WriteAllText(outputFile, string.Concat(nativeOnly.Select(uid => $"---\nuid: '{uid.Replace("'", "''")}'\nnativeOnly: true\n---\n\n")));

    foreach (string page in restored)
    {
        File.Copy(baseline.MetaDataFileMap(page), published.MetaDataFileMap(page), overwrite: true);
    }
}
catch (Exception e)
{
    Console.Error.WriteLine($"Could not update the API files: {e.Message}");
    return 1;
}

Console.WriteLine($"Native only: {nativeOnly.Count} API entries marked.");
foreach (string uid in nativeOnly)
{
    Console.WriteLine($"  {uid}");
}

if (desktopGLOnly.Count > 0)
{
    bool onGitHub = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
    Console.WriteLine($"DesktopGL only: {desktopGLOnly.Count} API entries. Native Gap.");

    foreach (string uid in desktopGLOnly)
    {
        string entry = restored.Contains(baseline.MetaDataPageMap(uid)) ? uid : $"{uid} (not in the published docs)";
        Console.WriteLine(onGitHub ? $"::notice title=DesktopGL only API::{entry}" : $"  {entry}");
    }
}

return 0;

sealed class DocFXApiMetaDataCatalog(string dir, Dictionary<string, string> fileMapUid)
{
    private readonly Dictionary<string, string> pageMapFile = fileMapUid
        .GroupBy(entry => entry.Value)
        .ToDictionary(group => group.Key, group => group.MinBy(entry => entry.Key.Length).Key);

    public bool Contains(string uid) => fileMapUid.ContainsKey(uid);

    public string MetaDataPageMap(string uid) => pageMapFile[fileMapUid[uid]];

    public string MetaDataFileMap(string uid) => Path.Combine(dir, fileMapUid[uid]);

    public List<string> MissingFrom(DocFXApiMetaDataCatalog other) => fileMapUid.Keys
        .Where(uid => !other.Contains(uid) && (MetaDataPageMap(uid) == uid || other.Contains(MetaDataPageMap(uid))))
        .Order(StringComparer.Ordinal)
        .ToList();

    public static DocFXApiMetaDataCatalog? Load(string dir)
    {
        string manifest = Path.Combine(dir, ".manifest");
        if (!File.Exists(manifest))
        {
            Console.Error.WriteLine($"API metadata not found: {manifest}. Run 'dotnet docfx metadata' first.");
            return null;
        }

        var metaDataFileMapUid = new Dictionary<string, string>();
        try
        {
            using JsonDocument json = JsonDocument.Parse(File.ReadAllText(manifest));
            foreach (JsonProperty entry in json.RootElement.EnumerateObject())
            {
                if (entry.Value.ValueKind == JsonValueKind.String && entry.Value.GetString() is { Length: > 0 } file)
                {
                    metaDataFileMapUid[entry.Name] = file;
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Could not read {manifest}: {e.Message}");
            return null;
        }

        if (metaDataFileMapUid.Count == 0)
        {
            Console.Error.WriteLine($"No API items found in {manifest}.");
            return null;
        }

        return new DocFXApiMetaDataCatalog(dir, metaDataFileMapUid);
    }
}
