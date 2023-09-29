using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	float Giro = 0;
	public int playerId;
	
	public bool Habilitado = true;
	CarController carController;
		
	//---------------------------------------------------------//
	
	// Use this for initialization
	void Start () 
	{
		carController = GetComponent<CarController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Giro = InputManager.Instance.GetAxis($"Horizontal{playerId}"); 
		carController.SetGiro(Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
	
}
