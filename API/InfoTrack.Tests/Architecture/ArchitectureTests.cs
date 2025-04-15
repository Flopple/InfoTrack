using InfoTrack.Web.API;
using NetArchTest.Rules;

namespace InfoTrack.Tests.Architecture;

public class ArchitectureTests
{
    private const string CommonNamespace = "InfoTrack.Common";
    private const string DomainNamespace = "InfoTrack.Domain";
    private const string PersistenceNamespace = "InfoTrack.Persistence";
    private const string ServicesNamespace = "InfoTrack.Services";
    private const string WebApiNamespace = "InfoTrack.Web.API";
    private const string TestsNamespace = "InfoTrack.Tests";

    [Fact]
    public void Domain_ShouldNotReferenceOtherProjects()
    {
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var forbiddenProjects = new string[]
        {
            ServicesNamespace,
            PersistenceNamespace,
            WebApiNamespace,
            CommonNamespace,
            TestsNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Common_ShouldNotReferenceOtherProjects()
    {
        var assembly = typeof(Common.AssemblyReference).Assembly;

        var forbiddenProjects = new string[]
        {
            DomainNamespace,
            ServicesNamespace,
            PersistenceNamespace,
            WebApiNamespace,
            TestsNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Services_ShouldNotReferenceOtherProjects()
    {
        var assembly = typeof(Services.AssemblyReference).Assembly;

        var forbiddenProjects = new string[]
        {
            WebApiNamespace,
            TestsNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Persistence_ShouldNotReferenceOtherProjects()
    {
        var assembly = typeof(Persistence.AssemblyReference).Assembly;

        var forbiddenProjects = new string[]
        {
            ServicesNamespace,
            WebApiNamespace,
            TestsNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void WebApi_ShouldNotReferenceOtherProjects()
    {
        var assembly = typeof(Program).Assembly;

        var forbiddenProjects = new string[]
        {
            TestsNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}