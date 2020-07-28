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

    public GameState gms;
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
        info = "" +
            "O objeto deve chegar na plataforma final passando pela plataforma do meio." +
            "Ele inicia seu movimento na plataforma inicial e deve percorrer uma distância de " + dIP + " metros até pular." +
            "A plataforma do meio está a " + dPM + " metros de distância do ponto do pulo e possui " + dimM[0] + " metros de largura e " + dimM[1] + " metros de comprimento." +
            "A plataforma final está a " + dMF + " metros da plataforma do meio." +
            "Você pode decidir a aceleração inicial do objeto e o ângulo do pulo. ";
        textUI.text = info;
    }
}
