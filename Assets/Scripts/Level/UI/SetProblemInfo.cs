using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProblemInfo : MonoBehaviour
{
    /*
    O objeto deve chegar na plataforma final passando pela plataforma do meio.
    A plataforma do meio está a X metros de distância da plataforma inicial e possui uma área de Y por Z metros.
    A plataforma final está a W metros da plataforma do meio e possui uma área de K por L metros.
    Você pode decidir a aceleração inicial do objeto e o ângulo do pulo. 
    */

    public References references;
    private GameState gms;
    private PlayerController player;
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

    private void Start() {
        player = references.Player.GetComponent<PlayerController>();
        gms = references.GameState.GetComponent<GameState>();
    }

    public void OnInfoChanged(GetProblemInfo gpi){
        SetValues(gpi);
        SetText();
    }

    public void SetValues(GetProblemInfo gpi){
        DimensaoPlataformaInicial = gpi.GetDimensaoPlataformaInicial();
        DimensaoPlataformaDoMeio = gpi.GetDimensaoPlataformaMeio();
        DimensaoPlataformaFinal = gpi.GetDimensaoPlataformaFinal();
        DistanciaEntreInicialEPulo = gpi.GetDistanciaEntreInicialEPulo();
        DistanciaEntrePuloEMeio = gpi.GetDistanciaEntrePuloEMeio();
        DistanciaEntreMeioEFinal = gpi.GetDistanciaEntreMeioEFinal();
    }

    private void SetText(){
        string dIP = Mathf.Abs(DistanciaEntreInicialEPulo.z * gms.UnitScale).ToString(format);
        string dPM = Mathf.Abs(DistanciaEntrePuloEMeio.z * gms.UnitScale).ToString(format);
        string dMF = Mathf.Abs(DistanciaEntreMeioEFinal.z * gms.UnitScale).ToString(format);
        string[] dimM = {Mathf.Abs(DimensaoPlataformaDoMeio.x * gms.UnitScale).ToString(format), Mathf.Abs(DimensaoPlataformaDoMeio.z * gms.UnitScale).ToString(format)};
        
        string[] sprites = {"<sprite=0>","<sprite=1>","<sprite=3>"};

        if(player.StartPlatformPosition == 1){
            sprites[2] = "<sprite=3>";
        }
        else{
            sprites[2] = "<sprite=2>";
        }
        
        info = "" +
            "O objeto deve chegar na plataforma final passando pela plataforma do meio." +
            "\nEle inicia seu movimento na plataforma inicial e deve percorrer uma distância de D = " + dIP + " metros até pular." +
            "\n" + sprites[2] + "\n\n\n\n\n\n\n\n\n" +
            "\nA plataforma do meio está a uma distância W = " + dPM + " metros do ponto do pulo e possui " + dimM[0] + " metros de largura e " + dimM[1] + " metros de comprimento." +
            "\n" + sprites[0] + "\n\n\n\n\n\n\n\n\n" +
            "\nA plataforma final está a uma distância K = " + dMF + " metros da plataforma do meio." +
            "\n" + sprites[1] + "\n\n\n\n\n\n\n\n\n" +
            "\nVocê pode controlar as variáveis de aceleração e de ângulo do pulo." +
            "\nQuando o objeto estiver na plataforma inicial, a aceleração é constante até o momento do pulo. Entretanto, quando ele estiver na plataforma do meio, o valor da aceleração será considerado como o valor da velocidade.";
        textUI.text = info;
    }
}
