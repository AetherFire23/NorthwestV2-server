namespace AetherFire23.Commons.Composition.DummyProject;

/// <summary>
/// Doesn't know how it is being composed and/or mocked. 
/// </summary>
public class InstalledAndInitializedClass
{
    private string InitializedProperty { get; set; } = string.Empty;

    public void ImAnInitializer(string prop)
    {
        this.InitializedProperty = prop;
        
        Console.WriteLine(InitializedProperty);
    }
}