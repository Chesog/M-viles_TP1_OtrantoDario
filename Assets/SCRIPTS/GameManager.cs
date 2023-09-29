using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public float TiempoDeJuego = 60;

    [Serializable]
    public enum EstadoJuego
    {
        Calibrando,
        Jugando,
        Menu,
        Credits,
        Finalizado
    }

    [Serializable]
    public enum DificultadJuego
    {
        FACIL,
        NORMAL,
        DIFICIL,
        NONE
    }

    [Serializable]
    public enum GameMode
    {
        SINGLEPLAYER,
        MULTIPLAYER,
        NONE
    }

    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject gamemodePanel;
    [SerializeField] private GameObject[] botonesDescarga;
    public EstadoJuego EstAct = EstadoJuego.Menu;
    public GameMode modeAct = GameMode.NONE;

    public Player Player1;
    public Player Player2;

    bool ConteoRedresivo = true;
    public Rect ConteoPosEsc;
    public float ConteoParaInicion = 3;
    public Text ConteoInicio;
    public Text TiempoDeJuegoText;

    public float TiempEspMuestraPts = 3;

    //-----------------------------------------------------------------------//
    public Camera CamCali1;
    public Camera CamCond1;
    public Camera CamDesc1;

    public Camera CamCali2;
    public GameObject CamCond2;
    public GameObject CamDesc2;

    public GameObject player2UI;
    public GameObject tutorialScenePlayer2;
    public GameObject bodyPlayer2;
    public GameObject unloadScenPlayer2;
    //-----------------------------------------------------------------------//

    //posiciones de los camiones dependientes del lado que les toco en la pantalla
    //la pos 0 es para la izquierda y la 1 para la derecha
    public Vector3[] PosCamionesCarrera = new Vector3[2];

    //posiciones de los camiones para el tutorial
    public Vector3 PosCamion1Tuto = Vector3.zero;
    public Vector3 PosCamion2Tuto = Vector3.zero;

    //listas de GO que activa y desactiva por sub-escena
    //escena de tutorial
    public GameObject[] ObjsCalibracion1;

    public GameObject[] ObjsCalibracion2;

    //la pista de carreras
    public GameObject[] ObjsCarrera;
    public GameObject[] taxisObjects;

    public GameObject[] boxObjects;

    // Escena Menu
    public GameObject MenuCambas;

    public GameObject MenuScene;

    // Escena Creditos
    public GameObject CreditsScene;
    [SerializeField] private static bool singlePlayer;

    //--------------------------------------------------------//

    void Awake()
    {
        Instancia = this;
    }

    IEnumerator Start()
    {
        yield return null;
        EmpezarMenu();
    }

    void Update()
    {
        //REINICIAR
        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //CIERRA LA APLICACION
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (EstAct)
        {
            case EstadoJuego.Calibrando:

                if (Input.GetKeyDown(KeyCode.W))
                {
                    Player1.Seleccionado = true;
                }

                if (modeAct == GameMode.MULTIPLAYER)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        Player2.Seleccionado = true;
                    }
                }

                break;


            case EstadoJuego.Jugando:

                //SKIP LA CARRERA
                if (Input.GetKey(KeyCode.Alpha9))
                {
                    TiempoDeJuego = 0;
                }

                if (TiempoDeJuego <= 0)
                {
                    FinalizarCarrera();
                }

                if (ConteoRedresivo)
                {
                    ConteoParaInicion -= T.GetDT();
                    if (ConteoParaInicion < 0)
                    {
                        EmpezarCarrera();
                        ConteoRedresivo = false;
                    }
                }
                else
                {
                    //baja el tiempo del juego
                    TiempoDeJuego -= T.GetDT();
                }

                if (ConteoRedresivo)
                {
                    if (ConteoParaInicion > 1)
                    {
                        ConteoInicio.text = ConteoParaInicion.ToString("0");
                    }
                    else
                    {
                        ConteoInicio.text = "GO";
                    }
                }

                ConteoInicio.gameObject.SetActive(ConteoRedresivo);

                TiempoDeJuegoText.text = TiempoDeJuego.ToString("00");

                break;

            case EstadoJuego.Finalizado:

                //muestra el puntaje

                TiempEspMuestraPts -= Time.deltaTime;
                if (TiempEspMuestraPts <= 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                break;
        }

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(EstAct == EstadoJuego.Jugando && !ConteoRedresivo);
        CheckPlayers();
    }


    private void CheckPlayers()
    {
        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                CamCali1.rect = new Rect(0f, 0f, 1f, 1f);
                CamCond1.rect = new Rect(0f, 0f, 1f, 1f);
                CamDesc1.rect = new Rect(0f, 0f, 1f, 1f);

                player2UI.SetActive(false);
                tutorialScenePlayer2.SetActive(false);
                CamCond2.SetActive(false);
                bodyPlayer2.SetActive(false);
                unloadScenPlayer2.SetActive(false);
                break;
            case GameMode.MULTIPLAYER:
                CamCali1.rect = new Rect(0f, 0f, 0.5f, 1f);
                CamCond1.rect = new Rect(0f, 0f, 0.5f, 1f);
                CamDesc1.rect = new Rect(0f, 0f, 0.5f, 1f);

                player2UI.SetActive(true);
                tutorialScenePlayer2.SetActive(true);
                CamCond2.SetActive(true);
                bodyPlayer2.SetActive(true);
                unloadScenPlayer2.SetActive(true);
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //----------------------------------------------------------//

    public void IniciarTutorial()
    {
        for (int i = 0; i < botonesDescarga.Length; i++)
        {
#if UNITY_EDITOR
            botonesDescarga[i].SetActive(false);
#endif
#if UNITY_ANDROID
            botonesDescarga[i].SetActive(true);
#endif
        }


        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                
                for (int i = 0; i < ObjsCalibracion1.Length; i++)
                {
                    ObjsCalibracion1[i].SetActive(true);
                    ObjsCalibracion1[i].GetComponentInChildren<PalletMover>().enabled = true;
                }

                Player1.CambiarATutorial();
                break;
            case GameMode.MULTIPLAYER:
                for (int i = 0; i < ObjsCalibracion1.Length; i++)
                {
                    ObjsCalibracion1[i].SetActive(true);
                    ObjsCalibracion1[i].GetComponentInChildren<PalletMover>().enabled = true;
                    ObjsCalibracion2[i].SetActive(true);
                    ObjsCalibracion2[i].GetComponentInChildren<PalletMover>().enabled = true;
                }

                Player1.CambiarATutorial();
                Player2.CambiarATutorial();
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActive(false);
        }

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
        MenuCambas.SetActive(false);
        MenuScene.SetActive(false);
        CreditsScene.SetActive(false);
    }

    public void EmpezarMenu()
    {
        Player1.CambiarAMenu();
        Player2.CambiarAMenu();

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
        CreditsScene.SetActive(false);
        MenuCambas.SetActive(true);
        MenuScene.SetActive(true);
    }

    public void SeleccionarGameMode()
    {
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
        CreditsScene.SetActive(false);

        difficultyPanel.SetActive(false);
        gamemodePanel.SetActive(true);
    }

    public void SeleccionarDifficultad()
    {
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
        CreditsScene.SetActive(false);

        gamemodePanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void EmpezarCreditos()
    {
        EstAct = EstadoJuego.Credits;
        MenuCambas.SetActive(false);
        CreditsScene.SetActive(true);
    }

    public void OpenItchio()
    {
        System.Diagnostics.Process.Start("https://chesog.itch.io");
    }

    private void EmpezarCarrera()
    {
        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                Player1.GetComponent<Frenado>().RestaurarVel();
                Player1.GetComponent<ControlDireccion>().Habilitado = true;
                break;
            case GameMode.MULTIPLAYER:
                Player1.GetComponent<Frenado>().RestaurarVel();
                Player1.GetComponent<ControlDireccion>().Habilitado = true;
                Player2.GetComponent<Frenado>().RestaurarVel();
                Player2.GetComponent<ControlDireccion>().Habilitado = true;
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        MenuScene.SetActive(false);
    }

    void FinalizarCarrera()
    {
        EstAct = GameManager.EstadoJuego.Finalizado;

        TiempoDeJuego = 0;

        if (Player1.Dinero > Player2.Dinero)
        {
            //lado que gano
            if (Player1.LadoActual == Visualizacion.Lado.Der)
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
            else
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;
            //puntajes
            DatosPartida.PtsGanador = Player1.Dinero;
            DatosPartida.PtsPerdedor = Player2.Dinero;
        }
        else
        {
            //lado que gano
            if (Player2.LadoActual == Visualizacion.Lado.Der)
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
            else
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

            //puntajes
            DatosPartida.PtsGanador = Player2.Dinero;
            DatosPartida.PtsPerdedor = Player1.Dinero;
        }

        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                Player1.GetComponent<Frenado>().Frenar();
                Player1.ContrDesc.FinDelJuego();
                break;
            case GameMode.MULTIPLAYER:
                Player1.GetComponent<Frenado>().Frenar();
                Player1.ContrDesc.FinDelJuego();
                Player2.GetComponent<Frenado>().Frenar();
                Player2.ContrDesc.FinDelJuego();
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetGamemode(int IsSinglePlayer)
    {
        modeAct = (GameMode)IsSinglePlayer;
        SeleccionarDifficultad();
    }

    public void SetDifficulty(int difficulty)
    {
        DificultadJuego diff = (DificultadJuego)difficulty;
        switch (diff)
        {
            case DificultadJuego.FACIL:
                foreach (GameObject caja in boxObjects)
                    caja.SetActive(false);
                foreach (GameObject taxi in taxisObjects)
                    taxi.SetActive(false);
                break;
            case DificultadJuego.NORMAL:
                foreach (GameObject caja in boxObjects)
                    caja.SetActive(true);
                foreach (GameObject taxi in taxisObjects)
                    taxi.SetActive(false);
                break;
            case DificultadJuego.DIFICIL:
                foreach (GameObject caja in boxObjects)
                    caja.SetActive(true);
                foreach (GameObject taxi in taxisObjects)
                    taxi.SetActive(true);
                break;
            case DificultadJuego.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(diff), diff, null);
        }

        IniciarTutorial();
    }

    //cambia a modo de carrera
    public void CambiarACarrera()
    {
        EstAct = GameManager.EstadoJuego.Jugando;

        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActive(true);
        }

        //desactivacion de la calibracion
        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                Player1.FinCalibrado = true;
                for (int i = 0; i < ObjsCalibracion1.Length; i++)
                {
                    ObjsCalibracion1[i].SetActive(false);
                }

                //posiciona los camiones dependiendo de que lado de la pantalla esten
                if (Player1.LadoActual == Visualizacion.Lado.Izq)
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[0];
                }
                else
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[1];
                }
                
                Player1.transform.forward = Vector3.forward;
                Player1.GetComponent<Frenado>().Frenar();
                Player1.CambiarAConduccion();
                
                //los deja andando
                Player1.GetComponent<Frenado>().RestaurarVel();
                //cancela la direccion
                Player1.GetComponent<ControlDireccion>().Habilitado = false;
                //les de direccion
                Player1.transform.forward = Vector3.forward;
                break;
            case GameMode.MULTIPLAYER:
                Player2.FinCalibrado = true;
                Player1.FinCalibrado = true;
                for (int i = 0; i < ObjsCalibracion1.Length; i++)
                {
                    ObjsCalibracion1[i].SetActive(false);
                }
                for (int i = 0; i < ObjsCalibracion2.Length; i++)
                {
                    ObjsCalibracion2[i].SetActive(false);
                }

                //posiciona los camiones dependiendo de que lado de la pantalla esten
                if (Player1.LadoActual == Visualizacion.Lado.Izq)
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[0];
                    Player2.gameObject.transform.position = PosCamionesCarrera[1];
                }
                else
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[1];
                    Player2.gameObject.transform.position = PosCamionesCarrera[0];
                }
                
                Player1.transform.forward = Vector3.forward;
                Player1.GetComponent<Frenado>().Frenar();
                Player1.CambiarAConduccion();
                
                Player2.transform.forward = Vector3.forward;
                Player2.GetComponent<Frenado>().Frenar();
                Player2.CambiarAConduccion();
                //los deja andando
                Player2.GetComponent<Frenado>().RestaurarVel();
                //cancela la direccion
                Player2.GetComponent<ControlDireccion>().Habilitado = false;
                //les de direccion
                Player2.transform.forward = Vector3.forward;
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        MenuCambas.SetActive(false);
        MenuScene.SetActive(false);
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
    }

    public void CerrarElJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void FinCalibracion(int playerID)
    {
        switch (modeAct)
        {
            case GameMode.SINGLEPLAYER:
                if (playerID == 0)
                {
                    Player1.FinTuto = true;
                }
                if (Player1.FinTuto)
                    CambiarACarrera();
                break;
            case GameMode.MULTIPLAYER:
                if (playerID == 0)
                {
                    Player1.FinTuto = true;
                }
                if (playerID == 1)
                {
                    Player2.FinTuto = true;
                }
                if (Player1.FinTuto && Player2.FinTuto)
                    CambiarACarrera();
                break;
            case GameMode.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}