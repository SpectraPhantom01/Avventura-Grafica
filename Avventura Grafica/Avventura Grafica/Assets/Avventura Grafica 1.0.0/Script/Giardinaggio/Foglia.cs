using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foglia : MonoBehaviour
{
	public bool sullaLinea;
	public bool dentroLinea;
	public bool fuoriLinea;
	public Transform lookAtPivot;
	public Rigidbody2D rbody;
	public float speedOfSpin = 1f;
	public float powerOfHit = 1f;

	bool activated;
	int spinDirection;
	public SpriteRenderer spriteRenderer;

	Shape myShape;

    private void Start()
    {
		spriteRenderer.sortingOrder = -1; //-(int)transform.position.y;

		int rndRotation = Random.Range(0, 361);
		transform.Rotate(0, 0, rndRotation);

		transform.localScale = Vector3.one * 0.8f;
    }

    internal void RemoveMeFromList()
    {
		if(fuoriLinea)
			myShape.RemoveLeafFromList(this);
    }

    void Update()
	{
		if (activated)
			transform.Rotate(0, 0, spinDirection * speedOfSpin * Time.deltaTime);
	}

    internal bool External(Shape shape)
    {
		myShape = shape;
		return sullaLinea;
    }

    public void Hit()
	{
		if (this != null)
		{
			spinDirection = Random.Range(-1, 2);
			int rndX = Random.Range(-1, 2);
			int rndY = Random.Range(-1, 2);
			Vector2 direction = new Vector2(rndX, rndY);
			rbody.isKinematic = false;
			rbody.AddForce(direction * powerOfHit);
			activated = true;
			Destroy(gameObject, 2);
		}
	}


}
