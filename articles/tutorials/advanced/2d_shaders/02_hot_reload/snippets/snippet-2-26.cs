private static bool IsFileLocked(string path)
{
    try
    {
        using FileStream _ = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        // File is not locked
        return false;
    }
    catch (IOException)
    {
        // File is locked or inaccessible
        return true;
    }
}
