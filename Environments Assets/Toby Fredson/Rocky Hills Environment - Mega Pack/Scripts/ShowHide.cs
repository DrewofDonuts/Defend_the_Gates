using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TobyFredson
{
public class ShowHide : MonoBehaviour{

	public void showIt(GameObject obj){
		obj.SetActive (true);
	}

	public void hideIt(GameObject obj){
		obj.SetActive (false);
    }
}
}