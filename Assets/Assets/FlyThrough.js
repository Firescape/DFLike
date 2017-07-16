var lookSpeed = 1;
var moveSpeed = 1.0;

public var rotationX = 30.45;
public var rotationY = 227.25;
public var trans:Vector3;
public var target:Transform;


var rotateCamera = true;


function Start()
{
	//target = new Transform(transform.position + Vector3(5, -5, 0));
	rotationX = 35.54995;
	rotationY = -37.50003;
	
	rotationX += Input.GetAxis("Mouse X")*lookSpeed;
	rotationY += Input.GetAxis("Mouse Y")*lookSpeed;
	rotationY = Mathf.Clamp (rotationY, -90, 90);
	
	transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
	transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
	
	transform.position = new Vector3(-3.342079, 39.08573, -9.685539);
}

function Update()
{
	trans = transform.position;
	var tAdj = 100f;
	
	transform.position += transform.forward * 100 * Input.GetAxis("Mouse ScrollWheel");
	
	
	
	if(Input.GetMouseButton(2))
	{
		//transform.RotateAround(target.transform.position, target.transform.up, 8 * Input.GetAxis("Mouse X"));
		//target.transform.RotateAround(target.transform.position, target.transform.up, 5 * -Input.GetAxis("Mouse Y"));
		//target.Rotate(target.transform.up, 5 * Input.GetAxis("Mouse X"));
		//transform.RotateAround(target.transform.position, transform.right, 4.5 * -Input.GetAxis("Mouse Y"));
		//target.Rotate(target.transform.right, 5 * -Input.GetAxis("Mouse Y"));
		//transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0, Space.Self);
		transform.RotateAround(new Vector3(0, 1, 0), 0.07 * Input.GetAxis("Mouse X"));
		//var dir2:Vector3 = new Vector3(transform.forward.x, 0, transform.forward.z);
		//dir2.Normalize();
		transform.RotateAround(transform.right, 0.07 * -Input.GetAxis("Mouse Y"));
		//transform.Rotate(
		//target.transform.RotateAround(target.transform.position, target.transform.right, 5 * -Input.GetAxis("Mouse Y"));
	}

	var dir:Vector3 = new Vector3(transform.forward.x, 0, transform.forward.z);
	dir.Normalize();
	
	transform.position += Time.deltaTime * tAdj * dir * moveSpeed * Input.GetAxis("Vertical");
	target.transform.position += Time.deltaTime * tAdj * dir * moveSpeed * Input.GetAxis("Vertical");
	
	transform.position += Time.deltaTime * tAdj * transform.right*moveSpeed*Input.GetAxis("Horizontal");
	target.transform.position += Time.deltaTime * tAdj * transform.right*moveSpeed*Input.GetAxis("Horizontal");
		
	
	if(Input.GetKeyDown("e"))
	{
		transform.parent.position += transform.parent.up * 1;
	}
	else if(Input.GetKeyDown("q"))
	{
		transform.parent.position += transform.parent.up * -1;
	}
	
	
	//transform.LookAt(target, Vector3.up);
}
