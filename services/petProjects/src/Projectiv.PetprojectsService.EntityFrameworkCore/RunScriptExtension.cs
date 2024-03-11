using Microsoft.EntityFrameworkCore.Migrations;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

namespace Projectiv.PetprojectsService.EntityFrameworkCore;

public static class RunScriptExtension
{
    private static readonly string ScriptDirectory = "Scripts";
    private static readonly string WorkbenchDelimiterKeyword = "DELIMITER";
    private static readonly string WorkbenchDelimiter = "//";

    public static void RunScript(this MigrationBuilder builder, string fileName)
    {
        var filePath = Path.Combine(ScriptDirectory, fileName);
        var existing = File.Exists(filePath);
        if (!existing)
            throw new ServerException($"Can not find file {fileName}");

        var alltext = File.ReadAllText(filePath);
        alltext = alltext.Replace(WorkbenchDelimiter, "").Replace(WorkbenchDelimiterKeyword, "");
        try
        {
            builder.Sql(alltext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Execute script failed: {filePath}{Environment.NewLine}{ex.Message}");
            throw new ServerException($"Execute script failed: {filePath}{Environment.NewLine}{ex.Message}");
        }
    }
}