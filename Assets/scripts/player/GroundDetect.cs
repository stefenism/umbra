using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour {
	private PlayerMovement player;
	public float groundDistance;
	public LayerMask groundLayer;

	public float width;
	public float height;

	private Vector2 ray2;
	private Vector2 ray3;

	public BoxCollider2D boxCollider;
	public CircleCollider2D circleCollider;
	public Transform opTransform;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerMovement>();

		width = boxCollider.bounds.extents.x - 0.01f;
		height = boxCollider.bounds.extents.y + 0.02f;
	}

	public void Initalize(BoxCollider2D box) {
		player = GetComponent<PlayerMovement>();

		width = boxCollider.bounds.extents.x - 0.01f;
		height = boxCollider.bounds.extents.y + 0.02f;
	}

	public void Initalize(CircleCollider2D circle) {
		width = circle.bounds.extents.x - 0.01f;

    }

	// Update is called once per frame
	void Update () {

		GroundDetection();
	}

	void GroundDetection()
	{

		ray2 = new Vector2(opTransform.position.x + width, opTransform.position.y);
		ray3 = new Vector2(opTransform.position.x - width, opTransform.position.y);


		RaycastHit2D hit = Physics2D.Raycast(opTransform.position, -Vector2.up, groundDistance, groundLayer);
		RaycastHit2D hit2 = Physics2D.Raycast(ray2, -Vector2.up, groundDistance, groundLayer);
		RaycastHit2D hit3 = Physics2D.Raycast(ray3, -Vector2.up, groundDistance, groundLayer);

		Ray2D landingRay = new Ray2D(opTransform.position, -Vector2.up);

		Debug.DrawRay(opTransform.position, -Vector2.up * groundDistance);
		Debug.DrawRay(ray2, -Vector2.up * groundDistance);
		Debug.DrawRay(ray3, -Vector2.up * groundDistance);

		if(hit.collider != null)
		{

			if(hit.collider.gameObject.tag == "Ground" && (player.rb.velocity.y > 5))
			{
				player.grounded = false;
			}

			else
			{
				player.grounded = true;
			}
		}

		else if(hit2.collider != null)
		{
			if(hit2.collider.gameObject.tag == "Ground" && (player.rb.velocity.y > 5))
			{
				player.grounded = false;
			}
			else
			{
				player.grounded = true;
			}
		}

		else if(hit3.collider != null)
		{
			if(hit3.collider.gameObject.tag == "Ground" && (player.rb.velocity.y > 5))
			{
				player.grounded = false;
			}
			else
			{
				player.grounded = true;
			}
		}

		else
		{
			player.grounded = false;
		}
	}
	}
