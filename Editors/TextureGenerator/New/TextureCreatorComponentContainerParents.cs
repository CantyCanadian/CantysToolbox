using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextureCreatorComponentContainerTypes
{
    Input,
    Modifier,

    InputForModifier,
    ModifierNeedingInput
}

public abstract class TextureCreatorComponentContainerBase
{
    public TextureCreatorComponentContainerTypes ContainerType { get; private set; }
    public string ContainerName { get; private set; }

    public bool IsDirty { get; protected set; }

    public void Rename(string newName)
    {
        ContainerName = newName;
    }

    public abstract void OnGUI(float width);
    public abstract Texture2D Invoke(Texture2D input);

    public TextureCreatorComponentContainerBase(TextureCreatorComponentContainerTypes containerType, string containerName)
    {
        ContainerType = containerType;
        ContainerName = containerName;
    }
}

public abstract class TextureCreatorComponentContainerPairBase : TextureCreatorComponentContainerBase
{
    public TextureCreatorComponentContainerBase PairedInput;

    public TextureCreatorComponentContainerPairBase(TextureCreatorComponentContainerTypes containerType, string containerName, TextureCreatorComponentContainerBase pairInput) : base(containerType, containerName)
    {
        PairedInput = pairInput;
    }
}