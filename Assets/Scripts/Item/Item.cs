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
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var success = other.GetComponent<ICollector>().Collect(ItemType);

        if (success) Destroy(gameObject);
    }
}
