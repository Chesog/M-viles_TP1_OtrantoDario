using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class MngPts : MonoBehaviour 
{
	Rect R = new Rect();

	[SerializeField] private TextMeshProUGUI textP1;
	[SerializeField] private TextMeshProUGUI textP2;
	[SerializeField] private RectTransform rectP1;
	[SerializeField] private RectTransform rectP2;
	[SerializeField] private GameObject player1Wins;
	[SerializeField] private GameObject player2Wins;
	public float TiempEmpAnims = 2.5f;
	float Tempo = 0;
	
	public Vector2[] DineroPos;
	public Vector2 DineroEsc;
	public GUISkin GS_Dinero;
	
	public Vector2 GanadorPos;
	public Vector2 GanadorEsc;
	public Texture2D[] Ganadores;
	public GUISkin GS_Ganador;
	
	public GameObject Fondo;
	public GameObject Creditos;
	
	public float TiempEspReiniciar = 10;
	public float TiempEspReiniciarCreditos = 10;
	
	
	public float TiempParpadeo = 0.7f;
	float TempoParpadeo = 0;
	bool PrimerImaParp = true;
	
	public bool ActivadoAnims = false;
	
	Visualizacion Viz = new Visualizacion();
	
	//---------------------------------//
	
	// Use this for initialization
	void Start () 
	{		
		SetGanador();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//PARA JUGAR
		if(Input.GetKeyDown(KeyCode.Space) || 
		   Input.GetKeyDown(KeyCode.Return) ||
		   Input.GetKeyDown(KeyCode.Alpha0))
		{
			SceneManager.LoadScene(0);
		}
		
		//CIERRA LA APLICACION
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}		
		
		
		TiempEspReiniciar -= Time.deltaTime;
		if(TiempEspReiniciar <= 0 )
		{
			TiempEspReiniciarCreditos -= Time.deltaTime;
			if (TiempEspReiniciarCreditos <= 0) { SceneManager.LoadScene(0); }
			Creditos.SetActive(true);
		}
		
		
		
		
		if(ActivadoAnims)
		{
			TempoParpadeo += Time.deltaTime;
			
			if(TempoParpadeo >= TiempParpadeo)
			{
				TempoParpadeo = 0;
				
				if(PrimerImaParp)
					PrimerImaParp = false;
				else
				{
					TempoParpadeo += 0.1f;
					PrimerImaParp = true;
				}
			}
		}
		
		
		
		if(!ActivadoAnims)
		{
			Tempo += Time.deltaTime;
			if(Tempo >= TiempEmpAnims)
			{
				Tempo = 0;
				ActivadoAnims = true;
			}
		}
		if(ActivadoAnims)
		{
			SetDineroText();
			//SetDinero();
			SetCartelGanador();
		}
	}
	
	void OnGUI()
	{
		//if(ActivadoAnims)
		//{
		//	SetDineroText();
		//	//SetDinero();
		//	SetCartelGanador();
		//}
		
		GUI.skin = null;
	}
	
	//---------------------------------//
	
	
	void SetGanador()
	{
		switch(DatosPartida.LadoGanadaor)
		{
		case DatosPartida.Lados.Der:
			
			GS_Ganador.box.normal.background = Ganadores[1];
			
			break;
			
		case DatosPartida.Lados.Izq:
			
			GS_Ganador.box.normal.background = Ganadores[0];
			
			break;
		}
	}

	private void SetDineroText()
	{
		if (DatosPartida.LadoGanadaor == DatosPartida.Lados.Izq)
		{
			string p1Text = "$ " + DatosPartida.PtsGanador;
			string p2Text = "$ " + DatosPartida.PtsPerdedor;
			textP1.SetText(p1Text);
			textP2.SetText(p2Text);
			if (textP1.isActiveAndEnabled)
			{
				TempoParpadeo += Time.deltaTime;
				if (TempoParpadeo >= TiempParpadeo)
				{
					TempoParpadeo = 0.0f;
					textP1.enabled = false;		
				}
			}
			else
			{
				TempoParpadeo += Time.deltaTime;
				if (TempoParpadeo >= TiempParpadeo)
				{
					TempoParpadeo = 0.0f;
					textP1.enabled = true;		
				}
			}
		}
		else
		{
			string p1Text = "$ " + DatosPartida.PtsPerdedor;
			string p2Text = "$ " + DatosPartida.PtsGanador;
			textP1.SetText(p1Text);
			textP2.SetText(p2Text);
			if (textP2.isActiveAndEnabled)
			{
				TempoParpadeo += Time.deltaTime;
				if (TempoParpadeo >= TiempParpadeo)
				{
					TempoParpadeo = 0.0f;
					textP2.enabled = false;		
				}
			}
			else
			{
				TempoParpadeo += Time.deltaTime;
				if (TempoParpadeo >= TiempParpadeo)
				{
					TempoParpadeo = 0.0f;
					textP2.enabled = true;		
				}
			}
		}
	}

	void SetDinero()
	{
		GUI.skin = GS_Dinero;
		
		R.width = DineroEsc.x * Screen.width/100;
		R.height = DineroEsc.y * Screen.height/100;
		
		
		
		//IZQUIERDA
		R.x = DineroPos[0].x * Screen.width/100;
		R.y = DineroPos[0].y * Screen.height/100;
		
		if(DatosPartida.LadoGanadaor == DatosPartida.Lados.Izq)//izquierda
		{
			if(!PrimerImaParp)//para que parpadee
				GUI.Box(R, "$" + Viz.PrepararNumeros(DatosPartida.PtsGanador));
		}
		else
		{
			GUI.Box(R, "$" + Viz.PrepararNumeros(DatosPartida.PtsPerdedor));
		}
		
		
		
		//DERECHA
		R.x = DineroPos[1].x * Screen.width/100;
		R.y = DineroPos[1].y * Screen.height/100;
		
		if(DatosPartida.LadoGanadaor == DatosPartida.Lados.Der)//derecha
		{
			if(!PrimerImaParp)//para que parpadee
				GUI.Box(R, "$" + Viz.PrepararNumeros(DatosPartida.PtsGanador));
		}
		else
		{
			GUI.Box(R, "$" + Viz.PrepararNumeros(DatosPartida.PtsPerdedor));
		}
		
	}
	public void OpenItchio() { System.Diagnostics.Process.Start("https://chesog.itch.io"); }
	void SetCartelGanador()
	{
		if (DatosPartida.LadoGanadaor == DatosPartida.Lados.Izq)
		{
			player2Wins.SetActive(false);
			player1Wins.SetActive(true);
		}
		else
		{
			player1Wins.SetActive(false);
			player2Wins.SetActive(true);
		}
	}
	
	public void DesaparecerGUI()
	{
		ActivadoAnims = false;
		Tempo = -100;
	}
}
