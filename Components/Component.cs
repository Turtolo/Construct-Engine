using System;

public class Component
{
    public Type RootType { get; set; }
    public Object Root { get; set; }

    public Component(object root)
    {
        Root = root;
        RootType = root.GetType();
    }
}
