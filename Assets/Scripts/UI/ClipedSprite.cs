using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipedSprite : MonoBehaviour {

    public SpriteRenderer Sprite;
    public float MaxSize;
    public float MinSize;
    public Direction Direction;
    public ClipedSprite ChildClipedSprite;

    public void Clip(float value)
    {
        float newSize = Mathf.Lerp(MinSize, MaxSize, value);

        if (Direction == Direction.Horizontal)
            Sprite.size = new Vector2(newSize, Sprite.size.y);
        else
            Sprite.size = new Vector2(Sprite.size.x, newSize);

        if (ChildClipedSprite != null)
            ChildClipedSprite.Clip(value);
    }
}

public enum Direction
{
    Horizontal,
    Vertical
}
