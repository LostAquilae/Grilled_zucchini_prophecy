using UnityEngine;

public class IsometricObject : MonoBehaviour
{
	public float floorHeight;
	public bool  isStatic;

	[HideInInspector]
	public float spriteLowerBound;
	[HideInInspector]
	public float spriteHalfWidth;

	void Start()
	{
        RefreshSprite();
	}

	void LateUpdate()
	{
		if(!isStatic)
		{
			PlaceInZ();
		}
	}

	public void PlaceInZ()
	{
        transform.position = new Vector3
			(
				transform.position.x,
				transform.position.y,
				(transform.position.y - spriteLowerBound + floorHeight) * Mathf.Tan(Mathf.PI / 6)
			);
	}

    public void RefreshSprite()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteLowerBound = spriteRenderer.bounds.size.y * 0.5f;
        spriteHalfWidth = spriteRenderer.bounds.size.x * 0.5f;
    }

	void OnDrawGizmos()
	{
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		spriteLowerBound = spriteRenderer.bounds.size.y * 0.5f;
		spriteHalfWidth = spriteRenderer.bounds.size.x * 0.5f;

  		Vector3 floorHeightPos = new Vector3
           	(
                transform.position.x,
                transform.position.y - spriteLowerBound + floorHeight,
                transform.position.z
            );
 
		Gizmos.color = new Color(1f, 0.6f, 0f);
	    Gizmos.DrawLine(floorHeightPos + Vector3.left * spriteHalfWidth, floorHeightPos + Vector3.right * spriteHalfWidth);
	}
}
