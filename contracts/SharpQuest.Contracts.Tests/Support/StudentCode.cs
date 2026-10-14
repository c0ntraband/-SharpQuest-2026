using System.Reflection;
using System.Runtime.CompilerServices;

namespace SharpQuest.Contracts.Tests.Support;

public static class StudentCode
{
    public const string AssemblyName = "Capstone.Core";

    public const string NotStartedPrefix = "Not started:";

    public const string CatchUpNamespace = "Capstone.Core.CatchUp";

    public static Assembly Assembly { get; } = Assembly.Load(new AssemblyName(AssemblyName));

    public static bool IsCatchUp(Type? type) =>
        type?.Namespace is { } ns && (ns == CatchUpNamespace || ns.StartsWith(CatchUpNamespace + ".", StringComparison.Ordinal));

    public static TWiring Wiring<TWiring>() where TWiring : class =>
        WiringIn<TWiring>(Assembly, $"src/{AssemblyName}/Wiring/");

    public const string ApiAssemblyName = "Capstone.Api";

    private static readonly Lazy<Assembly?> ApiAssemblyLazy = new(() =>
    {
        try { return Assembly.Load(new AssemblyName(ApiAssemblyName)); }
        catch (FileNotFoundException) { return null; }
    });

    public static Assembly? ApiAssembly => ApiAssemblyLazy.Value;

    public static TWiring ApiWiring<TWiring>() where TWiring : class
    {
        var inCore = Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(TWiring).IsAssignableFrom(t) && !IsCatchUp(t))
            .ToList();
        if (inCore.Count > 0)
            Assert.Fail($"{inCore[0].Name} implements {typeof(TWiring).Name}, but it is in {AssemblyName}. " +
                        $"This lab's wiring belongs in src/{ApiAssemblyName}/ — an HTTP endpoint is a front end, like " +
                        $"the console app and the window, and your domain library shouldn't know HTTP exists.");

        if (ApiAssembly is null)
            Assert.Skip($"{NotStartedPrefix} there is no src/{ApiAssemblyName} project yet. Create it with " +
                        $"`dotnet new web -o src/{ApiAssemblyName}` to start this lab.");

        return WiringIn<TWiring>(ApiAssembly!, $"src/{ApiAssemblyName}/");
    }

    private static TWiring WiringIn<TWiring>(Assembly assembly, string where) where TWiring : class
    {
        var all = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(TWiring).IsAssignableFrom(t))
            .ToList();

        var candidates = all.Where(t => !IsCatchUp(t)).ToList();

        if (candidates.Count == 0)
            Assert.Skip(all.Count > 0
                ? $"{NotStartedPrefix} the only class implementing {typeof(TWiring).Name} is the catch-up kit's " +
                  $"({all[0].FullName}). The kit unblocks later labs; it doesn't do this one. " +
                  $"Write your own in {where}."
                : $"{NotStartedPrefix} no class in {assembly.GetName().Name} implements {typeof(TWiring).Name}. " +
                  $"Add one to {where} to start this lab.");
        if (candidates.Count > 1)
            Assert.Fail($"{candidates.Count} classes implement {typeof(TWiring).Name} " +
                        $"({string.Join(", ", candidates.Select(c => c.Name))}). Keep exactly one.");

        var type = candidates[0];
        if (type.GetConstructor(Type.EmptyTypes) is null)
            Assert.Fail($"{type.Name} needs a public constructor with no parameters.");

        var wiring = (TWiring)Activator.CreateInstance(type)!;
        RejectKitObjects(wiring);
        return wiring;
    }

    private static void RejectKitObjects(object wiring)
    {
        foreach (var method in wiring.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!method.Name.StartsWith("Create", StringComparison.Ordinal)) continue;
            if (method.ReturnType == typeof(void) || method.ReturnType.IsValueType) continue;

            var parameters = method.GetParameters();
            object?[] arguments;
            if (parameters.Length == 0) arguments = [];
            else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int)) arguments = [1];
            else continue;

            object? made;
            try { made = method.Invoke(wiring, arguments); }
            catch { continue; }

            if (made is not null && IsCatchUp(made.GetType()))
                Assert.Fail($"{wiring.GetType().Name}.{method.Name}() returns {made.GetType().FullName}, " +
                            "which comes from the catch-up kit. The kit is there to unblock later labs — " +
                            "pointing your wiring at it doesn't earn this one. Write your own and return that.");
        }
    }

    public static IReadOnlyList<Type> ModelTypes()
    {
        return Assembly.GetTypes()
            .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
            .Where(t => !t.Name.StartsWith('<'))
            .Where(t => t.Name != "Program")
            .Where(t => !IsWiring(t))
            .Where(t => !IsCatchUp(t))
            .ToList();
    }

    public static bool IsWiring(Type t) =>
        t.GetInterfaces().Any(i => i.Namespace == "SharpQuest.Contracts" && i.Name.EndsWith("Wiring"));

    public static bool IsRecord(Type t) =>
        t.GetMethod("<Clone>$") is not null
        || (t.IsValueType && t.GetMethod("PrintMembers",
               BindingFlags.Instance | BindingFlags.NonPublic) is not null
            && t.GetMethods().Any(m => m.Name == "op_Equality"));
}
