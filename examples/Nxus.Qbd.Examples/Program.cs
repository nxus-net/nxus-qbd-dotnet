using DotNetEnv;
using Nxus.Qbd.Examples;

// Load .env file from solution root if present
var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".env");
if (File.Exists(envPath))
    Env.Load(envPath);

var example = args.Length > 0 ? args[0].ToLower() : "crud";

switch (example) {
    case "crud":
        await BasicCrud.RunAsync();
        break;
    case "pagination":
        await AutoPagination.RunAsync();
        break;
    default:
        Console.WriteLine("Usage: dotnet run -- <example>");
        Console.WriteLine();
        Console.WriteLine("Available examples:");
        Console.WriteLine("  crud         Full CRUD lifecycle on Vendors (async)");
        Console.WriteLine("  pagination   Auto-pagination and manual page navigation (async)");
        break;
}
