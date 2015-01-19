using UnityEngine;
using System.Collections;

namespace goshdarngames.Character
{
	public enum JumpState
	{
		grounded,
		jumping,
		doublejump
	}

	[RequireComponent (typeof(Rigidbody))]
	public class PlatformMovement : MonoBehaviour, ICharacter
	{
		public bool doubleJump = false;
		public bool wallJump = false;
				
		[Range (0,20)]
		public float speed;
		
		[Range (0, 2000)]
		public float jumpForce;
		
		[Range (0, 1)]
		public float airMove;
		
		public Transform floor;
		public JumpState jumpState;
	
		void Start ()
		{
			
		}
		
		void Update ()
		{
			bool onGround = Physics.Raycast(floor.position, Vector3.down, 0.3f);
			if(onGround && (jumpState == JumpState.jumping || jumpState == JumpState.doublejump))
			{
				jumpState = JumpState.grounded;
			}
			
			if(!onGround && jumpState == JumpState.grounded)
			{
				jumpState = JumpState.jumping;
			}
		}
		
		private void Jump()
		{
			if(jumpState == JumpState.grounded || jumpState == JumpState.jumping && doubleJump)
			{
				if(jumpState == JumpState.jumping)
				{
					jumpState = JumpState.doublejump;
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
				}
				rigidbody.AddForce(Vector3.up * jumpForce);
			}
		}
		
		#region ICharacter implementation
			
		public void Move (Vector2 value)
		{
			Vector3 dir = new Vector3(value.x, 0, 0);
			dir *= speed * Time.deltaTime;
			if(jumpState != JumpState.grounded)
			{
				dir *= airMove;
			}

			rigidbody.MovePosition(rigidbody.position + dir);
			//rigidbody.AddForce(rigidbody.position + dir * 500);
		}
	
		public void Action1 ()
		{
			Jump();
		}
	
		public void Action2 ()
		{
		}
	
		public void Action3 ()
		{
		}
	
		public void Action4 ()
		{
		}
	
		#endregion
	}
}