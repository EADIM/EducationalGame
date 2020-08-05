using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetProblemInfo : MonoBehaviour
{
    public References references;
    public SetProblemInfo setProblemInfo;

    [SerializeField]
    public BoxCollider ColisorPlataformaInicial;
    [SerializeField]
    private BoxCollider ColisorPlataformaDoMeio;
    [SerializeField]
    private BoxCollider ColisorPlataformaFinal;
    [SerializeField]
    private GameObject CentroDoRaycastDoPulo;
    [SerializeField]
    private Vector3 dimensaoPlataformaInicial = Vector3.zero;
    [SerializeField]
    private Vector3 dimensaoPlataformaMeio = Vector3.zero;
    [SerializeField]
    private Vector3 dimensaoPlataformaFinal = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntreInicialEPulo = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntrePuloEMeio = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntreMeioEFinal = Vector3.zero;

    private void Awake() {
        ColisorPlataformaInicial = references.PlataformaInicial01.GetComponent<BoxCollider>();
        ColisorPlataformaDoMeio = references.PlataformaDoMeio.GetComponent<BoxCollider>();
        ColisorPlataformaFinal = references.PlataformaFinal.GetComponent<BoxCollider>();
        CentroDoRaycastDoPulo = references.RaycastCenter;
        SetVariables();
    }

    private void Start() {
        SetVariables();
    }

    public void OnIntialPlatformChange(){
        distanciaEntreInicialEPulo = CentroDoRaycastDoPulo.transform.position - ColisorPlataformaInicial.bounds.center;
        setProblemInfo.OnInfoChanged(this);
    }

    private void SetVariables(){
        dimensaoPlataformaInicial = ColisorPlataformaInicial.bounds.size;
        dimensaoPlataformaMeio = ColisorPlataformaDoMeio.bounds.size;
        dimensaoPlataformaFinal = ColisorPlataformaFinal.bounds.size;
        distanciaEntreInicialEPulo = CentroDoRaycastDoPulo.transform.position - ColisorPlataformaInicial.bounds.center;
        distanciaEntrePuloEMeio = ColisorPlataformaDoMeio.bounds.center - CentroDoRaycastDoPulo.transform.position;
        distanciaEntreMeioEFinal = ColisorPlataformaFinal.bounds.center - ColisorPlataformaDoMeio.bounds.center;
    }

    public Vector3 GetDimensaoPlataformaInicial(){
        return dimensaoPlataformaInicial;
    }

    public Vector3 GetDimensaoPlataformaMeio(){
        return dimensaoPlataformaMeio;
    }

    public Vector3 GetDimensaoPlataformaFinal(){
        return dimensaoPlataformaFinal;
    }

    public Vector3 GetDistanciaEntreInicialEPulo(){
        return distanciaEntreInicialEPulo;
    }

    public Vector3 GetDistanciaEntrePuloEMeio(){
        return distanciaEntrePuloEMeio;
    }

    public Vector3 GetDistanciaEntreMeioEFinal(){
        return distanciaEntreMeioEFinal;
    }

}
