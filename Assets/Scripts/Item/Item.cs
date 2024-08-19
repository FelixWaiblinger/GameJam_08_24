using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType ItemType;
    SpriteRenderer _sprite;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = ItemType.Color;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        var success = other.gameObject.GetComponent<ICollector>().Collect(ItemType);

        if (success) Destroy(gameObject);
    }
}
