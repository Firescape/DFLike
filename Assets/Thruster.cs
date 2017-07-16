using UnityEngine;
using System.Collections;


public class Thruster : MonoBehaviour {
   // declare the variables
   public float Speed = 9;
   public float Drag = 20;
   public float DragNoMovement = 50;
   const float  airDrag = 0F;

   void FixedUpdate () {
      // get the inputs
      float horizontal = Input.GetAxis ("Horizontal");
      float vertical = Input.GetAxis ("Vertical");
      float altitude = Input.GetAxis ("UpDown");

      // check to see if the user is moving
      bool userMoved = Mathf.Abs (horizontal) > 0.1F || Mathf.Abs (vertical) > 0.1F || Mathf.Abs (altitude) > 0.1F;

      // determine the force vector
              float x = horizontal * Speed;         
      float z = vertical * Speed;
      float y = altitude * Speed;
      GetComponent<Rigidbody>().AddRelativeForce (new Vector3 (x, y, z), ForceMode.VelocityChange);
      
      // apply the appropriate drag when moving
      if (userMoved)
         GetComponent<Rigidbody>().drag = Drag;
      else
         GetComponent<Rigidbody>().drag = DragNoMovement;
   }
   
   
   void Start () {
      if (GetComponent<Rigidbody>()==null)
         gameObject.AddComponent <Rigidbody>();

      // don't let the physics engine rotate the character
      GetComponent<Rigidbody>().freezeRotation = true;
   }
}
