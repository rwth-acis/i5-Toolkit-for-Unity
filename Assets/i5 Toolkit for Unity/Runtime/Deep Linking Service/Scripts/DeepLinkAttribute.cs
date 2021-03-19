using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class DeepLinkAttribute : Attribute
{
    public string Path { get; set; }

    public DeepLinkAttribute(string path)
    {
        Path = path;
    }
}

