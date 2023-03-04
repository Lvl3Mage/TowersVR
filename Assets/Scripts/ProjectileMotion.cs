using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ProjectileMotion
{
	static readonly float g = 9.81f;
	public static float CalculateLaunchAngle(Vector2 relTarget, float v){ // link to original post: https://gamedev.stackexchange.com/questions/53552/how-can-i-find-a-projectiles-launch-angle
		float vSq = Mathf.Pow(v,2);
		float Root = Mathf.Pow(v,4) - g*(g*Mathf.Pow(relTarget.x,2) + 2*relTarget.y*vSq);
		Root = Mathf.Sqrt(Root);
		if(float.IsNaN(Root)){
			return Root; // if root is nan no thurther calculation are needed
		}
		else{
			float preAngle = (vSq - Root)/(g*relTarget.x);
			float angle = Mathf.Atan(preAngle);
			return angle * Mathf.Rad2Deg;
		}
	}
	public static float ResolveParabolaForY(float alpha, float TotalVel, float x){// a solution to the kinematic equations where y = horizV*(x/vertV) - (g)(x^2)/(2horizV^2)
		Vector2 v = new Vector2(Mathf.Cos(alpha*Mathf.Deg2Rad)*TotalVel,Mathf.Sin(alpha*Mathf.Deg2Rad)*TotalVel);
		float t = x/v.x;
		float height = v.y*t - g*Mathf.Pow(t,2)/2;
		return height;
	}
	public static Vector2 ResolveParabolaForX(float alpha, float TotalVel, float y){
		Vector2 v = new Vector2(Mathf.Cos(alpha*Mathf.Deg2Rad)*TotalVel,Mathf.Sin(alpha*Mathf.Deg2Rad)*TotalVel);
		float a = -g/(2 * Mathf.Pow(v.x,2));
		float b = v.y/v.x;
		float c = -y;
		float Root = Mathf.Sqrt(Mathf.Pow(b,2)-4*a*c);
		if(float.IsNaN(Root)){
			return new Vector2(Root,Root);
		}
		else{
			Vector2 Dist;
			Dist.x = (-b-Root)/(2*a);
			Dist.y = (-b+Root)/(2*a);
			return Dist;
		}
		/*float Root = Mathf.Pow(v.y,2)-y*g;
		Root = Mathf.Sqrt(Root);
		if(float.IsNaN(Root)){
			return Root; // if root is nan no thurther calculation are needed
		}
		else{
			float x = 2*v.x*(v.y+Root);
			x /= g;
			return x;
		}*/
		
	}
}
