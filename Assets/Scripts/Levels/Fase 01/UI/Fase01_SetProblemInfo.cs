using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fase01_SetProblemInfo : MonoBehaviour
{
    /*
    O objeto deve chegar na plataforma final passando pela plataforma do meio.
    A plataforma do meio está a X metros de distância da plataforma inicial e possui uma área de Y por Z metros.
    A plataforma final está a W metros da plataforma do meio e possui uma área de K por L metros.
    Você pode decidir a aceleração inicial do objeto e o ângulo do pulo. 
    */

    public Fase01_References references;
    private Fase01_GameState gms;
    private Fase01_PlayerController player;
    public TMPro.TMP_Text textUI;

    [SerializeField]
    private string format = "#.##";

    [SerializeField]
    private Vector3 DimensaoPlataformaInicial = Vector3.zero;
    [SerializeField]
    private Vector3 DimensaoPlataformaDoMeio = Vector3.zero;
    [SerializeField]
    private Vector3 DimensaoPlataformaFinal = Vector3.zero;
    [SerializeField]
    private Vector3 DistanciaEntreInicialEPulo = Vector3.zero;
    [SerializeField]
    private Vector3 DistanciaEntrePuloEMeio = Vector3.zero;
    [SerializeField]
    private Vector3 DistanciaEntreMeioEFinal = Vector3.zero;
    [SerializeField]
    private string info;
    private Dictionary<string, string> Imagens = new Dictionary<string, string>();

    private float UnitScale = 0.0f;

    private void Awake() {
        InitializeImagens();
    }

    private void Start() {
        player = references.Player.GetComponent<Fase01_PlayerController>();
        gms = references.GameState.GetComponent<Fase01_GameState>();
        UnitScale = gms.UnitScale;
    }

    public void OnInfoChanged(Fase01_GetProblemInfo gpi){
        SetValues(gpi);
        SetText();
    }

    public void OnInfoChanged(Fase01_GetProblemInfo gpi, string info){
        SetValues(gpi);
        SetText(info);
    }

    public void SetValues(Fase01_GetProblemInfo gpi){
        DimensaoPlataformaInicial = gpi.GetDimensaoPlataformaInicial();
        DimensaoPlataformaDoMeio = gpi.GetDimensaoPlataformaMeio();
        DimensaoPlataformaFinal = gpi.GetDimensaoPlataformaFinal();
        DistanciaEntreInicialEPulo = gpi.GetDistanciaEntreInicialEPulo();
        DistanciaEntrePuloEMeio = gpi.GetDistanciaEntrePuloEMeio();
        DistanciaEntreMeioEFinal = gpi.GetDistanciaEntreMeioEFinal();
    }

    private void SetText(){

        Debug.Log(UnitScale);
        string dIP = Mathf.Abs(DistanciaEntreInicialEPulo.z * UnitScale).ToString(format);
        string dPM = Mathf.Abs(DistanciaEntrePuloEMeio.z * UnitScale).ToString(format);
        string dMF = Mathf.Abs(DistanciaEntreMeioEFinal.z * UnitScale).ToString(format);
        string[] dimM = {
            Mathf.Abs(DimensaoPlataformaDoMeio.x * UnitScale).ToString(format),
            Mathf.Abs(DimensaoPlataformaDoMeio.z * UnitScale).ToString(format)
        };
        
        string[] sprites = {"<sprite=0>","<sprite=1>","<sprite=2>"};

        if(player.StartPlatformPosition == 1){
            sprites[2] = Imagens["InitialPlatform02"];
        }
        else{
            sprites[2] = Imagens["InitialPlatform01"];
        }
        
        info = "" +
            "O robô deve chegar na plataforma final passando pela plataforma do meio." +
            "\nEle inicia seu movimento na plataforma inicial e deve percorrer uma distância de D = " + dIP + " metros até pular." +
            "\n" + sprites[2] + "\n\n\n\n\n\n\n\n\n" +
            "\nA plataforma do meio está a uma distância W = " + dPM + " metros do ponto do pulo e possui " + dimM[0] + " metros de largura e " + dimM[1] + " metros de comprimento." +
            "\n" + sprites[0] + "\n\n\n\n\n\n\n\n\n" +
            "\nA plataforma final está a uma distância K = " + dMF + " metros da plataforma do meio." +
            "\n" + sprites[1] + "\n\n\n\n\n\n\n\n\n" +
            "\nVocê pode controlar as variáveis de aceleração e de ângulo do pulo." +
            "\nQuando o robô estiver na plataforma inicial, a aceleração é constante até o momento do pulo." +
            "";
        textUI.text = info;
    }

    private void SetText(string info){
        string dIP = Mathf.Abs(DistanciaEntreInicialEPulo.z * UnitScale).ToString(format);
        string dPM = Mathf.Abs(DistanciaEntrePuloEMeio.z * UnitScale).ToString(format);
        string dMF = Mathf.Abs(DistanciaEntreMeioEFinal.z * UnitScale).ToString(format);
        string[] dimM = {Mathf.Abs(DimensaoPlataformaDoMeio.x * UnitScale).ToString(format), Mathf.Abs(DimensaoPlataformaDoMeio.z * UnitScale).ToString(format)};
        
        string[] sprites = {Imagens["InitialAndMidPlatform"], Imagens["MidAndFinalPlatform"], Imagens["InitialPlatform01"]};

        if(player.StartPlatformPosition == 1){
            sprites[2] = Imagens["InitialPlatform02"];
        }
        else{
            sprites[2] = Imagens["InitialPlatform01"];
        }
        
        textUI.text = info;
    }

    private void InitializeImagens(){
        Imagens.Add("InitialAndMidPlatform", "<sprite=0>");
        Imagens.Add("MidAndFinalPlatform", "<sprite=1>");
        Imagens.Add("InitialPlatform01", "<sprite=2>");
        Imagens.Add("InitialPlatform02", "<sprite=3>");
    }
}
