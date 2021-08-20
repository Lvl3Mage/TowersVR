using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPickedUpObject
{
	public Transform Obj, Offset;
	public Transform Parent;
	public float Mass, Drag, AngDrag;
	public PCPickedUpObject(Transform _PickedUpObj, Transform _PickedUpObjOffset, Transform _PickedUpObjParent, float _Mass, float _Drag, float _AngDrag){
		Obj = _PickedUpObj;
		Offset = _PickedUpObjOffset;
		Parent = _PickedUpObjParent;
		Mass = _Mass;
		Drag = _Drag;
		AngDrag = _AngDrag;

	}
}
